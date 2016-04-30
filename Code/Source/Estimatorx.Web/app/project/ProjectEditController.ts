/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProjectEditController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            '$uibModal',
            'logger',
            'identityService',
            'modelFactory',
            'projectCalculator',
            'projectRepository',
            'templateRepository',
            'organizationRepository'
        ];
        constructor(
            $scope,
            $location: angular.ILocationService,
            $uibModal: angular.ui.bootstrap.IModalService,
            logger: Logger,
            identityService: IdentityService,
            modelFactory: ModelFactory,
            projectCalculator: ProjectCalculator,
            projectRepository: ProjectRepository,
            templateRepository: TemplateRepository,
            organizationRepository: OrganizationRepository) {

            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;

            self.$location = $location;
            self.$uibModal = $uibModal;

            self.logger = logger;
            self.identityService = identityService;
            self.modelFactory = modelFactory;
            self.projectCalculator = projectCalculator;
            self.projectRepository = projectRepository;
            self.templateRepository = templateRepository;
            self.organizationRepository = organizationRepository;

            self.project = <IProject>{};

            // calculate on project change
            $scope.$watch(
                s => angular.toJson(s.viewModel.project),
                _.debounce(angular.bind(self, self.change), 500)
            );

            // watch for navigation
            $(window).bind('beforeunload', () => {
                // prevent navigation by returning string
                if (self.isDirty())
                    return 'You have unsaved changes!';
            });

            self.init();
        }

        $scope: any;
        $location: angular.ILocationService;
        $uibModal: angular.ui.bootstrap.IModalService;
        identityService: IdentityService;
        logger: Logger;
        modelFactory: ModelFactory;
        projectCalculator: ProjectCalculator;
        projectRepository: ProjectRepository;

        original: IProject;
        project: IProject;
        projectId: string;

        template: ITemplate;
        templates: ITemplate[];
        templateRepository: TemplateRepository;

        organizations: IOrganization[];
        organizationRepository: OrganizationRepository;

        init() {
            var self = this;

            self.templateRepository.all()
                .success((data, status, headers, config) => {
                    self.templates = data;
                })
                .error(self.logger.handelErrorProxy);

            self.organizationRepository.all()
                .success((data, status, headers, config) => {
                    self.organizations = data;
                })
                .error(self.logger.handelErrorProxy);
        }

        load(id?: string) {
            var self = this;

            self.projectId = id;

            // get project id
            if (!self.projectId) {
                self.project = self.modelFactory.createProject();
                return;
            }

            this.projectRepository.find(self.projectId)
                .success((data, status, headers, config) => {
                    self.loadDone(data);
                })
                .error((data, status, headers, config) => {
                    if (status == 404) {
                        self.project = self.modelFactory.createProject(self.projectId);
                        return;
                    }

                    self.logger.handelError(data, status, headers, config);
                });
        }

        loadDone(project: IProject) {
            var self = this;

            self.original = <IProject>angular.copy(project, {});
            self.project = project;

            self.setClean();
        }

        save(valid: boolean) {
            var self = this;

            if (!valid) {
                self.logger.showAlert({
                    type: 'error',
                    title: 'Validation Error',
                    message: 'A form field has a validation error. Please fix the error to continue.',
                    timeOut: 4000
                });

                return;
            }

            this.projectRepository.save(this.project)
                .success((data, status, headers, config) => {
                    self.loadDone(data);
                    self.logger.showAlert({
                        type: 'success',
                        title: 'Save Successful',
                        message: 'Project saved successfully.',
                        timeOut: 4000
                    });
                })
                .error(self.logger.handelErrorProxy);
        }

        change() {
            var self = this;

            self.calculate();
        }

        undo() {
            var self = this;

            BootstrapDialog.confirm("Are you sure you want to undo changes?", (result) => {
                if (!result)
                    return;

                self.project = <IProject>angular.copy(self.original, {});

                self.setClean();

                self.$scope.$applyAsync();
            });
        }

        calculate() {
            this.projectCalculator.updateTotals(this.project);
            this.$scope.$apply();
        }

        delete() {
            var self = this;

            BootstrapDialog.confirm("Are you sure you want to delete this project?",(result) => {
                if (!result)
                    return;

                self.projectRepository.delete(self.project.Id)
                    .success((data, status, headers, config) => {
                        self.logger.showAlert({
                            type: 'success',
                            title: 'Delete Successful',
                            message: 'Project deleted successfully.',
                            timeOut: 4000
                        });
                        
                        //redirect
                        window.location.href = 'Project';
                    })
                    .error(self.logger.handelErrorProxy);
            });

        }

        isDirty(): boolean {
            return this.$scope.projectForm.$dirty;
        }

        setDirty() {
            this.$scope.projectForm.$setDirty();
        }

        setClean() {
            this.$scope.projectForm.$setPristine();
            this.$scope.projectForm.$setUntouched();
        }


        addAssumption() {
            if (!this.project.Assumptions)
                this.project.Assumptions = [];

            var assumption = '';
            this.project.Assumptions.push(assumption);

            this.setDirty();
        }

        removeAssumption(index: number) {

            BootstrapDialog.confirm("Are you sure you want to remove this assumption?", (result) => {
                if (!result)
                    return;

                this.project.Assumptions.splice(index, 1);

                this.setDirty();
                this.$scope.$apply();
            });
        }

        reorderAssumptions() {
            var self = this;

            var modalInstance = self.$uibModal.open({
                templateUrl: 'textReorderModal.html',
                controller: 'reorderModalController',
                resolve: {
                    name: () => 'Assumptions',
                    items: () => self.project.Assumptions
                }
            });

            modalInstance.result.then((items: any[]) => {
                self.project.Assumptions = items;
                self.setDirty();
            });
        }


        addFactor() {
            if (!this.project.Factors)
                this.project.Factors = [];

            var factor = this.modelFactory.createFactor();
            this.project.Factors.push(factor);

            this.setDirty();
        }

        removeFactor(factor: IFactor) {
            if (!factor)
                return;

            BootstrapDialog.confirm("Are you sure you want to remove this factor?", (result) => {
                if (!result)
                    return;

                for (var i = 0; i < this.project.Factors.length; i++) {
                    if (this.project.Factors[i].Id == factor.Id) {
                        this.project.Factors.splice(i, 1);
                        break;
                    }
                }

                this.setDirty();
                this.$scope.$apply();
            });
        }

        reorderFactors() {
            var self = this;

            var modalInstance = self.$uibModal.open({
                templateUrl: 'nameReorderModal.html',
                controller: 'reorderModalController',
                resolve: {
                    name: () => 'Factors',
                    items: () => self.project.Factors
                }
            });

            modalInstance.result.then((items: any[]) => {
                self.project.Factors = items;
                self.setDirty();
            });
        }


        addSection() {
            if (!this.project.Sections)
                this.project.Sections = [];

            var section = this.modelFactory.createSection();
            this.project.Sections.push(section);

            this.setDirty();
        }

        removeSection(section: ISection) {
            if (!section)
                return;

            if (!this.project.Sections)
                return;

            if (section.Tasks && section.Tasks.length) {
                BootstrapDialog.alert("Section not empty. Remove all tasks before removing section.");
                return;
            }


            BootstrapDialog.confirm("Are you sure you want to remove this section?", (result) => {
                if (!result)
                    return;

                for (var i = 0; i < this.project.Sections.length; i++) {
                    if (this.project.Sections[i].Id == section.Id) {
                        this.project.Sections.splice(i, 1);
                        break;
                    }
                }

                this.setDirty();
                this.$scope.$apply();
            });
        }

        reorderSections() {
            var self = this;

            var modalInstance = self.$uibModal.open({
                templateUrl: 'nameReorderModal.html',
                controller: 'reorderModalController',
                resolve: {
                    name: () => 'Sections',
                    items: () => self.project.Sections
                }
            });

            modalInstance.result.then((items: any[]) => {
                self.project.Sections = items;
                self.setDirty();
            });
        }


        addTask(section: ISection) {
            if (!section)
                return;

            if (!section.Tasks)
                section.Tasks = [];

            var task = this.modelFactory.createTask();
            task.Name = 'Task ' + section.Tasks.length;

            section.Tasks.push(task);
            this.setDirty();
        }

        removeTask(section: ISection, task: ITask) {
            if (!section || !task)
                return;

            if (!section.Tasks)
                return;

            BootstrapDialog.confirm("Are you sure you want to remove this task?", (result) => {
                if (!result)
                    return;

                for (var i = 0; i < section.Tasks.length; i++) {
                    if (section.Tasks[i].Id == task.Id) {
                        section.Tasks.splice(i, 1);
                        break;
                    }
                }

                this.setDirty();
                this.$scope.$apply();
            });
        }

        reorderTasks(section: ISection) {
            var self = this;

            var modalInstance = self.$uibModal.open({
                templateUrl: 'nameReorderModal.html',
                controller: 'reorderModalController',
                resolve: {
                    name: () => 'Tasks',
                    items: () => section.Tasks
                }
            });

            modalInstance.result.then((items: any[]) => {
                section.Tasks = items;
                self.setDirty();
            });

        }


        addTemplate() {
            var self = this;
            if (!self.project.Factors)
                self.project.Factors = [];

            var modalInstance = self.$uibModal.open({
                templateUrl: 'templateModal.html',
                controller: 'templateModalController',
                resolve: {
                    items: () => self.templates
                }
            });

            modalInstance.result.then((item: ITemplate) => {
                angular.forEach(item.Factors,(value, key) => {

                    // change key to prevent duplicates
                    var factor = <IFactor>angular.copy(value, {});
                    factor.Id = self.identityService.newObjectId();

                    self.project.Factors.push(factor);
                });

                this.setDirty();
            });

        }


        addSecurityKey() {
            var self = this;

            self.project.SecurityKey = this.identityService.newSecurityKey();
            self.setDirty();

            self.logger.showAlert({
                type: 'success',
                title: 'Link Successful',
                message: 'Link created succesfully. You must save the project before using the new link.',
                timeOut: 8000
            });
        }

        removeSecurityKey() {
            var self = this;

            BootstrapDialog.confirm("Are you sure you want to remove the shared link?",(result) => {
                if (!result)
                    return;

                self.project.SecurityKey = null;
                self.setDirty();
                self.$scope.$applyAsync();
            });
        }

        shareLink(relitive: boolean = false): string {
            var self = this;

            if (relitive) {
                return "Project/Share/" + self.project.Id + "/" + self.project.SecurityKey;
            }

            var url = self.$location.protocol() + "://"
                + self.$location.host()
                + angular.element('base').attr('href')
                + "Project/Share/"
                + self.project.Id + "/" + self.project.SecurityKey;

            return url;
        }

        shareMessage() {
            this.logger.showAlert({
                type: 'success',
                title: 'Copy Successful',
                message: 'Project share link copied to clipboard.',
                timeOut: 4000
            });
        }
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('projectEditController', [
            '$scope',
            '$location',
            '$uibModal',
            'logger',
            'identityService',
            'modelFactory',
            'projectCalculator',
            'projectRepository',
            'templateRepository',
            'organizationRepository',

            ProjectEditController // controller must be last
        ]);
}


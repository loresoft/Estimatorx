/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProjectEditController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            '$modal',
            'identityService',
            'modelFactory',
            'projectCalculator',
            'projectRepository',
            'templateRepository'
        ];
        constructor(
            $scope,
            $location: ng.ILocationService,
            $modal: any,
            identityService: IdentityService,
            modelFactory: ModelFactory,
            projectCalculator: ProjectCalculator,
            projectRepository: ProjectRepository,
            templateRepository: TemplateRepository)
        {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$location = $location;
            self.$modal = $modal;

            self.identityService = identityService;
            self.modelFactory = modelFactory;
            self.projectCalculator = projectCalculator;
            self.projectRepository = projectRepository;
            self.templateRepository = templateRepository;

            self.project = <IProject>{};

            // calculate on project change
            $scope.$watch(
                s => angular.toJson(s.viewModel.project),
                _.debounce($.proxy(self.calculate, self), 500));

            self.init();
        }

        $scope: ng.IScope;
        $location: ng.ILocationService;
        $modal: any;
        identityService: IdentityService;
        modelFactory: ModelFactory;
        projectCalculator: ProjectCalculator;
        projectRepository: ProjectRepository;

        project: IProject;
        projectId: string;

        template: ITemplate;
        templates: ITemplate[];
        templateRepository: TemplateRepository;

        init() {
            var self = this;
            self.templateRepository.all()
                .success((data, status, headers, config) => {
                    self.templates = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
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
                    self.project = data;
                })
                .error((data, status, headers, config) => {
                    if (status == 404) {
                        self.project = self.modelFactory.createProject(self.projectId);
                        return;
                    }
                        
                    // TODO show error
                });
        }

        save() {
            var self = this;

            this.projectRepository.save(this.project)
                .success((data, status, headers, config) => {
                    self.project = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        calculate() {
            console.log("project calculate");

            this.projectCalculator.updateTotals(this.project);
            this.$scope.$apply();
        }


        addAssumption() {
            if (!this.project.Assumptions)
                this.project.Assumptions = [];

            var assumption = this.modelFactory.createAssumption();
            this.project.Assumptions.push(assumption);
        }

        removeAssumption(assumption: IAssumption) {
            if (!assumption)
                return;

            BootstrapDialog.confirm("Are you sure you want to remove this assumption?", (result) => {
                if (!result)
                    return;

                for (var i = 0; i < this.project.Assumptions.length; i++) {
                    if (this.project.Assumptions[i].Id == assumption.Id) {
                        this.project.Assumptions.splice(i, 1);
                        break;
                    }
                }
                this.$scope.$apply();
            });
        }


        addFactor() {
            if (!this.project.Factors)
                this.project.Factors = [];

            var factor = this.modelFactory.createFactor();
            this.project.Factors.push(factor);
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
                this.$scope.$apply();
            });
        }


        addSection() {
            if (!this.project.Sections)
                this.project.Sections = [];

            var section = this.modelFactory.createSection();
            this.project.Sections.push(section);
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
                this.$scope.$apply();
            });
        }


        addTask(section: ISection) {
            if (!section)
                return;

            if (!section.Tasks)
                section.Tasks = [];

            var task = <ITask>{
                Id: this.identityService.newUUID(),
                Name: 'Task ' + section.Tasks.length,
                IsActive: true
            };

            section.Tasks.push(task);
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
                this.$scope.$apply();
            });
        }

        addTemplate() {
            var self = this;

            console.log('Add Template');

            var modalInstance = self.$modal.open({
                templateUrl: 'templateModal.html',
                controller: 'templateModalController',
                resolve: {
                    items: () => self.templates
                }
            });

            modalInstance.result.then((item: ITemplate) => {
                console.log('Select Template: ' + angular.toJson(item));

                angular.forEach(item.Factors, (value, key) => {
                    self.project.Factors.push(value);
                });
            });

        }
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('projectEditController', [
            '$scope',
            '$location',
            '$modal',
            'identityService',
            'modelFactory',
            'projectCalculator',
            'projectRepository',
            'templateRepository',

            ProjectEditController // controller must be last
        ]);

}


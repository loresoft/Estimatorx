/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class TemplateEditController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            'logger',
            'modelFactory',
            'templateRepository',
            'organizationRepository'
        ];

        constructor(
            $scope,
            $location: ng.ILocationService,
            logger: Logger,
            modelFactory: ModelFactory,
            templateRepository: TemplateRepository,
            organizationRepository: OrganizationRepository
        )
        {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;

            self.$location = $location;

            self.modelFactory = modelFactory;
            self.logger = logger;
            self.templateRepository = templateRepository;
            self.organizationRepository = organizationRepository;

            self.template = <ITemplate>{};

            self.init();
        }

        $scope: ng.IScope;
        $location: ng.ILocationService;
        logger: Logger;
        modelFactory: ModelFactory;

        templateRepository: TemplateRepository;
        template: ITemplate;
        templateId: string;

        organizations: IOrganization[];
        organizationRepository: OrganizationRepository;

        init() {
            var self = this;

            self.organizationRepository.all()
                .success((data, status, headers, config) => {
                    self.organizations = data;
                })
                .error(self.logger.handelError);
        }

        load(id?: string) {
            var self = this;

            self.templateId = id;

            // get template id
            if (!self.templateId) {
                self.template = self.modelFactory.createTemplate();
                return;
            }

            this.templateRepository.find(self.templateId)
                .success((data, status, headers, config) => {
                    self.template = data;
                })
                .error((data, status, headers, config) => {
                    if (status == 404) {
                        self.template = self.modelFactory.createTemplate(self.templateId);
                        return;
                    }

                    self.logger.handelError(data, status, headers, config);
                });
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

            this.templateRepository.save(this.template)
                .success((data, status, headers, config) => {
                    self.template = data;                    
                    self.logger.showAlert({
                        type: 'success',
                        title: 'Save Successful',
                        message: 'Template saved successfully.',
                        timeOut: 4000
                    });
                })
                .error(self.logger.handelError);

        }

        addFactor() {
            if (!this.template.Factors)
                this.template.Factors = [];

            var factor = this.modelFactory.createFactor();
            this.template.Factors.push(factor);
        }

        removeFactor(factor: IFactor) {
            if (!factor)
                return;

            BootstrapDialog.confirm("Are you sure you want to remove this factor?", (result) => {
                if (!result)
                    return;

                for (var i = 0; i < this.template.Factors.length; i++) {
                    if (this.template.Factors[i].Id == factor.Id) {
                        this.template.Factors.splice(i, 1);
                        break;
                    }
                }
                this.$scope.$apply();
            });
        }


    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('templateEditController',
        [
            '$scope',
            '$location',
            'logger',
            'modelFactory',
            'templateRepository',
            'organizationRepository',
            TemplateEditController
        ]
    );
}


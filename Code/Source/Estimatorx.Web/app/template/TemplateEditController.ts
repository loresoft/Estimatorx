/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class TemplateEditController extends ControllerBase {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            'modelFactory',
            'templateRepository',
            'organizationRepository'
        ];

        constructor(
            $scope,
            $location: ng.ILocationService,
            modelFactory: ModelFactory,
            templateRepository: TemplateRepository,
            organizationRepository: OrganizationRepository
        )
        {
            // call base class
            super($scope);

            var self = this;

            self.$location = $location;

            self.modelFactory = modelFactory;
            self.templateRepository = templateRepository;
            self.organizationRepository = organizationRepository;

            self.template = <ITemplate>{};

            self.init();
        }

        $scope: ng.IScope;
        $location: ng.ILocationService;
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
                .error(self.handelError);
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

                    self.handelError(data, status, headers, config);
                });
        }

        save() {
            var self = this;

            this.templateRepository.save(this.template)
                .success((data, status, headers, config) => {
                    self.template = data;
                })
                .error(self.handelError);
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
            'modelFactory',
            'templateRepository',
            'organizationRepository',
            TemplateEditController
        ]
    );
}


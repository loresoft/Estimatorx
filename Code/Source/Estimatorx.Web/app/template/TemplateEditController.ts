/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class TemplateEditController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            'modelFactory',
            'templateRepository'
        ];

        constructor(
            $scope,
            $location: ng.ILocationService,
            modelFactory: ModelFactory,
            templateRepository: TemplateRepository)
        {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$location = $location;

            self.modelFactory = modelFactory;
            self.templateRepository = templateRepository;
            self.template = <ITemplate>{};
        }

        $scope: ng.IScope;
        $location: ng.ILocationService;
        modelFactory: ModelFactory;
        templateRepository: TemplateRepository;
        template: ITemplate;
        templateId: string;

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

                    // TODO show error
                });
        }

        save() {
            var self = this;

            this.templateRepository.save(this.template)
                .success((data, status, headers, config) => {
                    self.template = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
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
            TemplateEditController
        ]
    );
}


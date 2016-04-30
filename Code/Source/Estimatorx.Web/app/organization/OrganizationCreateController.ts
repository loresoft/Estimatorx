/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class OrganizationCreateController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            'logger',
            'modelFactory',
            'organizationRepository'
        ];

        constructor(
            $scope,
            $location: angular.ILocationService,
            logger: Logger,
            modelFactory: ModelFactory,
            organizationRepository: OrganizationRepository
        ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$location = $location;

            self.logger = logger;
            self.modelFactory = modelFactory;
            self.organizationRepository = organizationRepository;

            self.organization = <IOrganization>{};

        }

        $scope: any;
        $location: angular.ILocationService;

        logger: Logger;
        modelFactory: ModelFactory;

        organizationRepository: OrganizationRepository;
        organization: IOrganization;
        organizationId: string;

        userId: string;

        selfOwner() {
            return true;
        }

        load(userId?: string) {
            var self = this;

            self.userId = userId;
            self.organization = self.modelFactory.createOrganization(null, userId);
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

            this.organizationRepository.save(this.organization)
                .success((data, status, headers, config) => {
                    self.organization = data;

                    self.logger.showAlert({
                        type: 'success',
                        title: 'Save Successful',
                        message: 'Organization saved successfully.',
                        timeOut: 4000
                    });

                    // redirect to edit
                    window.location.href = 'Organization/Edit/' + data.Id;
                })
                .error(self.logger.handelErrorProxy);
        }
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('organizationCreateController', [
            '$scope',
            '$location',
            'logger',
            'modelFactory',
            'organizationRepository',
            OrganizationCreateController
        ]);
}


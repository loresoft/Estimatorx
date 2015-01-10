/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProfileEditController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            'logger',
            'modelFactory',
            'userRepository'
        ];

        constructor(
            $scope,
            $location: ng.ILocationService,
            logger: Logger,
            modelFactory: ModelFactory,
            userRepository: UserRepository) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$location = $location;

            self.logger = logger;
            self.modelFactory = modelFactory;
            self.userRepository = userRepository;
            self.user = <IUser>{};
            self.password = <IPassword>{};
        }

        $scope: any;
        $location: ng.ILocationService;
        logger: Logger;
        modelFactory: ModelFactory;

        userRepository: UserRepository;
        user: IUser;
        userId: string;

        password: IPassword;

        load(id?: string) {
            var self = this;

            self.userId = id;

            // get template id
            if (!self.userId) {
                return;
            }

            this.userRepository.profile()
                .success((data, status, headers, config) => {
                    self.user = data;
                })
                .error(self.logger.handelErrorProxy);
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

            this.userRepository.save(this.user)
                .success((data, status, headers, config) => {
                    self.user = data;
                    self.logger.showAlert({
                        type: 'success',
                        title: 'Save Successful',
                        message: 'Profile saved successfully.',
                        timeOut: 4000
                    });
                })
                .error(self.logger.handelErrorProxy);
        }

        changePassword(valid: boolean) {
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

            var func = (self.user.PasswordHash)
                ? angular.bind(self.userRepository, self.userRepository.changePassword, self.password)
                : angular.bind(self.userRepository, self.userRepository.setPassword, self.password);

            func()
                .success((data, status, headers, config) => {
                    self.logger.showAlert({
                        type: 'success',
                        title: 'Save Successful',
                        message: 'Password changed successfully.',
                        timeOut: 4000
                    });

                    self.resetValues();
                    self.load(self.userId);
                })
                .error(self.logger.handelErrorProxy);
        }

        removeLogin(provider: string, key: string) {
            var self = this;
            this.userRepository.removeLogin(provider, key)
                .success((data, status, headers, config) => {
                    self.load(self.userId);
                })
                .error(self.logger.handelErrorProxy);
        }

        resetValues() {
            var self = this;

            self.password = <IPassword>{};
            self.$scope.passwordForm.$setPristine();
            self.$scope.passwordForm.$setUntouched();
        }
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('profileEditController',
        [
            '$scope',
            '$location',
            'logger',
            'modelFactory',
            'userRepository',
            ProfileEditController
        ]);
}


/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProfileEditController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$location',
            'modelFactory',
            'userRepository'
        ];

        constructor(
            $scope,
            $location: ng.ILocationService,
            modelFactory: ModelFactory,
            userRepository: UserRepository) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$location = $location;

            self.modelFactory = modelFactory;
            self.userRepository = userRepository;
            self.user = <IUser>{};
            self.password = <IPassword>{};
        }

        $scope: any;
        $location: ng.ILocationService;
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

            this.userRepository.find(self.userId)
                .success((data, status, headers, config) => {
                    self.user = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        save(valid: boolean) {
            var self = this;
            if (!valid) {
                // TODO show error
                return;
            }

            this.userRepository.save(this.user)
                .success((data, status, headers, config) => {
                    self.user = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        changePassword(valid: boolean) {
            var self = this;
            if (!valid) {
                // TODO show error
                return;
            }

            var func = (self.user.PasswordHash)
                ? angular.bind(self.userRepository, self.userRepository.changePassword, self.password)
                : angular.bind(self.userRepository, self.userRepository.setPassword, self.password);

            func()
                .success((data, status, headers, config) => {
                    // TODO show changed
                    self.resetPassword();
                    self.load(self.userId);
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        removeLogin(provider: string, key: string) {
            var self = this;
            this.userRepository.removeLogin(provider, key)
                .success((data, status, headers, config) => {
                    // TODO show changed
                    self.load(self.userId);
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        resetPassword() {
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
            'modelFactory',
            'userRepository',
            ProfileEditController
        ]
        );
}


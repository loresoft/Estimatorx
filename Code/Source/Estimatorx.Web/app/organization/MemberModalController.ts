/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class MemberModalController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$modalInstance',
            'userRepository'
        ];

        constructor(
            $scope,
            $modalInstance, 
            userRepository: UserRepository
        ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;

            self.$scope = $scope;
            self.$modalInstance = $modalInstance;

            self.userRepository = userRepository;
        }

        $scope: any;
        $modalInstance: any;

        userRepository: UserRepository;

        users: IUser[];
        userId: string;

        searchUsers(search: string): void {
            var self = this;

            if (!search)
                return;

            self.userRepository.search(search)
                .success((data, status, headers, config) => {
                    self.users = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        select() {
            var self = this;
            self.$modalInstance.close(self.userId);

        }

        cancel() {
            var self = this;
            self.$modalInstance.dismiss('cancel');
        }

    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('memberModalController', [
            '$scope',
            '$modalInstance',
            'userRepository',
            MemberModalController
        ]);
}
 
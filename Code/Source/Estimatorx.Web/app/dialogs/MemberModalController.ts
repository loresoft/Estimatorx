/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class MemberModalController implements angular.IController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$uibModalInstance',
            'logger',
            'userRepository'
        ];

        constructor(
            $scope,
            $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance,
            logger: Logger, 
            userRepository: UserRepository
        ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;

            self.$scope = $scope;
            self.$uibModalInstance = $uibModalInstance;

            self.logger = logger;
            self.userRepository = userRepository;
        }

        $scope: any;
        $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance;
        logger: Logger;

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
                .error(self.logger.handelErrorProxy);
        }

        select() {
            var self = this;
            self.$uibModalInstance.close(self.userId);

        }

        cancel() {
            var self = this;
            self.$uibModalInstance.dismiss('cancel');
        }

        $onInit = () => { };
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('memberModalController', MemberModalController);
}
 
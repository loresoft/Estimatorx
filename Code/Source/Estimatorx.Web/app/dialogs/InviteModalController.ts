/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class InviteModalController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$uibModalInstance',
            'logger'
        ];

        constructor(
            $scope,
            $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance,
            logger: Logger) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;

            self.$scope = $scope;
            self.$uibModalInstance = $uibModalInstance;

            self.logger = logger;
        }

        $scope: any;
        $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance;
        logger: Logger;

        userRepository: UserRepository;

        inviteEmail: string;
        
        send(valid: boolean) {
            var self = this;

            if (!valid) {               
                return;
            }

            self.$uibModalInstance.close(self.inviteEmail);
        }

        cancel() {
            var self = this;
            self.$uibModalInstance.dismiss('cancel');
        }

    }

    // register controller
    angular.module(Estimatorx.applicationName).controller('inviteModalController', [
        '$scope',
        '$uibModalInstance',
        'logger',
        InviteModalController
    ]);
}
  
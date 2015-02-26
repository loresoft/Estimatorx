/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class InviteModalController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$modalInstance',
            'logger'
        ];

        constructor(
            $scope,
            $modalInstance,
            logger: Logger) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;

            self.$scope = $scope;
            self.$modalInstance = $modalInstance;

            self.logger = logger;
        }

        $scope: any;
        $modalInstance: any;
        logger: Logger;

        userRepository: UserRepository;

        inviteEmail: string;
        
        send(valid: boolean) {
            var self = this;

            if (!valid) {               
                return;
            }

            self.$modalInstance.close(self.inviteEmail);
        }

        cancel() {
            var self = this;
            self.$modalInstance.dismiss('cancel');
        }

    }

    // register controller
    angular.module(Estimatorx.applicationName).controller('inviteModalController', [
        '$scope',
        '$modalInstance',
        'logger',
        InviteModalController
    ]);
}
  
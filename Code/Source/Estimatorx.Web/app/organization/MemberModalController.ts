/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class MemberModalController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$modalInstance',
            'logger',
            'userRepository'
        ];

        constructor(
            $scope,
            $modalInstance,
            logger: Logger, 
            userRepository: UserRepository
        ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;

            self.$scope = $scope;
            self.$modalInstance = $modalInstance;

            self.logger = logger;
            self.userRepository = userRepository;
        }

        $scope: any;
        $modalInstance: any;
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
                .error(self.logger.handelError);
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
            'logger',
            'userRepository',
            MemberModalController
        ]);
}
 
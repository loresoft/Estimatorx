/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class UserListController {

        // project for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            'logger',
            'userRepository',
            'organizationRepository'
        ];

        constructor($scope,
            logger: Logger,
            userRepository: UserRepository,
            organizationRepository: OrganizationRepository
            ) {
            // assign viewModel to controller
            $scope.viewModel = this;

            this.logger = logger;
            this.userRepository = userRepository;
            this.organizationRepository = organizationRepository;

            // default
            this.result = <IQueryResult<IUser>>{
                Page: 1,
                PageSize: 10,
                Sort: 'Updated',
                Descending: true
            };

            this.init();
            this.load();
        }


        logger: Logger;
        userRepository: UserRepository;

        searchText: string;
        organizationId: string;

        organizations: IOrganization[];
        organizationRepository: OrganizationRepository;
        result: IQueryResult<IUser>;
        sort = angular.bind(this, this.load);

        init() {
            var self = this;

            self.organizationRepository.all()
                .success((data, status, headers, config) => {
                    self.organizations = data;
                })
                .error(self.logger.handelErrorProxy);
        }

        load() {
            var self = this;

            var request = <IQuerySearch>{
                Page: self.result.Page,
                PageSize: self.result.PageSize,
                Sort: self.result.Sort,
                Descending: self.result.Descending,
                Search: self.searchText,
            };

            this.userRepository.query(request)
                .success((data, status, headers, config) => {
                    self.result = data;
                })
                .error(self.logger.handelErrorProxy);
        }

        sortClick(column: string) {
            var self = this;

            if (self.result.Sort == column)
                self.result.Descending = !self.result.Descending;
            else
                self.result.Descending = false;

            self.result.Sort = column;

            self.load();
        }

        search() {
            var self = this;
            self.load();
        }
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('userListController',
        [
            '$scope',
            'logger',
            'userRepository',
            'organizationRepository',
            UserListController
        ]);
}


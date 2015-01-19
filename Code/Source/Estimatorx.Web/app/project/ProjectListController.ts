/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProjectListController {

        // project for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            'logger',
            'projectRepository',
            'organizationRepository'
        ];

        constructor($scope,
            logger: Logger,
            projectRepository: ProjectRepository,
            organizationRepository: OrganizationRepository
            ) {
            // assign viewModel to controller
            $scope.viewModel = this;

            this.logger = logger;
            this.projectRepository = projectRepository;
            this.organizationRepository = organizationRepository;

            // default
            this.result = <IQueryResult<IProject>>{
                Page: 1,
                PageSize: 10,
                Sort: 'Updated',
                Descending: true
            };

            this.init();
            this.load();
        }


        logger: Logger;
        projectRepository: ProjectRepository;

        searchText: string;
        organizationId: string;

        organizations: IOrganization[];
        organizationRepository: OrganizationRepository;
        result: IQueryResult<IProject>;
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
                Organization: self.organizationId
            };

            this.projectRepository.query(request)
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
        .controller('projectListController',
        [
            '$scope',
            'logger',
            'projectRepository',
            'organizationRepository',
            ProjectListController
        ]);
}


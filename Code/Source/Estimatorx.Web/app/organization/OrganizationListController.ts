/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class OrganizationListController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            'logger',
            'organizationRepository'
        ];

        constructor($scope, logger: Logger, organizationRepository: OrganizationRepository) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;

            self.logger = logger;
            self.repository = organizationRepository;

            // default
            self.result = <IQueryResult<IOrganization>>{
                Page: 1,
                PageSize: 10,
                Sort: 'Name'
            };

            self.load();

        }

        $scope: any;
        logger: Logger;
        repository: OrganizationRepository;

        result: IQueryResult<IOrganization>;
        sort = angular.bind(this, this.load);

        load() {
            var self = this;

            var request = <IQueryRequest>{
                Page: self.result.Page,
                PageSize: self.result.PageSize,
                Sort: self.result.Sort,
                Descending: self.result.Descending
            };

            this.repository.query(request)
                .success((data, status, headers, config) => {
                    self.result = data;
                })
                .error(self.logger.handelError);
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

    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('organizationListController', [
            '$scope',
            'logger',
            'organizationRepository',
            OrganizationListController
        ]);
}


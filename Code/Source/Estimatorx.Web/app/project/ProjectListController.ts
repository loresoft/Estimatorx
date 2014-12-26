/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProjectListController {

        // project for minification, must match contructor signiture.
        static $inject = ['$scope', 'projectSummaryRepository'];
        constructor($scope, projectSummaryRepository: ProjectSummaryRepository) {
            // assign viewModel to controller
            $scope.viewModel = this;
            this.repository = projectSummaryRepository;

            // default
            this.result = <IQueryResult<IProject>>{
                Page: 1,
                PageSize: 10,
                Sort: 'Updated',
                Descending: true
            };

            this.load();
        }


        repository: ProjectSummaryRepository;

        result: IQueryResult<IProject>;
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
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        sortClick(column:string) {
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
        .controller('projectListController',
        [
            '$scope',
            'projectSummaryRepository',
            ProjectListController
        ]
        );
}


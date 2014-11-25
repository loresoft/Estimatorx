/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export class EstimateListController {

        // project for minification, must match contructor signiture.
        static $inject = ['$scope', 'projectSummaryRepository'];
        constructor($scope, projectSummaryRepository: ProjectSummaryRepository) {
            // assign viewModel to controller
            $scope.viewModel = this;
            this.repository = projectSummaryRepository;

            // default
            this.result = <IQueryResult<IProject>>{
                Page: 1,
                PageSize: 2
            };

            this.load();
        }


        repository: ProjectSummaryRepository

        result: IQueryResult<IProject>;

        load() {
            var self = this;

            var request = <IQueryRequest>{
                Page: self.result.Page,
                PageSize: self.result.PageSize,
                Sort: self.result.Sort,
                Descending: self.result.Descending,
            }

            this.repository.query(request)
                .success((data, status, headers, config) => {
                    self.result = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        sort = angular.bind(this, this.load)
    }

    // register controller
    angular.module(Estimator.applicationName)
        .controller('estimateListController',
        [
            '$scope',
            'projectSummaryRepository',
            EstimateListController
        ]
        );
}


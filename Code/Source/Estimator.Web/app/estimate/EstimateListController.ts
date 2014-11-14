/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export class EstimateListController {

        // project for minification, must match contructor signiture.
        static $inject = ['$scope'];
        constructor($scope) {
            // assign viewModel to controller
            $scope.viewModel = this;
        }

        count: number = 11;

    }

    // register controller
    angular.module(Estimator.applicationName)
        .controller('estimateListController',
        [
            '$scope',
            EstimateListController
        ]
    );
}


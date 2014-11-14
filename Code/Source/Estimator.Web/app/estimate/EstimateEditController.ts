/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";
    
    export class EstimateEditController {

        // project for minification, must match contructor signiture.
        static $inject = ['$scope'];
        constructor($scope) {
            // assign viewModel to controller
            $scope.viewModel = this;
        }

        count: number = 12;
    }

    // register controller
    angular.module(Estimator.applicationName)
        .controller('estimateEditController',
        [
            '$scope',
            EstimateEditController
        ]
    );
}


/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export class FactorEditController {

        // project for minification, must match contructor signiture.
        static $inject = ['$scope'];

        constructor($scope) {
            // assign viewModel to controller
            $scope.viewModel = this;
        }

        count: number = 22;

    }

    // register controller
    angular.module(Estimator.applicationName)
        .controller('factorEditController',
        [
            '$scope',
            FactorEditController
        ]
    );
}


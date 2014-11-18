/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export class HomeController {

        // project for minification, must match contructor signiture.
        static $inject = ['$scope'];
        constructor($scope) {
            
            // assign viewModel to controller
            $scope.viewModel = this;
            this.$scope = $scope;
        }

        $scope: ng.IScope;
    }

    // register controller
    angular.module(Estimator.applicationName)
        .controller('homeController', [
            '$scope',

            HomeController // controller must be last
        ]);
}


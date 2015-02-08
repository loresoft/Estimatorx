/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ReorderModalController {

        // protect for minification, must match constructor signiture.
        static $inject = [
            '$scope',
            '$modalInstance',
            'name',
            'items'
        ];

        constructor(
            $scope,
            $modalInstance,
            name: string,
            items: any[]
        ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$modalInstance = $modalInstance;
            self.items = angular.copy(items);
            self.name = name;
        }

        $scope: any;
        $modalInstance: any;
        items: any[];
        name: string;

        select() {
            var self = this;
            self.$modalInstance.close(self.items);
        }

        cancel() {
            var self = this;
            self.$modalInstance.dismiss('cancel');
        }

    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('reorderModalController',
        [
            '$scope',
            '$modalInstance',
            'name',
            'items',
            ReorderModalController
        ]);
}

 
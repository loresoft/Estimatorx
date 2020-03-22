/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ReorderModalController implements angular.IController  {

        // protect for minification, must match constructor signiture.
        static $inject = [
            '$scope',
            '$uibModalInstance',
            'name',
            'items'
        ];

        constructor(
            $scope,
            $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance,
            name: string,
            items: any[]
        ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$uibModalInstance = $uibModalInstance;
            self.items = angular.copy(items);
            self.name = name;
        }

        $scope: any;
        $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance;
        items: any[];
        name: string;

        select() {
            var self = this;
            self.$uibModalInstance.close(self.items);
        }

        cancel() {
            var self = this;
            self.$uibModalInstance.dismiss('cancel');
        }

        $onInit = () => { };
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('reorderModalController', ReorderModalController);
}

 
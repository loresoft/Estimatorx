/// <reference path="../_ref.ts" />
module Estimatorx {
    "use strict";

    export class TemplateModalController implements angular.IController {

        // protect for minification, must match constructor signiture.
        static $inject = [
            '$scope',
            '$uibModalInstance',
            'items'
        ];

        constructor($scope, $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance, items : ITemplate[]) {
            // assign vm to controller
            $scope.viewModel = this;
            var self = this;

            self.$uibModalInstance = $uibModalInstance;
            self.items = items;
        }

        $uibModalInstance: angular.ui.bootstrap.IModalServiceInstance;
        items: ITemplate[];
        selected: ITemplate;

        select(){
            var self = this;
            self.$uibModalInstance.close(self.selected);

        }

        cancel() {
            var self = this;
            self.$uibModalInstance.dismiss('cancel');
        }

        $onInit = () => { };
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('templateModalController', TemplateModalController);
}
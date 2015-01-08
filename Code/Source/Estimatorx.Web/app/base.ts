/// <reference path="_ref.ts" />

module Estimatorx {
    "use strict";

    export class ControllerBase {

        constructor(
            $scope
        ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
        }

        $scope: any;

        handelError(data, status, headers, config) {
            console.log('error: ' + data);
        }
    }
} 
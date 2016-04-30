/// <reference path="_ref.ts" />

module Estimatorx {
    "use strict";

    export var applicationName: string = 'app';

    export var application: angular.IModule = angular
        .module(Estimatorx.applicationName, [
            'ngAnimate',
            'ngSanitize',
            'ngMessages',
            'ngStorage',
            'angularMoment',
            'ui.bootstrap',
            'ui.select',
            'ui.gravatar',
            'ui.sortable',
            'blockUI',
            'ngClipboard',
            'sticky',
            'hc.marked',
            'toastr'
        ])
        .run(['$localStorage', '$rootScope', ($localStorage, $rootScope) => {
            $rootScope.$localStorage = $localStorage;
        }]);
}

/// <reference path="_ref.ts" />

module Estimatorx {
    "use strict";

    export var applicationName: string = 'app';

    export var application: ng.IModule = angular.module(
        Estimatorx.applicationName,
        [
            'ngAnimate',
            'ngResource',
            'ngSanitize',
            'ngMessages',
            'angularMoment',
            'ui.bootstrap',
            'ui.select',
            'ui.gravatar',
            'ui.sortable',
            'blockUI',
            'ngClipboard',
            'sticky',
            'hc.marked',
            'toaster'
        ]
    );
}

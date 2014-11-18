/// <reference path="_ref.ts" />

module Estimator {
    "use strict";

    export var applicationName: string = 'app';

    export var application: ng.IModule = angular
        .module(Estimator.applicationName, [
            'ngAnimate',
            'ngResource',
            'ngSanitize',
            'angularMoment',
            'ui.bootstrap',
            'ui.router',
            'ui.select'
        ]);
}

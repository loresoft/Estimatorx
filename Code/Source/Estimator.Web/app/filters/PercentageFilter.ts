module Estimator {
    "use strict";

    // register service
    angular.module(Estimator.applicationName)
        .filter('percentage', ['$filter', $filter =>
            (input, decimals) => $filter('number')(input * 100, decimals) + '%']);
}
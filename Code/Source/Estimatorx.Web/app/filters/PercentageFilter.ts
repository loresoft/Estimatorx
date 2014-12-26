/// <reference path="../_ref.ts" />
module Estimatorx {
    "use strict";

    // register service
    angular.module(Estimatorx.applicationName)
        .filter('percentage', ['$filter', $filter =>
            (input, decimals) => $filter('number')(input * 100, decimals) + '%']);
}
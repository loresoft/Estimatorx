/// <reference path="../_ref.ts" />
module Estimator {
    "use strict";

    // register directive
    angular.module(Estimator.applicationName)
        .directive('sortBy', () => {
        return {
            restrict: 'E',
            transclude: true,
            replace: true,
            scope: {
                sort: '=',
                descending: '=',
                sortValue: '@',
                onSort: '='
            },
            link: (scope: any, element, attrs) => {
                scope.sortClick = () => {
                    if (scope.sort == scope.sortValue)
                        scope.descending = !scope.descending;
                    else {
                        scope.sort = scope.sortValue;
                        scope.descending = false;
                    }

                    if (scope.onSort) {
                        scope.onSort(scope.sort, scope.descending);
                    }
                }
            },
            template: '<a ng-click="sortClick(sortValue)" style="display: block;">' +
                      '   <ng-transclude></ng-transclude>' +
                      '   <span class="ng-hide dropdown" ng-show="sort == sortValue" ng-class="{true:\'dropup\', false:\'dropdown\'}[descending]">' +
                      '       <span class="caret" style="margin: 5px 5px;"></span> ' +
                      '   </span>' +
                      '</a>'
        };
    });
}
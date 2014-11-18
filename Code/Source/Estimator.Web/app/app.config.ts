/// <reference path="_ref.ts" />

module Estimator {
    "use strict";

    export class Routing {
        static $inject = ['$stateProvider', '$urlRouterProvider'];
        constructor($stateProvider: ng.ui.IStateProvider, $urlRouterProvider: ng.ui.IUrlRouterProvider) {

            $urlRouterProvider.otherwise('/home');


            $stateProvider
                .state('home', {
                    url: '/home',
                    templateUrl: 'app/home/home.html',
                    controller: 'homeController'
                })
                .state('estimate', {
                    url: '/estimate',
                    templateUrl: 'app/estimate/list.html',
                    controller: 'estimateListController'
                })

                .state('estimateCreate', {
                    url: '/estimate/create',
                    templateUrl: 'app/estimate/edit.html',
                    controller: 'estimateEditController'
                })

                .state('estimateEdit', {
                    url: '/estimate/edit/:id',
                    templateUrl: 'app/estimate/edit.html',
                    controller: 'estimateEditController'
                })
                .state('estimateEdit.project', {
                    url: '/estimate/edit/:id/project',
                    templateUrl: 'app/estimate/edit-project.html'
                })
                .state('estimateEdit.assumption', {
                    url: '/estimate/edit/:id/assumption',
                    templateUrl: 'app/estimate/edit-assumption.html'
                })
                .state('estimateEdit.estimate', {
                    url: '/estimate/edit/:id/estimate',
                    templateUrl: 'app/estimate/edit-estimate.html'
                })
                .state('estimateEdit.factors', {
                    url: '/estimate/edit/:id/factors',
                    templateUrl: 'app/estimate/edit-factors.html'
                })
                .state('estimateEdit.summary', {
                    url: '/estimate/edit/:id/summary',
                    templateUrl: 'app/estimate/edit-summary.html'
                })
            ;

        }
    }

    // configure state
    angular.module(Estimator.applicationName)
        .config(["$locationProvider", $locationProvider => { $locationProvider.html5Mode(true); }]);

    angular.module(Estimator.applicationName)
        .config(Routing);

} 
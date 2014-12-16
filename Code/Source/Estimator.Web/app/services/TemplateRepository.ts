/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";


    export class TemplateRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/Template';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        all(): ng.IHttpPromise<ITemplate[]> {
            return this.$http.get<ITemplate[]>(this.urlBase);
        }

        find(id: string): ng.IHttpPromise<ITemplate> {
            return this.$http.get<ITemplate>(this.urlBase + '/' + id);
        }

        save(project: ITemplate): ng.IHttpPromise<ITemplate> {
            return this.$http.post<ITemplate>(this.urlBase, project);
        }

        delete(id: string): ng.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }

    }

    // register service
    angular.module(Estimator.applicationName)
        .service('templateRepository', ['$http', TemplateRepository]);

} 
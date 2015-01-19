/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class TemplateRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/Template';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        query(request?: IQuerySearch): ng.IHttpPromise<IQueryResult<ITemplate>> {
            var config = {
                params: request
            };

            return this.$http.get<IQueryResult<ITemplate>>(this.urlBase + '/Query', config);
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
    angular.module(Estimatorx.applicationName)
        .service('templateRepository', ['$http', TemplateRepository]);

} 
/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class TemplateRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/Template';
        $http: angular.IHttpService;

        constructor($http: angular.IHttpService) {
            this.$http = $http;
        }

        query(request?: IQuerySearch): angular.IHttpPromise<IQueryResult<ITemplate>> {
            var config = {
                params: request
            };

            return this.$http.get<IQueryResult<ITemplate>>(this.urlBase + '/Query', config);
        }

        all(): angular.IHttpPromise<ITemplate[]> {
            return this.$http.get<ITemplate[]>(this.urlBase);
        }

        find(id: string): angular.IHttpPromise<ITemplate> {
            return this.$http.get<ITemplate>(this.urlBase + '/' + id);
        }

        save(project: ITemplate): angular.IHttpPromise<ITemplate> {
            return this.$http.post<ITemplate>(this.urlBase, project);
        }

        delete(id: string): angular.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }

    }

    // register service
    angular.module(Estimatorx.applicationName)
        .service('templateRepository', ['$http', TemplateRepository]);

} 
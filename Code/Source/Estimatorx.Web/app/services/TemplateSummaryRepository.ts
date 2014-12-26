/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class TemplateSummaryRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/TemplateSummary';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        query(request?: IQueryRequest): ng.IHttpPromise<IQueryResult<ITemplate>> {
            var config = {
                params: request
            };

            return this.$http.get<IQueryResult<ITemplate>>(this.urlBase, config);
        }
    }

    // register service
    angular.module(Estimatorx.applicationName)
        .service('templateSummaryRepository', ['$http', TemplateSummaryRepository]);

}  
/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class OrganizationRepository {
        static $inject = [
            '$http'
        ];

        urlBase: string = 'api/Organization';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {

            this.$http = $http;
        }

        query(request?: IQueryRequest): ng.IHttpPromise<IQueryResult<IOrganization>> {
            var config = {
                params: request
            };

            return this.$http.get<IQueryResult<IOrganization>>(this.urlBase + '/Query', config);
        }

        all(): ng.IHttpPromise<IOrganization[]> {
            return this.$http.get<IOrganization[]>(this.urlBase);
        }

        find(id: string): ng.IHttpPromise<IOrganization> {
            return this.$http.get<IOrganization>(this.urlBase + '/' + id);
        }

        save(organization: IOrganization): ng.IHttpPromise<IOrganization> {
            return this.$http.post<IOrganization>(this.urlBase, organization);
        }

        delete(id: string): ng.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }
    }

    // register service
    angular.module(Estimatorx.applicationName)
        .service('organizationRepository', [
            '$http',
            OrganizationRepository
        ]);
}  
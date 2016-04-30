/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class OrganizationRepository {
        static $inject = [
            '$http'
        ];

        urlBase: string = 'api/Organization';
        $http: angular.IHttpService;

        constructor($http: angular.IHttpService) {

            this.$http = $http;
        }

        query(request?: IQueryRequest): angular.IHttpPromise<IQueryResult<IOrganization>> {
            var config = {
                params: request
            };

            return this.$http.get<IQueryResult<IOrganization>>(this.urlBase + '/Query', config);
        }

        all(): angular.IHttpPromise<IOrganization[]> {
            return this.$http.get<IOrganization[]>(this.urlBase);
        }

        find(id: string): angular.IHttpPromise<IOrganization> {
            return this.$http.get<IOrganization>(this.urlBase + '/' + id);
        }

        save(organization: IOrganization): angular.IHttpPromise<IOrganization> {
            return this.$http.post<IOrganization>(this.urlBase, organization);
        }

        delete(id: string): angular.IHttpPromise<void> {
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
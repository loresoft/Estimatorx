/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class InviteRepository {
        static $inject = [
            '$http'
        ];

        urlBase: string = 'api/Invite';
        $http: angular.IHttpService;

        constructor($http: angular.IHttpService) {
            this.$http = $http;
        }

        organization(organizationId: string) {
            return this.$http.get<IInvite[]>(this.urlBase + '/Organization/' + organizationId);
        }

        send(id: string): angular.IHttpPromise<IInvite> {
            return this.$http.get<IInvite>(this.urlBase + '/' + id + '/Send');
        }
        
        find(id: string): angular.IHttpPromise<IInvite> {
            return this.$http.get<IInvite>(this.urlBase + '/' + id);
        }

        save(invite: IInvite): angular.IHttpPromise<IInvite> {
            return this.$http.post<IInvite>(this.urlBase, invite);
        }

        delete(id: string): angular.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }
    }

    // register service
    angular.module(Estimatorx.applicationName).service('inviteRepository', [
        '$http',
        InviteRepository
    ]);
}   
/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class InviteRepository {
        static $inject = [
            '$http'
        ];

        urlBase: string = 'api/Invite';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        organization(organizationId: string) {
            return this.$http.get<IInvite[]>(this.urlBase + '/Organization/' + organizationId);
        }

        send(id: string): ng.IHttpPromise<IInvite> {
            return this.$http.get<IInvite>(this.urlBase + '/' + id + '/Send');
        }
        
        find(id: string): ng.IHttpPromise<IInvite> {
            return this.$http.get<IInvite>(this.urlBase + '/' + id);
        }

        save(invite: IInvite): ng.IHttpPromise<IInvite> {
            return this.$http.post<IInvite>(this.urlBase, invite);
        }

        delete(id: string): ng.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }
    }

    // register service
    angular.module(Estimatorx.applicationName).service('inviteRepository', [
        '$http',
        InviteRepository
    ]);
}   
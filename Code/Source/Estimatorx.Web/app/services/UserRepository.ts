/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class UserRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/User';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }


        profile(): ng.IHttpPromise<IUser> {
            return this.$http.get<IUser>(this.urlBase + '/Profile');
        }

        save(user: IUser): ng.IHttpPromise<IUser> {
            return this.$http.post<IUser>(this.urlBase, user);
        }

        search(text: string): ng.IHttpPromise<IUser[]> {
            var config = {
                params: {
                    q: text
                }
            };
        
            return this.$http.get<IUser[]>(this.urlBase + '/Search', config);
        }


        setPassword(password: IPassword): ng.IHttpPromise<void> {
            return this.$http.post<void>(this.urlBase + '/SetPassword', password);
        }

        changePassword(password: IPassword): ng.IHttpPromise<void> {
            return this.$http.post<void>(this.urlBase + '/ChangePassword', password);
        }


        removeLogin(provider: string, key: string): ng.IHttpPromise<void> {
            var model = {
                LoginProvider: provider,
                ProviderKey: key
            }
            return this.$http.post<void>(this.urlBase + '/RemoveLogin', model);
        }


        organizationMembers(organizationId: string) {
            return this.$http.get<IUser[]>(this.urlBase + '/OrganizationMembers/' + organizationId);
        }

        organizationOwners(organizationId: string) {
            return this.$http.get<IUser[]>(this.urlBase + '/OrganizationOwners/' + organizationId);
        }

        addOrganization(organizationId: string, userId: string): ng.IHttpPromise<void> {
            var model = {
                OrganizationId: organizationId,
                UserId: userId
            };

            return this.$http.post<void>(this.urlBase + '/AddOrganization', model);
        }

        removeOrganization(organizationId: string, userId: string): ng.IHttpPromise<void> {
            var model = {
                OrganizationId: organizationId,
                UserId: userId
            };

            return this.$http.post<void>(this.urlBase + '/RemoveOrganization', model);
        }
    }

    // register service
    angular.module(Estimatorx.applicationName)
        .service('userRepository', ['$http', UserRepository]);

}  
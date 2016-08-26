/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class UserRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/User';
        $http: angular.IHttpService;

        constructor($http: angular.IHttpService) {
            this.$http = $http;
        }

        find(id: string): angular.IHttpPromise<IUser> {
          return this.$http.get<IUser>(this.urlBase + '/' + id);
        }

        profile(): angular.IHttpPromise<IUser> {
            return this.$http.get<IUser>(this.urlBase + '/Profile');
        }

        save(user: IUser): angular.IHttpPromise<IUser> {
            return this.$http.post<IUser>(this.urlBase, user);
        }

        delete(id: string): angular.IHttpPromise<void> {
          return this.$http.delete<void>(this.urlBase + '/' + id);
        }

        search(text: string): angular.IHttpPromise<IUser[]> {
            var config = {
                params: {
                    q: text
                }
            };
        
            return this.$http.get<IUser[]>(this.urlBase + '/Search', config);
        }

        query(request?: IQuerySearch): angular.IHttpPromise<IQueryResult<IUser>> {
          var config = {
            params: request
          };

          return this.$http.get<IQueryResult<IUser>>(this.urlBase + '/Query', config);
        }


        setPassword(password: IPassword): angular.IHttpPromise<void> {
            return this.$http.post<void>(this.urlBase + '/SetPassword', password);
        }

        changePassword(password: IPassword): angular.IHttpPromise<void> {
            return this.$http.post<void>(this.urlBase + '/ChangePassword', password);
        }


        removeLogin(provider: string, key: string): angular.IHttpPromise<void> {
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

        addOrganization(organizationId: string, userId: string): angular.IHttpPromise<void> {
            var model = {
                OrganizationId: organizationId,
                UserId: userId
            };

            return this.$http.post<void>(this.urlBase + '/AddOrganization', model);
        }

        removeOrganization(organizationId: string, userId: string): angular.IHttpPromise<void> {
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
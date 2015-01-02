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

        all(): ng.IHttpPromise<IUser[]> {
            return this.$http.get<IUser[]>(this.urlBase);
        }

        find(id: string): ng.IHttpPromise<IUser> {
            return this.$http.get<IUser>(this.urlBase + '/' + id);
        }

        save(user: IUser): ng.IHttpPromise<IUser> {
            return this.$http.post<IUser>(this.urlBase, user);
        }

        delete(id: string): ng.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
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

    }

    // register service
    angular.module(Estimatorx.applicationName)
        .service('userRepository', ['$http', UserRepository]);

}  
/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class ProjectRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/Project';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }

        query(request?: IQuerySearch): ng.IHttpPromise<IQueryResult<IProject>> {
            var config = {
                params: request
            };

            return this.$http.get<IQueryResult<IProject>>(this.urlBase + '/Query', config);
        }

        all(): ng.IHttpPromise<IProject[]> {            
            return this.$http.get<IProject[]>(this.urlBase);
        }

        find(id: string): ng.IHttpPromise<IProject> {
            return this.$http.get<IProject>(this.urlBase + '/' + id);
        }

        shared(id: string, key: string): ng.IHttpPromise<IProject> {
            return this.$http.get<IProject>(this.urlBase + '/Share/' + id + '/' + key);
        }


        save(project: IProject): ng.IHttpPromise<IProject> {
            return this.$http.post<IProject>(this.urlBase, project);
        }

        delete(id: string): ng.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }

    }

    // register service
    angular.module(Estimatorx.applicationName)
        .service('projectRepository', ['$http', ProjectRepository]);

} 
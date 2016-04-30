/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";


    export class ProjectRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/Project';
        $http: angular.IHttpService;

        constructor($http: angular.IHttpService) {
            this.$http = $http;
        }

        query(request?: IQuerySearch): angular.IHttpPromise<IQueryResult<IProject>> {
            var config = {
                params: request
            };

            return this.$http.get<IQueryResult<IProject>>(this.urlBase + '/Query', config);
        }

        all(): angular.IHttpPromise<IProject[]> {            
            return this.$http.get<IProject[]>(this.urlBase);
        }

        find(id: string): angular.IHttpPromise<IProject> {
            return this.$http.get<IProject>(this.urlBase + '/' + id);
        }

        shared(id: string, key: string): angular.IHttpPromise<IProject> {
            return this.$http.get<IProject>(this.urlBase + '/Share/' + id + '/' + key);
        }


        save(project: IProject): angular.IHttpPromise<IProject> {
            return this.$http.post<IProject>(this.urlBase, project);
        }

        delete(id: string): angular.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }

    }

    // register service
    angular.module(Estimatorx.applicationName)
        .service('projectRepository', ['$http', ProjectRepository]);

} 
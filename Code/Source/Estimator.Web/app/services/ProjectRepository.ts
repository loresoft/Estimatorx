/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";


    export class ProjectRepository {
        static $inject = ['$http'];

        urlBase: string = 'api/project';
        $http: ng.IHttpService;

        constructor($http: ng.IHttpService) {
            this.$http = $http;
        }
        
        all(): ng.IHttpPromise<IProject[]> {            
            return this.$http.get<IProject[]>(this.urlBase);
        }

        find(id: string): ng.IHttpPromise<IProject> {
            return this.$http.get<IProject>(this.urlBase + '/' + id);
        }

        save(project: IProject): ng.IHttpPromise<IProject> {
            return this.$http.post<IProject>(this.urlBase, project);
        }

        delete(id: string): ng.IHttpPromise<void> {
            return this.$http.delete<void>(this.urlBase + '/' + id);
        }

    }

    // register service
    angular.module(Estimator.applicationName)
        .service('projectRepository', ['$http', ProjectRepository]);

} 
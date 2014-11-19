module Estimator {
    "use strict";

    export class ProjectRepository {
        static $inject = ['$resource'];

        constructor($resource: ng.resource.IResourceService) {
            
        }
        
    }


    // register service
    angular.module(Estimator.applicationName)
        .service('projectRepository', ['$resource', ProjectRepository]);

} 
/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProjectReportController implements angular.IController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            'logger',
            'projectRepository'
        ];
        constructor(
            $scope,
            logger: Logger,
            projectRepository: ProjectRepository
        ) {

            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;

            self.logger = logger;
            self.projectRepository = projectRepository;

            self.project = <IProject>{};
        }

        $scope: angular.IScope;
        logger: Logger;
        projectRepository: ProjectRepository;

        project: IProject;
        projectId: string;
        securityKey: string;

        load(id?: string, key?: string) {
            var self = this;

            self.projectId = id;
            self.securityKey = key;

            if (self.securityKey) {
                self.projectRepository.shared(self.projectId, self.securityKey)
                    .then((response) => {
                        self.project = response.data;
                    })
                    .catch(self.logger.handelErrorProxy);

            } else {
                self.projectRepository.find(self.projectId)
                    .then((response) => {
                        self.project = response.data;
                    })
                    .catch(self.logger.handelErrorProxy);                
            }
        }

        lookupFactor(factorId: string): string {
            var self = this;

            return Enumerable.From(self.project.Factors)
                .Where(p => p.Id === factorId)
                .Select(p => p.Name)
                .FirstOrDefault("");
        }

        $onInit = () => { };
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('projectReportController', ProjectReportController);
}


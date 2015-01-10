/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ModelFactory {

        static $inject = ['identityService'];
        constructor(identityService: IdentityService) {
            this.identityService = identityService;
        }

        factorCounter: number = 0
        taskCounter: number = 0
        sectionCounter: number = 0

        identityService: IdentityService;

        createTemplate(id?: string): ITemplate {
            var template = <ITemplate>{};
            template.Id = id ? id : this.identityService.newObjectId();
            template.Name = 'New Template';
            template.Created = new Date();
            template.Updated = new Date();

            return template;
        }


        createProject(id?: string): IProject {
            var project = <IProject>{};
            project.Id = id ? id : this.identityService.newObjectId();
            project.Name = 'New Project';
            project.HoursPerWeek = 30;
            project.ContingencyRate = 10;
            project.TotalTasks = 0;
            project.TotalHours = 0;
            project.TotalWeeks = 0;
            project.ContingencyHours = 0;
            project.ContingencyWeeks = 0;
            project.Created = new Date();
            project.Updated = new Date();

            return project;
        }

        createSection(): ISection {
            var section = <ISection>{};
            section.Id = this.identityService.newObjectId();
            section.Name = 'Section ' + (this.sectionCounter++);
            section.TotalTasks = 0;
            section.TotalHours = 0;
            section.TotalWeeks = 0;

            return section;
        }

        createFactor(): IFactor {
            var factor = <IFactor>{};
            factor.Id = this.identityService.newObjectId();
            factor.Name = 'Factor ' + (this.factorCounter++);
            factor.VerySimple = 2;
            factor.Simple = 4;
            factor.Medium = 8;
            factor.Complex = 16;
            factor.VeryComplex = 32;

            return factor;
        }

        createTask(): ITask {
            var estimate = <ITask>{};
            estimate.Id = this.identityService.newObjectId();
            estimate.Name = 'Task ' + (this.taskCounter++);
            estimate.TotalTasks = 0;
            estimate.TotalHours = 0;

            return estimate;
        }

        createOrganization(id?: string, ownerId?: string): IOrganization {
            var organization = <IOrganization>{};
            organization.Id = id ? id : this.identityService.newObjectId();

            organization.Name = '';
            organization.Created = new Date();
            organization.Updated = new Date();

            if (ownerId)
                organization.Owners = [ownerId];

            return organization;
        }

    }


    // register service
    angular.module(Estimatorx.applicationName)
        .service('modelFactory', ['identityService', ModelFactory]);

}





/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ModelFactory {

        static $inject = ['identityService'];
        constructor(identityService: IdentityService) {
            this.identityService = identityService;
        }

        factorCounter: number = 0
        estimateCounter: number = 0
        sectionCounter: number = 0

        identityService: IdentityService;

        createTemplate(id?: string): ITemplate {
            var template = this.createModel<ITemplate>(id);
            template.Name = 'New Template';

            return template;
        }


        createProject(id?: string): IProject {
            var project = this.createModel<IProject>(id);
            project.Name = 'New Project';
            project.HoursPerWeek = 30;
            project.ContingencyRate = 10;
            project.TotalTasks = 0;
            project.TotalHours = 0;
            project.TotalWeeks = 0;
            project.ContingencyHours = 0;
            project.ContingencyWeeks = 0;

            return project;
        }

        createSection(): ISection {
            var section = this.createModel<ISection>();
            section.Name = 'Section ' + (this.sectionCounter++);
            section.TotalTasks = 0;
            section.TotalHours = 0;
            section.TotalWeeks = 0;

            return section;
        }

        createFactor(): IFactor {
            var factor = this.createModel<IFactor>();
            factor.Name = 'Factor ' + (this.factorCounter++);
            factor.VerySimple = 2;
            factor.Simple = 4;
            factor.Medium = 8;
            factor.Complex = 16;
            factor.VeryComplex = 32;

            return factor;
        }

        createEstimate(): IEstimate {
            var estimate = this.createModel<IEstimate>();
            estimate.Name = 'Estimate ' + (this.estimateCounter++);
            estimate.TotalTasks = 0;
            estimate.TotalHours = 0;

            return estimate;
        }

        createAssumption(): IAssumption {
            var assumption = this.createModel<IAssumption>();
            return assumption;
        }

        createModel<T extends IModelBase>(id?: string): T {
            var model = <T>{
                Id: id ? id : this.identityService.newUUID(),
                IsActive: true,
                SysCreateDate: new Date,
                SysUpdateDate: new Date,
            };

            return model;
        }
    }


    // register service
    angular.module(Estimatorx.applicationName)
        .service('modelFactory', ['identityService', ModelFactory]);

}





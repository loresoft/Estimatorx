module Estimator {
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

        createProject(): IProject {
            var project = this.createModel<Project>();
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
            var section = this.createModel<Section>();
            section.Name = 'Section' + (this.sectionCounter++);
            section.TotalTasks = 0;
            section.TotalHours = 0;
            section.TotalWeeks = 0;

            return section;
        }

        createFactor(): IFactor {
            var factor = this.createModel<Factor>();
            factor.Name = 'Factor ' + (this.factorCounter++);
            factor.VerySimple = 2;
            factor.Simple = 4;
            factor.Medium = 8;
            factor.Complex = 16;
            factor.VeryComplex = 32;

            return factor;
        }

        createEstimate(): IEstimate {
            var estimate = this.createModel<Estimate>();
            estimate.Name = 'Estimate ' + (this.estimateCounter++);
            estimate.TotalTasks = 0;
            estimate.TotalHours = 0;

            return estimate;
        }

        createAssumption(): IAssumption {
            var assumption = this.createModel<IAssumption>();
            return assumption;
        }

        createModel<T extends IModelBase>(): T {
            var model = <T>{
                Id: this.identityService.newUUID(),
                IsActive: true,
                SysCreateDate: new Date,
                SysUpdateDate: new Date,
            };

            return model;
        }
    }


    // register service
    angular.module(Estimator.applicationName)
        .service('modelFactory', ['identityService', ModelFactory]);

}





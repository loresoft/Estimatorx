/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class ProjectCalculator {
        static $inject = [];

        constructor() {

        }

        updateTotals(project: IProject) {
            var hoursPerWeek = project.HoursPerWeek || 30;

            // reset
            project.TotalTasks = 0;
            project.TotalHours = 0;
            project.TotalWeeks = 0;
            project.ContingencyHours = 0;
            project.ContingencyWeeks = 0;

            angular.forEach(project.Sections, (section) => {
                this.updateSection(project, section);

                // update parent totals
                project.TotalTasks += section.TotalTasks;
                project.TotalHours += section.TotalHours;

            });

            // calculate weeks after children total
            project.TotalWeeks = this.round(project.TotalHours / hoursPerWeek, 2);

            var contingencyPercent = project.ContingencyRate / 100;
            var contingencyHours = project.TotalHours * (1 + contingencyPercent);
            project.ContingencyHours = Number(this.round(contingencyHours, 0));
            project.ContingencyWeeks = this.round(project.ContingencyHours / hoursPerWeek, 2);

            return project;
        }

        updateSection(project: IProject, section: ISection) {
            // reset
            section.TotalTasks = 0;
            section.TotalHours = 0;
            section.TotalWeeks = 0;

            angular.forEach(section.Tasks, (estimate) => {
                this.updateEstimate(project, estimate);

                // update parent totals
                section.TotalTasks += estimate.TotalTasks;
                section.TotalHours += estimate.TotalHours;
            });

            // calculate weeks after children total
            var hoursPerWeek = project.HoursPerWeek;
            section.TotalWeeks = this.round(section.TotalHours / hoursPerWeek, 2);
        }

        updateEstimate(project: IProject, task: ITask) {
            var verySimple = task.VerySimple || 0,
                simple = task.Simple || 0,
                medium = task.Medium || 0,
                complex = task.Complex || 0,
                veryComplex = task.VeryComplex || 0;


            task.TotalTasks = verySimple + simple + medium + complex + veryComplex;

            var factor = <IFactor>Enumerable.From(project.Factors)
                .Where(f => f.Id == task.FactorId)
                .FirstOrDefault(<IFactor>{});

            var factorVerySimple = factor.VerySimple || 0,
                factorSimple = factor.Simple || 0,
                factorMedium = factor.Medium || 0,
                factorComplex = factor.Complex || 0,
                factorVeryComplex = factor.VeryComplex || 0;

            task.TotalHours = (verySimple * factorVerySimple)
                + (simple * factorSimple)
                + (medium * factorMedium)
                + (complex * factorComplex) 
                + (veryComplex * factorVeryComplex);

        }

        round(value: number, decimals: number = 2): number {
            if (decimals <= 0)
                return Math.round(value);

            var m: number = Math.pow(10, decimals);
            return Math.round(value * m) / m;
        }
    }


    // register service
    angular.module(Estimatorx.applicationName)
        .service('projectCalculator', ProjectCalculator);

}
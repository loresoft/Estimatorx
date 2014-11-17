/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export class EstimateEditController {

        // project for minification, must match contructor signiture.
        static $inject = ['$scope', '$location', 'identityService', 'projectCalculator'];
        constructor($scope, $location: ng.ILocationService, identityService: IdentityService, projectCalculator: ProjectCalculator) {
            // assign viewModel to controller
            $scope.viewModel = this;
            this.$scope = $scope;
            this.$location = $location;

            this.identityService = identityService;
            this.projectCalculator = projectCalculator;

            this.project = <IProject>{};
            var self = this;

            $scope.$watch(
                s => angular.toJson(s.viewModel.project),
                _.debounce($.proxy(self.calculate, self), 500));
        }

        $scope: ng.IScope;
        $location: ng.ILocationService;
        identityService: IdentityService;
        projectCalculator: ProjectCalculator;
        project: IProject;

        loadProject() {
            // get project id

            this.project = <IProject>{
                Id: this.identityService.newUUID(),
                Name: 'New Project',
                HoursPerWeek: 30,
                ContingencyRate: .10,
                SysCreateDate: new Date,
                SysCreateUser: 'paul.welter',
                SysUpdateDate: new Date,
                SysUpdateUser: 'paul.welter',
            };
        }

        saveProject() {
            
        }

        calculate() {
            console.log("project calculate");

            this.projectCalculator.updateTotals(this.project);
            this.$scope.$apply();
        }


        addAssumption() {
            if (!this.project.Assumptions)
                this.project.Assumptions = [];

            var assumption = <IAssumption>{
                Id: this.identityService.newUUID(),
                Description: 'Assumption ' + this.project.Assumptions.length,
                IsActive: true
            };

            this.project.Assumptions.push(assumption);
        }

        removeAssumption(assumption: IAssumption) {
            if (!assumption)
                return;

            for (var i = 0; i < this.project.Assumptions.length; i++) {
                if (this.project.Assumptions[i].Id == assumption.Id) {
                    this.project.Assumptions.splice(i, 1);
                    break;
                }
            }
        }


        addFactor() {
            if (!this.project.Factors)
                this.project.Factors = [];

            var factor = <IFactor>{
                Id: this.identityService.newUUID(),
                Name: 'Factor ' + this.project.Factors.length,
                VerySimple: 2,
                Simple: 4,
                Medium: 8,
                Complex: 16,
                VeryComplex: 32,
                IsActive: true
            };

            this.project.Factors.push(factor);
        }

        removeFactor(factor: IFactor) {
            if (!factor)
                return;

            for (var i = 0; i < this.project.Factors.length; i++) {
                if (this.project.Factors[i].Id == factor.Id) {
                    this.project.Factors.splice(i, 1);
                    break;
                }
            }
        }


        addSection() {
            if (!this.project.Sections)
                this.project.Sections = [];

            var section = <ISection>{
                Id: this.identityService.newUUID(),
                Name: 'Section ' + this.project.Sections.length,
                IsActive: true
            };

            this.project.Sections.push(section);
        }

        removeSection(section: ISection) {
            if (!section)
                return;

            if (section.Estimates && section.Estimates.length)
                return; 

            if (!this.project.Sections)
                return;

            for (var i = 0; i < this.project.Sections.length; i++) {
                if (this.project.Sections[i].Id == section.Id) {
                    this.project.Sections.splice(i, 1);
                    break;
                }
            }
        }


        addEstimate(section: ISection) {
            if (!section)
                return;

            if (!section.Estimates)
                section.Estimates = [];

            var estimate = <IEstimate>{
                Id: this.identityService.newUUID(),
                Name: 'Task ' + section.Estimates.length,
                IsActive: true
            };

            section.Estimates.push(estimate);
        }

        removeEstimate(section: ISection, estimate: IEstimate) {
            if (!section || !estimate)
                return;

            if (!section.Estimates)
                return;

            for (var i = 0; i < section.Estimates.length; i++) {
                if (section.Estimates[i].Id == estimate.Id) {
                    section.Estimates.splice(i, 1);
                    break;
                }
            }
        }
    }

    // register controller
    angular.module(Estimator.applicationName)
        .controller('estimateEditController', [
            '$scope',
            '$location',
            'identityService',
            'projectCalculator',

            EstimateEditController // controller must be last
        ]);
}


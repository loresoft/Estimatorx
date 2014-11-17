/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IProject extends IModelBase {
        Name: string;
        Description?: string;

        HoursPerWeek: number;
        ContingencyRate: number;

        TotalTasks: number;
        TotalHours: number;
        TotalWeeks: number;

        ContingencyHours: number;
        ContingencyWeeks: number;

        Assumptions: IAssumption[];
        Factors: IFactor[];
        Sections: ISection[];
    }

    export class Project extends ModelBase implements IProject {
        constructor() {
            super();
        }

        Name: string = '';
        Description: string;

        HoursPerWeek: number = 30;
        ContingencyRate: number = .10;

        TotalTasks: number = 0;
        TotalHours: number = 0;
        TotalWeeks: number = 0;

        ContingencyHours: number = 0;
        ContingencyWeeks: number = 0;

        Assumptions: IAssumption[] = [];
        Factors: IFactor[] = [];
        Sections: ISection[] = [];
    }
}

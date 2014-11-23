/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IProject {
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
}

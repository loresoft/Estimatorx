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

        Factors: IFactor[];
        Sections: ISection[];
    }
}

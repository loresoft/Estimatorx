/// <reference path="../_ref.ts" />

module Estimatorx {
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
}

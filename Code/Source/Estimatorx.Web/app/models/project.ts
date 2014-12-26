/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface IProject {
        Id: string;

        Name: string;
        Description: string;

        HoursPerWeek: number;
        ContingencyRate: number;

        TotalTasks: number;
        TotalHours: number;
        TotalWeeks: number;

        ContingencyHours: number;
        ContingencyWeeks: number;

        Created: Date;
        Creator: string;

        Updated: Date;
        Updater: string;

        Assumptions: string[];
        Factors: IFactor[];
        Sections: ISection[];
    }
}

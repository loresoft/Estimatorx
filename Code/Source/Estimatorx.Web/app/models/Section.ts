/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface ISection extends IModelBase {
        Name: string;
        Description?: string;

        TotalTasks: number;
        TotalHours: number;
        TotalWeeks: number;

        Estimates: IEstimate[];
    }
}

/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface ISection extends IModelBase {
        Name: string;
        Description?: string;

        TotalTasks: number;
        TotalHours: number;
        TotalWeeks: number;

        Estimates: IEstimate[];
    }

    export class Section extends ModelBase implements ISection {
        constructor() {
            super();
        }

        Name: string = '';
        Description: string;

        TotalTasks: number = 0;
        TotalHours: number = 0;
        TotalWeeks: number = 0;

        Estimates: IEstimate[] = [];
    }

}

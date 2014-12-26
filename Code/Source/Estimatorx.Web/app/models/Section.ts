/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface ISection {
        Id: string;

        Name: string;
        
        TotalTasks: number;
        TotalHours: number;
        TotalWeeks: number;

        Tasks: ITask[];
    }
}

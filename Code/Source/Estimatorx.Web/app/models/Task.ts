 /// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface ITask {
        Id: string;

        Name: string;

        VerySimple?: number;
        Simple?: number;
        Medium?: number;
        Complex?: number;
        VeryComplex?: number;

        TotalTasks: number;
        TotalHours: number;
        
        FactorId: string;
    }
}
 
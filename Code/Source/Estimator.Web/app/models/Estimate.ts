 /// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IEstimate extends IModelBase {
        Name: string;
        Description?: string;

        VerySimple?: number;
        Simple?: number;
        Medium?: number;
        Complex?: number;
        VeryComplex?: number;

        TotalTasks: number;
        TotalHours: number;
        
        FactorId: string;
    }

    export class Estimate extends ModelBase implements IEstimate {
        constructor() {
            super();
        }

        Name: string = '';
        Description: string = '';

        VerySimple: number;
        Simple: number;
        Medium: number;
        Complex: number;
        VeryComplex: number;

        TotalTasks: number;
        TotalHours: number;

        FactorId: string;
    }
}
 
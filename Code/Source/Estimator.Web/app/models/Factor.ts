 /// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IFactor extends IModelBase {
        Name: string;
        Description?: string;

        VerySimple: number;
        Simple: number;
        Medium: number;
        Complex: number;
        VeryComplex: number;
    }

    export class Factor extends ModelBase implements IFactor {
        constructor() {
            super();
        }

        Name: string = '';
        Description: string;

        VerySimple: number = 2;
        Simple: number = 4;
        Medium: number = 8;
        Complex: number = 16;
        VeryComplex: number = 32;
    }
}
  
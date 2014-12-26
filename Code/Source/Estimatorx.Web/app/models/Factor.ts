 /// <reference path="../_ref.ts" />

module Estimatorx {
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
}
  
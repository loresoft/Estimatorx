/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface ITemplate  {
        Name: string;
        Description?: string;

        Factors: IFactor[];        
    }
}
 
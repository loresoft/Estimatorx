/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface ITemplate extends IModelBase  {
        Name: string;
        Description?: string;

        Factors: IFactor[];        
    }
}
 
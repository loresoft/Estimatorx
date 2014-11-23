/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface ITemplate extends IModelResource<ITemplate> {
        Name: string;
        Description?: string;

        Factors: IFactor[];        
    }
}
 
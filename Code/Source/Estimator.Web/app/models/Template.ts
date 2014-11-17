/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface ITemplate extends IModelBase {
        Name: string;
        Description?: string;

        Factors: IFactor[];        
    }

    export class Template extends ModelBase implements ITemplate {
        constructor() {
            super();
        }

        Name: string = '';
        Description: string;

        Factors: IFactor[] = [];
    }

}
 
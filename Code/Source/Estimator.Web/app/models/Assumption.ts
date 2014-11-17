/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IAssumption extends IModelBase {
        Description: string;
    }

    export class Assumption extends ModelBase implements IAssumption {
        constructor() {
            super();

            this.Description = '';
        }

        Description: string;
    }
}
   
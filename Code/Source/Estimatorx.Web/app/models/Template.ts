/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface ITemplate {
        Id: string;

        Name: string;
        Description: string;

        Created: Date;
        Creator: string;

        Updated: Date;
        Updater: string;

        Factors: IFactor[];
    }
}

/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface IOrganization {
        Id: string;

        Name: string;
        Description: string;

        Created: Date;
        Creator: string;

        Updated: Date;
        Updater: string;

        Owners: string[];
    }
}
   
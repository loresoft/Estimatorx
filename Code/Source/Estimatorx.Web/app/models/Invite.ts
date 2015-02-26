/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface IInvite {
        Id: string;

        Email: string;
        OrganizationId: string;
        SecurityKey: string;

        Created: Date;
        Creator: string;

        Updated: Date;
        Updater: string;
    }
}
    
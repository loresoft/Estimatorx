/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IModelBase {
        Id: string;

        IsActive: boolean;

        SysCreateDate: Date;
        SysCreateUser: string;

        SysUpdateDate: Date;
        SysUpdateUser: string;
    }

    export class ModelBase implements IModelBase {
        constructor() {
            this.SysCreateDate = new Date();
            this.SysUpdateDate = new Date();
        }

        Id: string;

        IsActive: boolean;

        SysCreateDate: Date;
        SysCreateUser: string;

        SysUpdateDate: Date;
        SysUpdateUser: string;
    }
}
 
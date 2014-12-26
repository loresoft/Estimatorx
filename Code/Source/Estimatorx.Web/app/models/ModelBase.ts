/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface IModelBase  {
        Id: string;

        IsActive: boolean;

        SysCreateDate: Date;
        SysCreateUser: string;

        SysUpdateDate: Date;
        SysUpdateUser: string;
    }
}
 
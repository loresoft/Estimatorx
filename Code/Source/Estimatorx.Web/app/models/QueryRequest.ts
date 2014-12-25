/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IQueryRequest {
        Page?: number;
        PageSize?: number;
        Sort?: string;
        Descending?: boolean;        
    }
}

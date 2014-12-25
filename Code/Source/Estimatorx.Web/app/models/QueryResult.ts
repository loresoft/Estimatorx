/// <reference path="../_ref.ts" />

module Estimator {
    "use strict";

    export interface IQueryResult<T> extends IQueryRequest {
        Data?: T[];
        PageCount?: number;
        Total?: number;
    }
}

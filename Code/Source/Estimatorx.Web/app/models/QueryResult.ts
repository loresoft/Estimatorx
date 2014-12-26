/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export interface IQueryResult<T> extends IQueryRequest {
        Data?: T[];
        PageCount?: number;
        Total?: number;
    }
}

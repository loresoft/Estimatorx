/// <reference path="../_ref.ts" />

module Estimatorx {
  "use strict";

  export interface IQueryResult<T> {
    Data?: T[];

    Page?: number;
    PageSize?: number;
    PageCount?: number;

    Sort?: string;
    Descending?: boolean;

    Filter?: any;

    Total?: number;
  }
}

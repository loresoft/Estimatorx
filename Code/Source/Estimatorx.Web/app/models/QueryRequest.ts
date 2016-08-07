/// <reference path="../_ref.ts" />

module Estimatorx {
  "use strict";

  export interface IQueryRequest {
    Page?: number;
    PageSize?: number;

    Sort?: string;
    Descending?: boolean;

    Filter?: any;

  }

  export interface IQuerySearch extends IQueryRequest {
    Search?: string;
    Organization?: string;
  }

}

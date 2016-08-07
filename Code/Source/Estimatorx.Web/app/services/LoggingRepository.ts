/// <reference path="../_ref.ts" />

module Estimatorx {
  "use strict";

  export class LoggingRepository {
    static $inject = [
      '$http'
    ];

    urlBase: string = 'api/Logging';
    $http: angular.IHttpService;

    constructor(
      $http: angular.IHttpService) {
      this.$http = $http;
    }

    query(request?: IQueryRequest): angular.IHttpPromise<IQueryResult<ILogEvent>> {
      var config = {
        params: request
      };

      return this.$http.get<IQueryResult<ILogEvent>>(this.urlBase + '/Query', config);
    }

    all(): angular.IHttpPromise<ILogEvent[]> {
      return this.$http.get<ILogEvent[]>(this.urlBase);
    }

  }

  // register service
  angular.module(Estimatorx.applicationName).service('loggingRepository', [
    '$http',
    LoggingRepository
  ]);
}  
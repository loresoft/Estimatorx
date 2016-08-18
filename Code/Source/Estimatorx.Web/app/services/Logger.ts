/// <reference path="../_ref.ts" />

module Estimatorx {
  "use strict";

  export interface AlertOptions {
    type?: string;
    title?: string;
    message?: string;
    timeOut?: number;
  }

  export class Logger {
    static $inject = [
      'toastr'
    ];


    constructor(toastr: angular.toastr.IToastrService) {
      var self = this;

      self.toastr = toastr;
      self.handelErrorProxy = <angular.IHttpPromiseCallback<any>>angular.bind(self, self.handelError);

    }

    toastr: angular.toastr.IToastrService;

    showAlert(options: AlertOptions) {
      var self = this;
      var d = {
        type: 'warn',
        title: 'Complete',
        message: 'Unrecognized server response, please verify operation completed correctly.',
        timeOut: 6000,
        closeButton: true,
        progressBar: true
      };

      var o = $.extend({}, d, options);
      switch (o.type) {
        case 'error':
          self.toastr.error(o.message, o.title, o);
          break;
        case 'warn':
        case 'warning':
          self.toastr.warning(o.message, o.title, o);
          break;
        case 'success':
          self.toastr.success(o.message, o.title, o);
          break;
        default:
          self.toastr.info(o.message, o.title, o);
          break;
      }
    }

    handelErrorProxy: angular.IHttpPromiseCallback<any>;

    handelError(data: any, status: number, headers: angular.IHttpHeadersGetter, config: angular.IRequestConfig) {
      var self = this;
      var message = '';

      if (status === 400)
        message = self.extractModelState(data);
      else if (status === 500)
        message = self.extractServerError(data);
      else if (status)
        message += 'An error has occurred:\n ' + status;
      else
        message += 'An error has occurred:\n Unrecognized server response';

      // clear others to prevents flood of toast
      var options = {
        closeButton: true,
        progressBar: true,
        timeOut: 6000,
      }

      self.toastr.clear();
      self.toastr.error(message, 'Error', options);
    }

    extractServerError(error): string {
      var message = 'An error has occurred:\n';

      // get all inner errors
      var errors = [];
      var current = error;
      while (current) {
        if (current.ExceptionMessage)
          errors.push(current.ExceptionMessage);

        current = current.InnerException;
      }

      if (errors.length > 0)
        message += errors.join(' \n');
      else
        message += ' Unknown error occurred';

      return message;
    }

    extractModelState(error): string {
      var message = 'The request is invalid: \n';

      if (!error.ModelState)
        return message;

      // get all model state errors
      var errors = [];
      for (var key in error.ModelState) {
        if (error.ModelState.hasOwnProperty(key)) {
          // property name
          var parts = key.split('.');
          var name = parts.pop();

          // all errors for property
          for (var i = 0; i < error.ModelState[key].length; i++) {
            errors.push(name + ': ' + error.ModelState[key][i]);
          }
        }
      }

      message += errors.join(' \n');
      return message;
    }
  }

  // register service
  angular.module(Estimatorx.applicationName)
    .service('logger', [
      'toastr',
      Logger
    ]);
}  
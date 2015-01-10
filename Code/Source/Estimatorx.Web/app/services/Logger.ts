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
            'toaster'
        ];


        constructor(toaster) {
            var self = this;
            self.toaster = toaster;
        }

        toaster: any;

        showAlert(options: AlertOptions) {
            var self = this;
            var d: AlertOptions = {
                type: 'warn',
                title: 'Complete',
                message: 'Unrecognized server response, please verify operation completed correctly.',
                timeOut: 60000
            };

            var o = <AlertOptions>$.extend({}, d, options);
            self.toaster.pop(o.type, o.title, o.message, o.timeOut);
        }        
        
        handelError(data, status?, headers?, config?) {
            var self = this;
            var message = '';

            if (status.slice(0, 3) == '400')
                message = self.extractModelState(data);
            else if (status.slice(0, 3) == '500')
                message = self.extractServerError(data);
            else if (status)
                message += 'An error has occurred:\n ' + status;
            else
                message += 'An error has occurred:\n Unrecognized server response';

            // clear others to prevents flood of toast
            self.toaster.clear();
            self.toaster.pop('error', 'Error', message, 60000);
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
            'toaster',
            Logger
        ]);
}  
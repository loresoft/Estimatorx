/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class TemplateListController {

        // project for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            'logger',
            'templateSummaryRepository'
        ];

        constructor($scope, logger: Logger, templateSummaryRepository: TemplateSummaryRepository) {
            // assign viewModel to controller
            $scope.viewModel = this;

            this.logger = logger;
            this.repository = templateSummaryRepository;

            // default
            this.result = <IQueryResult<ITemplate>>{
                Page: 1,
                PageSize: 10,
                Sort: 'Updated',
                Descending: true
            };

            this.load();
        }

        logger: Logger;
        repository: TemplateSummaryRepository;

        result: IQueryResult<ITemplate>;
        sort = angular.bind(this, this.load);

        load() {
            var self = this;

            var request = <IQueryRequest>{
                Page: self.result.Page,
                PageSize: self.result.PageSize,
                Sort: self.result.Sort,
                Descending: self.result.Descending
            };

            this.repository.query(request)
                .success((data, status, headers, config) => {
                    self.result = data;
                })
                .error(self.logger.handelError);
        }

        sortClick(column: string) {
            var self = this;

            if (self.result.Sort == column)
                self.result.Descending = !self.result.Descending;
            else
                self.result.Descending = false;

            self.result.Sort = column;

            self.load();
        }

    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('templateListController',
        [
            '$scope',
            'logger',
            'templateSummaryRepository',
            TemplateListController
        ]
    );
}


/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class TemplateListController implements angular.IController {

        // project for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            'logger',
            'templateRepository',
            'organizationRepository'
        ];

        constructor($scope,
            logger: Logger,
            templateRepository: TemplateRepository,
            organizationRepository: OrganizationRepository
        ) {
            // assign viewModel to controller
            $scope.viewModel = this;

            this.logger = logger;
            this.templateRepository = templateRepository;
            this.organizationRepository = organizationRepository;

            // default
            this.result = <IQueryResult<ITemplate>>{
                Page: 1,
                PageSize: 10,
                Sort: 'Updated',
                Descending: true
            };

            this.init();
            this.load();
        }

        logger: Logger;
        templateRepository: TemplateRepository;

        searchText: string;
        organizationId: string;

        organizations: IOrganization[];
        organizationRepository: OrganizationRepository;

        result: IQueryResult<ITemplate>;
        sort = angular.bind(this, this.load);

        init() {
            var self = this;

            self.organizationRepository.all()
                .then((response) => {
                    self.organizations = response.data;
                })
                .catch(self.logger.handelErrorProxy);
        }

        load() {
            var self = this;

            var request = <IQuerySearch>{
                Page: self.result.Page,
                PageSize: self.result.PageSize,
                Sort: self.result.Sort,
                Descending: self.result.Descending,
                Search: self.searchText,
                Organization: self.organizationId
            };

            this.templateRepository.query(request)
                .then((response) => {
                    self.result = response.data;
                })
                .catch(self.logger.handelErrorProxy);
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

        search() {
            var self = this;

            self.load();
        }

        $onInit = () => { };
    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('templateListController', TemplateListController);
}


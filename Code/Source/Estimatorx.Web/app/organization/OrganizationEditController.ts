/// <reference path="../_ref.ts" />

module Estimatorx {
    "use strict";

    export class OrganizationEditController {

        // protect for minification, must match contructor signiture.
        static $inject = [
            '$scope',
            '$modal',
            'modelFactory',
            'organizationRepository',
            'userRepository'
        ];

        constructor(
            $scope,
            $modal: any,
            modelFactory: ModelFactory,
            organizationRepository: OrganizationRepository,
            userRepository: UserRepository
            ) {
            var self = this;

            // assign viewModel to controller
            $scope.viewModel = this;
            self.$scope = $scope;
            self.$modal = $modal;

            self.modelFactory = modelFactory;
            self.organizationRepository = organizationRepository;
            self.userRepository = userRepository;

            self.organization = <IOrganization>{};
        }

        $scope: any;
        $modal: any;
        modelFactory: ModelFactory;

        organizationRepository: OrganizationRepository;
        organization: IOrganization;
        organizationId: string;

        userRepository: UserRepository;
        userId: string;

        members: IUser[];
        owners: IUser[];

        isSelf(id: string): boolean {
            var self = this;
            return id === self.userId;
        }

        selfOwner(): boolean {
            var self = this;
            return self.isOwner(self.userId);
        }

        isOwner(id: string): boolean {
            var self = this;
            if (!id || !self.organization || !self.organization.Owners || self.organization.Owners.length === 0)
                return false;

            return _.contains(self.organization.Owners, id);
        }

        load(organizationId?: string, userId?: string) {
            var self = this;

            self.organizationId = organizationId;
            self.userId = userId;

            // get organization id
            if (!self.organizationId) {
                self.organization = self.modelFactory.createOrganization();
                return;
            }

            self.loadOrganization();
        }

        loadOrganization() {
            var self = this;

            self.organizationRepository.find(self.organizationId)
                .success((data, status, headers, config) => {
                    self.organization = data;

                    self.loadMembers();
                    self.loadOwners();
                })
                .error((data, status, headers, config) => {
                    if (status == 404) {
                        self.organization = self.modelFactory.createOrganization(self.organizationId, self.userId);
                        return;
                    }

                    // TODO show error
                });
        }

        loadMembers() {
            var self = this;

            self.userRepository.organizationMembers(self.organizationId)
                .success((data, status, headers, config) => {
                    self.members = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        loadOwners() {
            var self = this;

            self.userRepository.organizationOwners(self.organizationId)
                .success((data, status, headers, config) => {
                    self.owners = data;
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }

        save() {
            var self = this;

            this.organizationRepository.save(this.organization)
                .success((data, status, headers, config) => {
                    self.organization = data;

                    self.loadMembers();
                    self.loadOwners();
                })
                .error((data, status, headers, config) => {
                    // TODO show error
                });
        }


        addMember() {
            var self = this;

            console.log('Add Member');

            var modalInstance = self.$modal.open({
                templateUrl: 'memberModal.html',
                controller: 'memberModalController'
            });

            modalInstance.result.then((userId: string) => {
                self.userRepository.addOrganization(self.organizationId, userId)
                    .success((data, status, headers, config) => {
                        self.loadMembers();
                    })
                    .error((data, status, headers, config) => {
                        // TODO show error
                    });
            });

        }

        removeMember(user: IUser) {
            var self = this;

            // don't remove self
            if (user.Id == self.userId)
                return;

            BootstrapDialog.confirm("Are you sure you want to remove this member?", (result) => {
                if (!result)
                    return;

                self.userRepository.removeOrganization(self.organizationId, user.Id)
                    .success((data, status, headers, config) => {
                        self.loadMembers();
                    })
                    .error((data, status, headers, config) => {
                        // TODO show error
                    });
            });
        }

        addOwner() {
            var self = this;

            var modalInstance = self.$modal.open({
                templateUrl: 'memberModal.html',
                controller: 'memberModalController'
            });

            modalInstance.result.then((userId: string) => {
                self.organization.Owners.push(userId);
                self.save();
            });
        }

        removeOwner(user: IUser) {
            var self = this;

            // don't remove self
            if (user.Id === self.userId)
                return;

            BootstrapDialog.confirm("Are you sure you want to remove this owner?", (result) => {
                if (!result)
                    return;

                for (var i = 0; i < self.organization.Owners.length; i++) {
                    if (self.organization.Owners[i] === user.Id) {
                        self.organization.Owners.splice(i, 1);
                        break;
                    }
                }

                self.save();
            });
        }

    }

    // register controller
    angular.module(Estimatorx.applicationName)
        .controller('organizationEditController', [
            '$scope',
            '$modal',
            'modelFactory',
            'organizationRepository',
            'userRepository',
            OrganizationEditController
        ]);
}


/// <reference path="../_ref.ts" />

module Estimatorx {
  "use strict";

  export class UserEditController {

    // protect for minification, must match contructor signiture.
    static $inject = [
      '$scope',
      '$location',
      'logger',
      'modelFactory',
      'userRepository'
    ];

    constructor(
      $scope,
      $location: angular.ILocationService,
      logger: Logger,
      modelFactory: ModelFactory,
      userRepository: UserRepository) {
      var self = this;

      // assign viewModel to controller
      $scope.viewModel = this;
      self.$scope = $scope;
      self.$location = $location;

      self.logger = logger;
      self.modelFactory = modelFactory;
      self.userRepository = userRepository;
      self.user = <IUser>{};
      self.password = <IPassword>{};

      // watch for navigation
      $(window).bind('beforeunload', () => {
        // prevent navigation by returning string
        if (self.isDirty())
          return 'You have unsaved changes!';
      });
    }

    $scope: any;
    $location: angular.ILocationService;
    logger: Logger;
    modelFactory: ModelFactory;

    userRepository: UserRepository;

    original: IUser;
    user: IUser;
    userId: string;

    password: IPassword;

    init() {

    }

    load(id?: string) {
      var self = this;

      self.userId = id;

      // get user id
      if (!self.userId) {
        return;
      }

      this.userRepository.find(self.userId)
        .success((data, status, headers, config) => {
          self.loadDone(data);
        })
        .error(self.logger.handelErrorProxy);
    }

    loadDone(user: IUser) {
      var self = this;

      self.original = <IUser>angular.copy(user, {});
      self.user = user;

      self.setClean();
    }

    save(valid: boolean) {
      var self = this;
      if (!valid) {
        self.logger.showAlert({
          type: 'error',
          title: 'Validation Error',
          message: 'A form field has a validation error. Please fix the error to continue.',
          timeOut: 4000
        });

        return;
      }

      this.userRepository.save(this.user)
        .success((data, status, headers, config) => {
          self.loadDone(data);
          self.logger.showAlert({
            type: 'success',
            title: 'Save Successful',
            message: 'User saved successfully.',
            timeOut: 4000
          });
        })
        .error(self.logger.handelErrorProxy);
    }

    undo() {
      var self = this;

      BootstrapDialog.confirm("Are you sure you want to undo changes?", (result) => {
        if (!result)
          return;

        self.user = <IUser>angular.copy(self.original, {});

        self.setClean();

        self.$scope.$applyAsync();
      });
    }

    delete() {
      var self = this;

      BootstrapDialog.confirm("Are you sure you want to delete this user?", (result) => {
        if (!result)
          return;

        self.userRepository.delete(self.user.Id)
          .success((data, status, headers, config) => {
            self.logger.showAlert({
              type: 'success',
              title: 'Delete Successful',
              message: 'User deleted successfully.',
              timeOut: 4000
            });

            //redirect
            window.location.href = 'User';
          })
          .error(self.logger.handelErrorProxy);
      });
    }

    isDirty(): boolean {
      return this.$scope.profileForm.$dirty;
    }

    setDirty() {
      this.$scope.profileForm.$setDirty();
    }

    setClean() {
      this.$scope.profileForm.$setPristine();
      this.$scope.profileForm.$setUntouched();
    }


    setPassword(valid: boolean) {
      var self = this;

      if (!valid) {
        self.logger.showAlert({
          type: 'error',
          title: 'Validation Error',
          message: 'A form field has a validation error. Please fix the error to continue.',
          timeOut: 4000
        });

        return;
      }

      self.userRepository.setPassword(self.password)
        .success((data, status, headers, config) => {
          self.logger.showAlert({
            type: 'success',
            title: 'Save Successful',
            message: 'Password changed successfully.',
            timeOut: 4000
          });

          self.resetPasswordForm();
          self.load(self.userId);
        })
        .error(self.logger.handelErrorProxy);
    }

    removeLogin(provider: string, key: string) {
      var self = this;

      BootstrapDialog.confirm("Are you sure you want to remove this login?", (result) => {
        if (!result)
          return;

        this.userRepository.removeLogin(provider, key)
          .success((data, status, headers, config) => {
            self.load(self.userId);
          })
          .error(self.logger.handelErrorProxy);
      });

    }

    resetPasswordForm() {
      var self = this;

      self.password = <IPassword>{};
      self.$scope.passwordForm.$setPristine();
      self.$scope.passwordForm.$setUntouched();
    }


    toggleSelection(role) {
      var self = this;

      if (!self.user.Roles)
        self.user.Roles = [];

      var idx = self.user.Roles.indexOf(role);

      if (idx > -1) {
        self.user.Roles.splice(idx, 1);
      }
      else {
        self.user.Roles.push(role);
      }

      self.setDirty();
    };
  }

  // register controller
  angular.module(Estimatorx.applicationName)
    .controller('userEditController',
    [
      '$scope',
      '$location',
      'logger',
      'modelFactory',
      'userRepository',
      UserEditController
    ]);
}


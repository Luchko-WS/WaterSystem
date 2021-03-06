﻿(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ManageUsersCtrl', ManageUsersCtrl);

    ManageUsersCtrl.$inject = ['UsersManagementService', '$uibModal', 'MessageService'];

    function ManageUsersCtrl(UsersManagementService, $uibModal, MessageService) {
        var vm = this;

        vm.changeLockState = changeLockState;
        vm.changePassword = changePassword;
        vm.manageUserRoles = manageUserRoles;
        vm.deleteUser = deleteUser;

        vm.loaded = false;
        vm.users = [];

        init();
      
        function init() {
            loadUsers();
        }

        function loadUsers() {
            UsersManagementService.getUsersList()
                .success(function (data) {
                    vm.users = data;
                    vm.loaded = true;
                })
                .error(_errorHandler);
        }

        function changePassword(user) {
            $uibModal.open({
                templateUrl: '/Account/ChangePasswordForUser',
                controller: 'ChangePasswordForUserCtrl',
                controllerAs: 'vm',
                resolve: {
                    _user: user
                }
            });
        }

        function changeLockState(user) {
            var lockState = user.isLocked;
            UsersManagementService.setLockState(user.userName, lockState)
                .success(function (data) {
                    console.log('lock state is changed');
                })
                .error(function (error) {
                    user.isLocked = lockState;
                    _errorHandler(error);
                });
        }

        function manageUserRoles(user) {
            $uibModal.open({
                templateUrl: '/Account/ManageUserRoles',
                controller: 'ManageUserRolesCtrl',
                controllerAs: 'vm',
                resolve: {
                    _user: user
                }
            });
        }

        function deleteUser(user) {
            MessageService.showMessageYesNo("removeUserQuestion", "removeUser", { userName: user.userName }).then(function (result) {
                if (result === "OK") {
                    UsersManagementService.deleteUser(user.id)
                        .success(function (response) {
                            for (var i in vm.users) {
                                if (vm.users[i].id === user.id) {
                                    vm.users.splice(i, 1);
                                    break;
                                }
                            }
                        })
                        .error(_errorHandler);
                }
            });
        }

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }
})();

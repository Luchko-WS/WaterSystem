(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ManageUserRolesCtrl', ManageUserRolesCtrl);

    ManageUserRolesCtrl.$inject = ['$uibModalInstance', 'UsersManagementService', 'MessageService', '_user'];

    function ManageUserRolesCtrl($uibModalInstance, UsersManagementService, MessageService, _user) {
        var vm = this;
        vm.changeRole = changeRole;
        vm.cancel = cancel;

        activate();

        function activate() {
            vm.user = _user;
            vm.loaded = false;
            UsersManagementService.getAllRoles()
                .success(function (data) {
                    vm.loaded = true;
                    vm.roles = data;

                    for (var i = 0; i < vm.roles.length; i++) {
                        var res = false;
                        for (var j = 0; j < vm.roles[i].users.length; j++) {
                            if (vm.roles[i].users[j].userId == vm.user.id) {
                                res = true;
                                break;
                            }
                        }
                        vm.roles[i].containCurrentUser = res;
                    }
                })
                .error(_errorHandler);
        }

        function changeRole(role) {
            if (role.containCurrentUser) {
                UsersManagementService.addUserToRole(vm.user.userName, role.name)
                    .success(function (data) {
                        MessageService.showMessage("userAddedToRole", "rolesManagement");
                    })
                    .error(function (error) {
                        role.containCurrentUser = !role.containCurrentUser;
                        _errorHandler(error);
                    });
            }
            else {
                UsersManagementService.removeUserFromRole(vm.user.userName, role.name)
                    .success(function (data) {
                        MessageService.showMessage("userRemovedFromRole", "rolesManagement");
                    })
                    .error(function (error) {
                        role.containCurrentUser = !role.containCurrentUser;
                        _errorHandler(error);
                    });
            }
        }

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }
    }
})();
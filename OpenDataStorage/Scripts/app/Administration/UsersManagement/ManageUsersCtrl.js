(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ManageUsersCtrl', ManageUsersCtrl);

    ManageUsersCtrl.$inject = ['UsersManagementService', 'MessageService'];

    function ManageUsersCtrl(UsersManagementService, MessageService) {
        var vm = this;

        vm.changeLockState = changeLockState;
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

        function deleteUser(user) {
            MessageService.showMessageYesNo("ng_DeleteUser_ConfirmMessage", "ng_DeleteUser", { userName: user.userName }).then(function (result) {
                if (result === "OK") {
                    UsersManagementService.deleteUser(user.userName)
                        .success(function (response) {
                            if (response.status === 200) {
                                MessageService.showMessage("ng_DeleteUser_ResultedMessage", "ng_DeleteUser");
                                for (var i in vm.users) {
                                    if (vm.users[i].id === user.id) {
                                        vm.users.splice(i, 1);
                                        break;
                                    }
                                }
                            }
                        })
                        .error(_errorHandler);
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

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }
})();

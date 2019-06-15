(function () {
    'use strict';
    angular
        .module('MainApp')
        .controller('AddUserToRoleCtrl', AddUserToRoleCtrl);

    AddUserToRoleCtrl.$inject = ['$uibModalInstance', 'MessageService', 'UserService', 'mrOptions'];

    function AddUserToRoleCtrl($uibModalInstance, MessageService, UserService, mrOptions) {

        var vm = this;
        vm.cancel = cancel;
        vm.addUserToRole = addUserToRole;
        vm.isDefault = false;
        vm.userNames = [];
        activate();

        function activate() {
            vm.selectedRoleName = mrOptions.roleName;
            loadUserNames();
        }

        //loading names of users for autocomplete widget
        function loadUserNames() {
            UserService.loadUserNamesNotInRole(mrOptions.roleId)
                .success(function(data) {
                    vm.userNames = data;
                });
        }

        function addUserToRole() {
            UserService.addUserToRole(vm.userName.title, vm.isDefault, mrOptions.roleId).then(function (response) {
                if (response.status === 200) {
                    $uibModalInstance.close(true);
                } else {
                    MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
                }
            }, function (error) {
                MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
            });
        }
        
        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }
    }
})();
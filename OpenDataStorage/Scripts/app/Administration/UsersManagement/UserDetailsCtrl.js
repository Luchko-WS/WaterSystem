(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('UserDetailsCtrl', UserDetailsCtrl);

    UserDetailsCtrl.$inject = ['$uibModalInstance', '$uibModal', 'userDetails', 'UserService', 'MessageService', '$window'];

    function UserDetailsCtrl($uibModalInstance, $uibModal, userDetails, UserService, MessageService, $window) {
        /* jshint validthis:true */
        var vm = this;

        vm.loaded = false;
        vm.isEmailPublicChanged = false;
        vm.changeEmailState = function () { vm.isEmailPublicChanged = true; }
        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); }
        vm.saveUserDetails = saveUserDetails;
        vm.deleteAccount = deleteAccount;
        vm.changePassword = changePassword;

        activate();

        function activate() {
            loadUser();
        }

        function deleteAccount() {
            MessageService.showMessageYesNo("ng_DeleteUser_ConfirmMessage", "ng_DeleteUser", { userName: vm.user.userName }).then(function (result) {
                if (result === "OK") {
                    UserService.deleteUser(vm.user.id).then(function (response) {
                        if (response.status === 200) {
                            MessageService.showMessage("ng_DeleteUser_ResultedMessage", "ng_DeleteUser");
                            
                            $uibModalInstance.dismiss('cancel');
                            $window.location.reload();
                        }
                    }, failed);
                }
            });
        }

        function failed(error) {
            MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
        }

        function saveUserDetails() {
            UserService.saveUserDetails(vm.user.id, vm.user.abbreviation, vm.user.logLevel, vm.user.isPhonePublic, vm.user.isEmailPublic).then(function (response) {
                if (response.status !== 200) {
                    MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
                } else {
                    $uibModalInstance.close({ id: vm.user.id, abbreviation: vm.user.abbreviation });
                }
            }, function (error) {
                MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
            });
        }

        function loadUser() {
            UserService.getUser(userDetails.userId).then(function(response) {
                vm.loaded = true;
                vm.user = response.data;
            });
        }

        function changePassword(user) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Account/ChangePasswordForUser',
                controller: 'ChangePasswordForUserCtrl',
                controllerAs: 'vm',
                size: 'md',
                backdrop: 'static',
                resolve: { user: user }
            });
        }
    }
})();
(function(){
    angular
        .module('MainApp')
        .controller('ChangePasswordForUserCtrl', ChangePasswordForUserCtrl);

    ChangePasswordForUserCtrl.$inject = ['$uibModalInstance', 'MessageService', 'UsersManagementService', '_user'];

    function ChangePasswordForUserCtrl($uibModalInstance, MessageService, UsersManagementService, _user) {

        var vm = this;
        vm.submitForm = submitForm;
        vm.cancel = cancel;

        vm.passwordMinLength = 6;
        vm.passwordMaxLength = 100;
        vm.validateConfirmPassword = validateConfirmPassword;

        init();

        function init() {
            vm.user = _user;
        }

        function submitForm() {
            UsersManagementService.changePassword(vm.user.userName, vm.userPassword.newPassword)
                .success(function (data) {
                    $uibModalInstance.close("save");
                    MessageService.showMessage("passwordForUserChanged", "changePassword");
                })
                .error(_errorHandler);
        }

        function validateConfirmPassword(form) {
            if (form.ConfirmPassword.$dirty) {
                if (vm.userPassword.newPassword === vm.userPassword.confirmPassword) {
                    form.ConfirmPassword.$setValidity("passwordMismatch", true);
                }
                else {
                    form.ConfirmPassword.$setValidity("passwordMismatch", false);
                }
            }
        }

        function _errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }
    }
})();


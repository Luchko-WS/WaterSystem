(function(){
    angular
        .module('MainApp')
        .controller('ChangePasswordForUserCtrl', ChangePasswordForUserCtrl);

    ChangePasswordForUserCtrl.$inject = ['$uibModalInstance', 'EmailService', 'MessageService', 'UserService', 'user'];

    function ChangePasswordForUserCtrl($uibModalInstance, EmailService, MessageService, UserService, user) {

        var vm = this;
        vm.submitForm = submitForm;
        vm.cancel = cancel;

        vm.passwordMinLength = 6;
        vm.passwordMaxLength = 100;
        vm.validateConfirmPassword = validateConfirmPassword;

        activate();

        function activate() {
            if (user) {
                vm.user = user;
            }
        }

        function submitForm() {
            UserService.changePassword(vm.user.id, vm.userPassword.newPassword, vm.user.email, vm.user.language)
                .then(successResult, failedResult);
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

        function successResult(response) {
            if (response.status === 200) {
                $uibModalInstance.close("save");
                MessageService.showMessage("ng_PasswordForUserChanged_SuccessMessage", "ng_PasswordForUserChanged_SuccessTitle");
            }
        }

        function failedResult(error) {
            MessageService.showMessage("ng_Error_Try_Latter", "ng_Error_Title");
        }

        function cancel() {
            $uibModalInstance.dismiss('cancel');
        }
    }
})();


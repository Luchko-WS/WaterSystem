(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('MessageCtrl', MessageCtrl);
    MessageCtrl.$inject = ['$translate', '$uibModalInstance', 'modalParams'];

    function MessageCtrl($translate, $uibModalInstance, modalParams) {
        var vm = this;
        vm.ok = ok;
        vm.cancel = cancel;
        vm.closeForResult = closeForResult;
        vm.modalParams = modalParams;

        function ok() {
            $uibModalInstance.close("OK");
        }

        function cancel() {
            $uibModalInstance.dismiss("Cancel");
        }

        function closeForResult(result) {
            $uibModalInstance.close(result);
        }
    }
})();
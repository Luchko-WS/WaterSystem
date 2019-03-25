(function () {
    'use strict';

    //service
    angular
        .module('MainApp')
        .factory('MessageService', MessageService);
    MessageService.$inject = ['$uibModal'];

    function MessageService($uibModal) {
        function showMessage(message, header) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Templates/MessageServiceDialogWindows/OkDialogWindow.html',
                controller: 'MessageCtrl',
                controllerAs: "vm",
                resolve: {
                    modalParams: {
                        message: message,
                        header: header
                    }
                }
            });
            return modalInstance.result;
        }

        function showMessageYesNo(message, header) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Templates/MessageServiceDialogWindows/YesNoDialogWindow.html',
                controller: 'MessageCtrl',
                controllerAs: "vm",
                resolve: {
                    modalParams: {
                        message: message,
                        header: header
                    }
                }
            });
            return modalInstance.result;
        }

        function showMessageCustom(message, header, buttons) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Templates/MessageServiceDialogWindows/CustomDialogWindow.html',
                controller: 'MessageCtrl',
                controllerAs: "vm",
                resolve: {
                    modalParams: {
                        message: message,
                        header: header,
                        buttons: buttons
                    }
                }
            });
            return modalInstance.result;
        }

        return {
            showMessage: showMessage,
            showMessageYesNo: showMessageYesNo,
            showMessageCustom: showMessageCustom
        };
    }

    //controller
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
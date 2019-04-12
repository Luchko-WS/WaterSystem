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
})();
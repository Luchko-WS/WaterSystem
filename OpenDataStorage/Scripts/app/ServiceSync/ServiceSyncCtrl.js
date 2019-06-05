(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ServiceSyncCtrl', ServiceSyncCtrl);

    ServiceSyncCtrl.$inject = ['$uibModal', 'ServiceSyncService', 'MessageService'];

    function ServiceSyncCtrl($uibModal, ServiceSyncService, MessageService) {
        var vm = this;

        vm.sync = sync;

        init();

        function init() {
            vm.loaded = true;
        }

        function sync() {
            vm.loaded = false;
            ServiceSyncService.sync()
                .success(function (data) {
                    vm.loaded = true;
                })
                .error(_errorHandler);
        }

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
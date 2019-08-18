(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('DataSyncCtrl', DataSyncCtrl);

    DataSyncCtrl.$inject = ['$uibModal', 'DataSyncService', 'MessageService'];

    function DataSyncCtrl($uibModal, DataSyncService, MessageService) {
        var vm = this;

        vm.sync = sync;

        init();

        function init() {
            vm.loaded = true;
        }

        function sync() {
            vm.loaded = false;
            DataSyncService.sync()
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
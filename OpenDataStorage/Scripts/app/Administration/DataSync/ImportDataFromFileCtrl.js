(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ImportDataFromFileCtrl', ImportDataFromFileCtrl);

    ImportDataFromFileCtrl.$inject = ['$uibModal', 'DataSyncService', 'MessageService', 'common'];

    function ImportDataFromFileCtrl($uibModal, DataSyncService, MessageService, common) {
        var vm = this;

        vm.file = undefined;
        vm.loaded = true;
        vm.uploadFile = uploadFile;

        function uploadFile() {
            if (vm.file) {
                vm.loaded = false;
                DataSyncService.uploadSacmigDataFile(common.id, vm.file)
                    .success(function (response) {
                        //to do smth here
                        vm.loaded = true;
                    })
                    .error(function (error) {
                        vm.loaded = true;
                        _errorHandler(error);
                    });
            }
        }

        function _errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
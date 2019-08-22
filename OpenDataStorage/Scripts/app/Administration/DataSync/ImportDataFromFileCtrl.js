(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ImportDataFromFileCtrl', ImportDataFromFileCtrl);

    ImportDataFromFileCtrl.$inject = ['$uibModal', 'DataSyncService', 'HierarchyObjectService', 'AppConstantsService', 'MessageService', 'common'];

    function ImportDataFromFileCtrl($uibModal, DataSyncService, HierarchyObjectService, AppConstantsService, MessageService, common) {
        var vm = this;

        vm.file = undefined;
        vm.loaded = true;
        vm.uploadFile = uploadFile;

        init();

        function init() {
            loadObject();
            loadObjectsTree();

            vm.treeConfig = AppConstantsService.getSelectableDefaultTreeConfig();
            vm.nodeSelectedCallback = function (event, data) {
                vm.object = data;
            };
            vm.nodeUnselectedCallback = function (event, data) {
                vm.object = null;
            };
        }

        function loadObject() {
            if (common && common.id) {
                vm.objectsLoaded = false;
                HierarchyObjectService.get(common.id)
                    .success(function (data) {
                        vm.object = data;
                        vm.objectsLoaded = true;
                    })
                    .error(function (error) {
                        vm.objectsLoaded = true;
                        _errorHandler(error);
                    });
            }
        }

        function loadObjectsTree() {
            vm.objectsLoaded = false;
            HierarchyObjectService.getTree()
                .success(function (data) {
                    vm.objectsTree = data;
                    vm.objectsLoaded = true;
                })
                .error(function (error) {
                    vm.objectsLoaded = true;
                    _errorHandler(error);
                });
        }

        function uploadFile() {
            if (vm.file && vm.object) {
                vm.loaded = false;
                DataSyncService.uploadSacmigDataFile(vm.object.id, vm.file)
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
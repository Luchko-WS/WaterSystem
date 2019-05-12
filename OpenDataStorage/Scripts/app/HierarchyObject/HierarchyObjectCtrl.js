(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('HierarchyObjectCtrl', HierarchyObjectCtrl);

    HierarchyObjectCtrl.$inject = ['$uibModal', 'common', 'HierarchyObjectService', 'ObjectTypeService', 'DataService', 'MessageService', 'AppConstantsService', 'requestInfo'];

    function HierarchyObjectCtrl($uibModal, common, HierarchyObjectService, ObjectTypeService, DataService, MessageService, AppConstantsService, requestInfo) {
        var vm = this;

        vm.edit = edit;
        vm.remove = remove;

        init();

        function init() {
            if (requestInfo.isAuthenticated === 'True') vm.enableEditing = true;
            loadObject();
            loadAllData();
        }

        function loadObject() {
            vm.objLoaded = false;
            HierarchyObjectService.get(common.id)
                .success(function (data) {
                    vm.obj = data;
                    document.title = vm.obj.name;
                    vm.objLoaded = true;
                })
                .error(function (error) {
                    vm.objLoaded = true;
                    _errorHandler(error);
                });
        }

        function loadAllData() {
            vm.dataLoaded = false;
            DataService.getDataForObject(common.id)
                .success(function (data) {
                    console.log(data);
                    vm.data = data;
                    vm.dataLoaded = true;
                })
                .error(function (error) {
                    vm.dataLoaded = true;
                    _errorHandler(error);
                });
        }

        function edit() {
            var modalInstance = $uibModal.open({
                templateUrl: '/HierarchyObject/CreateEditObject',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'edit',
                        node: vm.obj,
                        init: {
                            initPromise: ObjectTypeService.getTree,
                            initSuccessCallback: _modalwindowInitSuccessCallback,
                            initErrorCallback: _modalwindowInitErrorCallback
                        },
                        tree: {
                            treeParserConfig: AppConstantsService.getDefaultTreeConfig()
                        }
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    _editObjectNode(model.node);
                });
        }

        function _editObjectNode(obj) {
            HierarchyObjectService.update(obj)
                .success(function (data) {
                    loadObject();
                })
                .error(_errorHandler);
        }

        function remove() {
            MessageService.showMessageYesNo('removeObjectQuestion', 'removeObject')
                .then(function (result) {
                    if (result === 'OK') {
                        HierarchyObjectService.delete(vm.obj.id)
                            .success(function (data) {
                                window.location.href = '/HierarchyObject/Tree';
                            })
                            .error(_errorHandler);
                    }
                });
        }

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }

        function _modalwindowInitSuccessCallback(data, node) {
            return {
                treeElement: {
                    tree: data,
                    treeParserConfig: AppConstantsService.getOnlySelectableFileFsTreeConfig(),
                    callbacks: {
                        nodeSelectedCallback: function (event, data) {
                            node.objectType = data;
                        },
                        nodeUnselectedCallback: function (event, data) {
                            node.objectType = null;
                        }
                    }
                },
                loaded: true
            };
        }

        function _modalwindowInitErrorCallback(error, node) {
            return {
                loaded: true
            };
        }
    }

})();
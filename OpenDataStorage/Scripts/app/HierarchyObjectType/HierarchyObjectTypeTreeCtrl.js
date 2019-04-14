﻿(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('HierarchyObjectTypeTreeCtrl', HierarchyObjectTypeTreeCtrl);

    HierarchyObjectTypeTreeCtrl.$inject = ['$uibModal', 'HierarchyObjectTypeService', 'MessageService', 'AppConstantsService'];

    function HierarchyObjectTypeTreeCtrl($uibModal, HierarchyObjectTypeService, MessageService, AppConstantsService) {
        var vm = this;

        vm.tree = [];
        vm.state = {
            currentNode: null
        };

        vm.createType = createType;
        vm.createFolder = createFolder;
        vm.edit = edit;
        vm.remove = remove;
        vm.nodeSelectedCallback = nodeSelectedCallback;
        vm.nodeUnselectedCallback = nodeUnselectedCallback;

        init();

        function init() {
            vm.fsNodeTypes = AppConstantsService.getFSNodeTypes();
            vm.treeParserConfig = AppConstantsService.getDefaultFsTreeConfig();
            loadData();
        }

        function loadData() {
            vm.loaded = false;
            vm.tree = HierarchyObjectTypeService.getTree()
                .success(function (data) {
                    vm.tree = data;
                    vm.loaded = true;
                })
                .error(_errorHandler);
        }

        function createType() {
            var modalInstance = $uibModal.open({
                templateUrl: '/HierarchyObjectType/CreateEditType',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        parentNode: vm.state.currentNode
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    var parentNodeId = model.parentNode.id;
                    var node = model.node;
                    node.type = vm.fsNodeTypes.file;
                    _createTypeNode(parentNodeId, node);
                });
        }

        function createFolder() {
            var modalInstance = $uibModal.open({
                templateUrl: '/HierarchyObjectType/CreateEditFolder',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        parentNode: vm.state.currentNode
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    var parentNodeId = model.parentNode.id;
                    var node = model.node;
                    node.type = vm.fsNodeTypes.folder;
                    _createTypeNode(parentNodeId, node);
                });
        }

        function _createTypeNode(parentNodeId, node) {
            HierarchyObjectTypeService.create(parentNodeId, node)
                .success(function (data) {
                    loadData();
                })
                .error(_errorHandler);
        }

        function edit() {
            var modalInstance = $uibModal.open({
                templateUrl: vm.state.currentNode.type === vm.fsNodeTypes.folder
                    ? '/HierarchyObjectType/CreateEditFolder'
                    : '/HierarchyObjectType/CreateEditCharacteristic',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        node: vm.state.currentNode
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    _editTypeNode(model.node);
                });
        }

        function _editTypeNode(node) {
            HierarchyObjectTypeService.update(node)
                .success(function (data) {
                    loadData();
                })
                .error(_errorHandler);
        }

        function remove() {
            MessageService.showMessageYesNo('removeQuestion', 'removeDictionary')
                .then(function (result) {
                    if (result === 'OK') {
                        HierarchyObjectTypeService.delete(vm.state.currentNode.id)
                            .success(function (data) {
                                loadData();
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

        function nodeSelectedCallback(event, data) {
            vm.state.currentNode = data;
        }

        function nodeUnselectedCallback(event, data) {
            vm.state.currentNode = null;
        }
    }

})();
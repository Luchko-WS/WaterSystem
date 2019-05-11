﻿(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ObjectTypeTreeCtrl', ObjectTypeTreeCtrl);

    ObjectTypeTreeCtrl.$inject = ['$uibModal', 'ObjectTypeService', 'MessageService', 'AppConstantsService'];

    function ObjectTypeTreeCtrl($uibModal, ObjectTypeService, MessageService, AppConstantsService) {
        var vm = this;

        vm.tree = [];

        vm.state = {
            currentNode: null,
            filter: null
        };
        vm.filter = filter;

        vm.createType = createType;
        vm.createFolder = createFolder;
        vm.edit = edit;
        vm.remove = remove;

        vm.nodeDblClickCallback = nodeDblClickCallback;
        vm.nodeDropCallback = nodeDropCallback;
        vm.nodeSelectedCallback = nodeSelectedCallback;
        vm.nodeUnselectedCallback = nodeUnselectedCallback;

        init();

        function init() {
            vm.fsNodeTypes = AppConstantsService.getFSNodeTypes();
            vm.treeParserConfig = AppConstantsService.getDefaultFsTreeConfig();
            loadData();
        }

        function loadData(selectedNode) {
            vm.loaded = false;
            ObjectTypeService.getTree(vm.state.filter)
                .success(function (data) {
                    vm.tree = data;
                    vm.loaded = true;
                    vm.treeParserConfig.treeConfig.nodesConfig.expandEachNode = Boolean(vm.state.filter !== null);
                    if (selectedNode) {
                        vm.state.currentNode = selectedNode;
                        vm.treeParserConfig.definedValues.selectedNode = selectedNode;
                    }
                })
                .error(_errorHandler);
        }

        function createType() {
            var modalInstance = $uibModal.open({
                templateUrl: '/ObjectType/CreateEditType',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'create',
                        parentNode: vm.state.currentNode
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    var parentNodeId = model.parentNode.id;
                    var node = model.node;
                    node.entityType = vm.fsNodeTypes.file;
                    _createTypeNode(parentNodeId, node);
                });
        }

        function createFolder() {
            var modalInstance = $uibModal.open({
                templateUrl: '/ObjectType/CreateEditFolder',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'create',
                        parentNode: vm.state.currentNode
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    var parentNodeId = model.parentNode.id;
                    var node = model.node;
                    node.entityType = vm.fsNodeTypes.folder;
                    _createTypeNode(parentNodeId, node);
                });
        }

        function _createTypeNode(parentNodeId, node) {
            ObjectTypeService.create(parentNodeId, node)
                .success(function (data) {
                    resetFilter();
                    loadData(data);
                })
                .error(_errorHandler);
        }

        function edit() {
            var modalInstance = $uibModal.open({
                templateUrl: vm.state.currentNode.entityType === vm.fsNodeTypes.folder
                    ? '/ObjectType/CreateEditFolder'
                    : '/ObjectType/CreateEditType',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'edit',
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
            ObjectTypeService.update(node)
                .success(function (data) {
                    loadData(data);
                })
                .error(_errorHandler);
        }

        function remove() {
            MessageService.showMessageYesNo('removeTypeQuestion', 'removeType')
                .then(function (result) {
                    if (result === 'OK') {
                        ObjectTypeService.delete(vm.state.currentNode.id)
                            .success(function (data) {
                                loadData(data);
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

        function nodeDblClickCallback(event, data) {
            nodeSelectedCallback(event, data);
            $uibModal.open({
                templateUrl: data.entityType === vm.fsNodeTypes.folder
                    ? '/ObjectType/ReadFolder'
                    : '/ObjectType/ReadType',
                controller: 'ReadModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        node: data
                    }
                }
            });
        }

        function nodeSelectedCallback(event, data) {
            vm.state.currentNode = data;
        }

        function nodeUnselectedCallback(event, data) {
            vm.state.currentNode = null;
        }

        function nodeDropCallback(event, data) {
            var draggedNode = data.draggedNode;
            var droppabletNode = data.droppabletNode;
            _moveTypeNode(draggedNode.id, droppabletNode.id);
        }

        function _moveTypeNode(nodeId, parentNodeId) {
            ObjectTypeService.move(nodeId, parentNodeId)
                .success(function (data) {
                    loadData(data);
                })
                .error(function (error) {
                    _errorHandler(error);
                    loadData();
                });
        }

        function filter() {
            if (vm.typeNamePattern) {
                vm.state.filter = {
                    name: vm.typeNamePattern
                };
            }
            else {
                vm.state.filter = null;
            }
            loadData();
        }

        function resetFilter() {
            vm.typeNamePattern = null;
            vm.state.filter = null;
        }
    }

})();
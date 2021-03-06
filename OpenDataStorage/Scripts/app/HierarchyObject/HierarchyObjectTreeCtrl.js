﻿(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('HierarchyObjectTreeCtrl', HierarchyObjectTreeCtrl);

    HierarchyObjectTreeCtrl.$inject = ['$uibModal', 'HierarchyObjectService', 'ObjectTypeService', 'MessageService', 'AppConstantsService', 'requestInfo'];

    function HierarchyObjectTreeCtrl($uibModal, HierarchyObjectService, ObjectTypeService, MessageService, AppConstantsService, requestInfo) {
        var vm = this;

        vm.tree = [];

        vm.state = {
            currentNode: null,
            filter: null
        };
        vm.filter = filter;

        vm.create = create;
        vm.edit = edit;
        vm.remove = remove;
        vm.showDetails = _showDetails;

        vm.nodeDblClickCallback = nodeDblClickCallback;
        vm.nodeDropCallback = nodeDropCallback;
        vm.nodeSelectedCallback = nodeSelectedCallback;
        vm.nodeUnselectedCallback = nodeUnselectedCallback;

        init();

        function init() {
            vm.treeParserConfig = AppConstantsService.getDefaultTreeConfig();
            if (requestInfo.isAuthenticated === 'False') vm.treeParserConfig.treeConfig.draggable = false;
            loadData();
        }

        function loadData(selectedNode) {
            vm.loaded = false;
            HierarchyObjectService.getTree(vm.state.filter)
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

        function create() {
            var modalInstance = $uibModal.open({
                templateUrl: '/HierarchyObject/CreateEditObject',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'create',
                        parentNode: vm.state.currentNode,
                        init: {
                            initPromise: ObjectTypeService.getTree,
                            initSuccessCallback: _modalwindowInitSuccessCallback
                        }
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    var parentNodeId = model.parentNode.id;
                    model.node.objectTypeId = model.node.objectType ? model.node.objectType.id : null;
                    _createObjectNode(parentNodeId, model.node);
                });
        }

        function _createObjectNode(parentNodeId, node) {
            HierarchyObjectService.create(parentNodeId, node)
                .success(function (data) {
                    resetFilter();
                    loadData(data);
                })
                .error(_errorHandler);
        }

        function edit() {
            var modalInstance = $uibModal.open({
                templateUrl: '/HierarchyObject/CreateEditObject',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'edit',
                        node: vm.state.currentNode,
                        init: {
                            initPromise: ObjectTypeService.getTree,
                            initSuccessCallback: _modalwindowInitSuccessCallback
                        }
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    model.node.objectTypeId = model.node.objectType !== null ? model.node.objectType.id : null;
                    _editObjectNode(model.node);
                });
        }

        function _editObjectNode(node) {
            HierarchyObjectService.update(node)
                .success(function (data) {
                    loadData(data);
                })
                .error(_errorHandler);
        }

        function remove() {
            MessageService.showMessageYesNo('removeObjectQuestion', 'removeObject')
                .then(function (result) {
                    if (result === 'OK') {
                        HierarchyObjectService.delete(vm.state.currentNode.id)
                            .success(function (data) {
                                loadData(data);
                            })
                            .error(_errorHandler);
                    }
                });
        }

        function _errorHandler(error) {
            if(error) console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }

        function nodeDblClickCallback(event, data) {
            _showDetails(data);
        }

        function _showDetails(data) {
            window.open('/HierarchyObject/Details/' + data.id, '_blank');
        }

        function nodeSelectedCallback(event, data) {
            vm.state.currentNode = data;
        }

        function nodeUnselectedCallback(event, data) {
            vm.state.currentNode = null;
        }

        function nodeDropCallback(event, data) {
            var node = data.draggedNode;
            var parentNode = data.droppableNode;

            //validate. move to shared service
            if (parentNode.leftKey > node.leftKey && parentNode.rightKey < node.rightKey) {
                _errorHandler();
                return;
            }

            _moveObjectNode(node.id, parentNode.id);
        }

        function _moveObjectNode(nodeId, parentNodeId) {
            HierarchyObjectService.move(nodeId, parentNodeId)
                .success(function (data) {
                    loadData(data);
                })
                .error(function (error) {
                    _errorHandler(error);
                    loadData();
                });
        }

        function _modalwindowInitSuccessCallback(data, model) {
            return {
                treeElement: {
                    tree: data,
                    treeParserConfig: AppConstantsService.getOnlySelectableFileFsTreeConfig(),
                    callbacks: {
                        nodeSelectedCallback: function (event, data) {
                            model.node.objectType = data;
                        },
                        nodeUnselectedCallback: function (event, data) {
                            model.node.objectType = null;
                        }
                    }
                }
            };
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
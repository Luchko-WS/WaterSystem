(function () {
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
            vm.tree = ObjectTypeService.getTree(vm.state.filter)
                .success(function (data) {
                    vm.tree = data;
                    vm.loaded = true;
                    vm.treeParserConfig.nodesConfig.expandEachNode = Boolean(vm.state.filter !== null);
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
                templateUrl: '/ObjectType/CreateEditFolder',
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
            ObjectTypeService.create(parentNodeId, node)
                .success(function (data) {
                    resetFilter();
                    loadData(data);
                })
                .error(_errorHandler);
        }

        function edit() {
            var modalInstance = $uibModal.open({
                templateUrl: vm.state.currentNode.type === vm.fsNodeTypes.folder
                    ? '/ObjectType/CreateEditFolder'
                    : '/ObjectType/CreateEditType',
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
                templateUrl: data.type === vm.fsNodeTypes.folder
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
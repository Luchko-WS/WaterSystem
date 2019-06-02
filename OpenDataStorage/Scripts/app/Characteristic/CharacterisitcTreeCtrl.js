(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CharacteristicTreeCtrl', CharacteristicTreeCtrl);

    CharacteristicTreeCtrl.$inject = ['$uibModal', 'CharacteristicService', 'MessageService', 'AppConstantsService', 'requestInfo'];

    function CharacteristicTreeCtrl($uibModal, CharacteristicService, MessageService, AppConstantsService, requestInfo) {
        var vm = this;

        vm.tree = [];

        vm.state = {
            currentNode: null,
            filter: null
        };
        vm.filter = filter;

        vm.createCharacteristic = createCharacteristic;
        vm.createFolder = createFolder;
        vm.edit = edit;
        vm.remove = remove;
        vm.showDetails = _showDetails;

        vm.nodeDblClickCallback = nodeDblClickCallback;
        vm.nodeDropCallback = nodeDropCallback;
        vm.nodeSelectedCallback = nodeSelectedCallback;
        vm.nodeUnselectedCallback = nodeUnselectedCallback;

        init();

        function init() {
            vm.fsNodeTypes = AppConstantsService.getFSNodeTypes();
            vm.treeParserConfig = AppConstantsService.getDefaultFsTreeConfig();
            if (requestInfo.isAuthenticated === 'False') vm.treeParserConfig.treeConfig.draggable = false;
            loadData();
        }

        function loadData(selectedNode) {
            vm.loaded = false;
            CharacteristicService.getTree()
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

        function createCharacteristic() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/CreateEditCharacteristic',
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
                    _createCharacteristicNode(parentNodeId, node);
                });
        }

        function createFolder() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/CreateEditFolder',
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
                    _createCharacteristicNode(parentNodeId, node);
                });
        }

        function _createCharacteristicNode(parentNodeId, node) {
            CharacteristicService.create(parentNodeId, node)
                .success(function (data) {
                    resetFilter();
                    loadData(data);
                })
                .error(_errorHandler);
        }

        function edit() {
            var modalInstance = $uibModal.open({
                templateUrl: vm.state.currentNode.entityType === vm.fsNodeTypes.folder
                    ? '/Characteristic/CreateEditFolder'
                    : '/Characteristic/CreateEditCharacteristic',
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
                    _editCharacteristicNode(model.node);
                });
        }

        function _editCharacteristicNode(node) {
            CharacteristicService.update(node)
                .success(function (data) {
                    loadData(data);
                })
                .error(_errorHandler);
        }

        function remove() {
            MessageService.showMessageYesNo('removeCharacteristicQuestion', 'removeCharacteristic')
                .then(function (result) {
                    if (result === 'OK') {
                        CharacteristicService.delete(vm.state.currentNode.id)
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
            _showDetails(data);
        }

        function _showDetails(data) {
            $uibModal.open({
                templateUrl: data.entityType === vm.fsNodeTypes.folder
                    ? '/Characteristic/ReadFolder'
                    : '/Characteristic/ReadCharacteristic',
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
            _moveCharacteristicNode(draggedNode.id, droppabletNode.id);
        }

        function _moveCharacteristicNode(nodeId, parentNodeId) {
            CharacteristicService.move(nodeId, parentNodeId)
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
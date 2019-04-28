(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('HierarchyObjectTreeCtrl', HierarchyObjectTreeCtrl);

    HierarchyObjectTreeCtrl.$inject = ['$uibModal', 'HierarchyObjectService', 'MessageService', 'AppConstantsService'];

    function HierarchyObjectTreeCtrl($uibModal, HierarchyObjectService, MessageService, AppConstantsService) {
        var vm = this;

        vm.tree = [];
        vm.state = {
            currentNode: null,
            filter: null
        };

        vm.create = create;
        vm.edit = edit;
        vm.remove = remove;
        vm.nodeSelectedCallback = nodeSelectedCallback;
        vm.nodeUnselectedCallback = nodeUnselectedCallback;
        vm.filter = filter;

        init();

        function init() {
            vm.treeParserConfig = AppConstantsService.getDefaultTreeConfig();
            loadData();
        }

        function loadData(selectedNode) {
            vm.loaded = false;
            vm.tree = HierarchyObjectService.getTree()
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

        function create() {
            var modalInstance = $uibModal.open({
                templateUrl: '/HierarchyObject/CreateEditObject',
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
                    _createObjectNode(parentNodeId, node);
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
                        node: vm.state.currentNode
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
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
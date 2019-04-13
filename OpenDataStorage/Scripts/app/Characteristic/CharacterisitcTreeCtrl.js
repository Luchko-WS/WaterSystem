﻿(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CharacteristicTreeCtrl', CharacteristicTreeCtrl);

    CharacteristicTreeCtrl.$inject = ['$uibModal', 'CharacteristicService', 'MessageService', 'AppConstantsService'];

    function CharacteristicTreeCtrl($uibModal, CharacteristicService, MessageService, AppConstantsService) {
        var vm = this;

        vm.tree = [];
        vm.state = {
            currentNode: null
        };

        vm.createCharacteristic = createCharacteristic;
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
            vm.tree = CharacteristicService.getTree()
                .then(function (response) {
                    vm.tree = response.data;
                    vm.loaded = true;
                })
                .catch(_errorHandler);
        }

        function createCharacteristic() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/CreateEditCharacteristic',
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
                    _createCharacteristicNode(parentNodeId, node);
                })
                .catch(_errorHandler);
        }

        function createFolder() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/CreateEditFolder',
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
                    _createCharacteristicNode(parentNodeId, node);
                })
                .catch(_errorHandler);
        }

        function _createCharacteristicNode(parentNodeId, node) {
            CharacteristicService.create(parentNodeId, node)
                .then(function (data) {
                    loadData();
                })
                .catch(_errorHandler);
        }

        function edit() {
            var modalInstance = $uibModal.open({
                templateUrl: vm.state.currentNode.type === vm.fsNodeTypes.folder
                    ? '/Characteristic/CreateEditFolder'
                    : '/Characteristic/CreateEditCharacteristic',
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
                    _editCharacteristicNode(model.node);
                })
                .catch(_errorHandler);
        }

        function _editCharacteristicNode(node) {
            CharacteristicService.update(node)
                .then(function (data) {
                    loadData();
                })
                .catch(_errorHandler);
        }

        function remove() {
            MessageService.showMessageYesNo('removeDictionaryQuestion', 'removeDictionary')
                .then(function (result) {
                    if (result === 'OK') {
                        CharacteristicService.delete(vm.state.currentNode.id)
                            .then(function (data) {
                                loadData();
                            })
                            .catch(_errorHandler);
                    }
                });
        }

        function _errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
            vm.loaded = true;
        }

        function nodeSelectedCallback(event, data) {
            vm.state.currentNode = data;
        }

        function nodeUnselectedCallback(event, data) {
            vm.state.currentNode = null;
        }
    }

})();
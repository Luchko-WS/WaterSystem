(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('AliasesCtrl', AliasesCtrl);

    AliasesCtrl.$inject = ['$uibModal', 'common', 'AliasesService', 'CharacteristicService', 'HierarchyObjectService', 'MessageService'];

    function AliasesCtrl($uibModal, common, AliasesService, CharacteristicService, HierarchyObjectService, MessageService) {
        var _service = null;

        var vm = this;
        vm.entity = {};
        vm.create = create;
        vm.toggleEditForm = toggleEditForm;
        vm.edit = edit;
        vm.remove = remove;

        init();

        function init() {
            vm.entity.id = common.id;

            switch (common.entityType) {
                case "characteristic":
                    loadCharacteristic(vm.entity.id);
                    loadCharacteristicAliasesService();
                    break;
                case "object":
                    loadObject(vm.entity.id);
                    loadObjectAliasesService();
                    break;
                default:
                    throw "Invalid entity type " + common.entityType;
            }

            getAliases(vm.entity.id);
        }

        function loadCharacteristic(id) {
            vm.entityLoaded = false;
            CharacteristicService.get(id)
                .success(function (data) {
                    vm.entity.name = data.name;
                    vm.entityLoaded = true;
                })
                .error(function (error) {
                    vm.entityLoaded = true;
                    _errorHandler(error);
                });
        }

        function loadCharacteristicAliasesService() {
            _service = AliasesService.CharacteristicAliasesService;
        }

        function loadObject(id) {
            HierarchyObjectService.get(id)
                .success(function (data) {
                    vm.entity.name = data.name;
                    vm.entityLoaded = true;
                })
                .error(function (error) {
                    vm.entityLoaded = true;
                    _errorHandler(error);
                });
        }

        function loadObjectAliasesService() {
            _service = AliasesService.HierarchyObjectAliasesService;
        }

        function getAliases(id) {
            vm.aliasesLoaded = false;
            _service.getAllForEntity(id)
                .success(function (data) {
                    vm.aliases = data;
                    vm.aliasesLoaded = true;
                })
                .error(function (error) {
                    vm.aliasesLoaded = true;
                    _errorHandler(error);
                });
        }

        function create(value) {
            var alias = {
                value: value
            };
            _service.create(vm.entity.id, alias)
                .success(function (data) {
                    getAliases(vm.entity.id);
                })
                .error(_errorHandler);
        }

        function toggleEditForm(alias) {
            alias.editUIMode = !alias.editUIMode;
            if (alias.editUIMode) {
                alias.oldValue = alias.value;
            }
            else {
                alias.value = alias.oldValue;
            }
        }

        function edit(alias) {
            _service.update(alias)
                .success(function (data) {
                    getAliases(vm.entity.id);
                })
                .error(_errorHandler);
        }

        function remove(id) {
            MessageService.showMessageYesNo('removeAliasQuestion', 'removeAlias')
                .then(function (result) {
                    if (result === 'OK') {
                        _service.delete(id)
                            .success(function (data) {
                                getAliases(vm.entity.id);
                            })
                            .error(_errorHandler);
                    }
                });
        }

        function _errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
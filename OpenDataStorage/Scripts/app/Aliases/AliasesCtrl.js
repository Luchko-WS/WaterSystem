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
                    vm.aliasesLoaded = false;
                    _errorHandler(error);
                });
        }

        function _errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
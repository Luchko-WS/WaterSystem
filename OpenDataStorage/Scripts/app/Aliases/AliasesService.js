(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('AliasesService', AliasesService);

    AliasesService.$inject = ['$http'];

    function AliasesService($http) {

        var characteristicServiceName = 'CharacteristicAliases';
        var characteristicAliasesService = createAliasesService(characteristicServiceName);
        characteristicAliasesService.getAllForEntity = function (id) {
            return $http.get('/api/' + characteristicServiceName + '/GetAllForCharacteristic/' + id);
        };

        var hierarchyObjectAliasesServiceName = 'HierarchyObjectAliases';
        var hierarchyObjectAliasesService = createAliasesService(hierarchyObjectAliasesServiceName);
        hierarchyObjectAliasesService.getAllForEntity = function (id) {
            return $http.get('/api/' + hierarchyObjectAliasesServiceName + '/GetAllForObject/' + id);
        };

        function createAliasesService(serviceName) {
            
            function _get(id) {
                return $http.get('/api/' + serviceName + '/Get/' + id);
            }

            function _create(entityId, vm) {
                return $http({
                    method: 'POST',
                    url: '/api/' + serviceName + '/Create/' + entityId,
                    data: vm
                });
            } 

            function _update(vm) {
                return $http({
                    method: 'PUT',
                    url: '/api/' + serviceName + '/Update/',
                    data: vm
                });
            }

            function _delete(id) {
                return $http({
                    method: 'DELETE',
                    url: '/api/' + serviceName + '/Delete/ ' + id
                });
            }

            return {
                get: _get,
                create: _create,
                update: _update,
                delete: _delete
            };
        }

        return {
            CharacteristicAliasesService: characteristicAliasesService,
            HierarchyObjectAliasesService: hierarchyObjectAliasesService
        };
    }

})();
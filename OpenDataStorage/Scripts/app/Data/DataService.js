(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('DataService', DataService);

    DataService.$inject = ['$http'];

    function DataService($http) {
        var _service = {
            getDataForObject: _getDataForObject,
            NumberService: {
                create: _createNumberValue,
                update: _updateNumberValue
            },
            StringService: {
                create: _createStringValue,
                update: _updateStringValue
            },
            get: _get,
            delete: _delete
        };
        return _service;

        function _getDataForObject(id) {
            return $http.get('/api/Data/GetDataForObject/' + id);
        }

        function _get(id) {
            return $http.get('/api/Data/Get/' + id);
        }

        function _delete(id) {
            return $http({
                method: 'DELETE',
                url: '/api/Data/Delete/' + id
            });
        }

        function _createNumberValue(vm, objectId, characteristicId) {
            return $http({
                method: 'POST',
                url: '/api/Data/Number/' + objectId + '/' + characteristicId,
                data: vm
            });
        } 

        function _updateNumberValue(vm) {
            return $http({
                method: 'PUT',
                url: '/api/Data/Number/',
                data: vm
            });
        }

        function _createStringValue(vm, objectId, characteristicId) {
            return $http({
                method: 'POST',
                url: '/api/Data/String/' + objectId + '/' + characteristicId,
                data: vm
            });
        }

        function _updateStringValue(vm) {
            return $http({
                method: 'PUT',
                url: '/api/Data/Number/',
                data: vm
            });
        }
    }

})();
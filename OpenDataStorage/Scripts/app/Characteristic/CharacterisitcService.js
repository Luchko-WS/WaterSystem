(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('CharacteristicService', CharacteristicService);

    CharacteristicService.$inject = ['$http'];

    function CharacteristicService($http) {
        var _service = {
            getTree: _getTree,
            getSubTree: _getSubTree,
            get: _get,
            create: _create,
            update: _update,
            delete: _delete
        };
        return _service;

        function _getTree() {
            return $http.get('/api/Characteristics/GetTree');
        }

        function _getSubTree(vm) {
            return $http.get('/api/Characteristics/GetSubTree', {
                params: vm
            });
        }

        function _get(id) {
            return $http.get('/api/Characteristics/Get/' + id);
        }

        function _create(vm) {
            return $http({
                method: 'POST',
                url: '/api/Characteristics/Create',
                data: vm
            });
        }

        function _update(vm) {
            return $http({
                method: 'PUT',
                url: '/api/Characteristics/Update',
                data: vm
            });
        }

        function _delete(id) {
            return $http({
                method: 'DELETE',
                url: '/api/Characteristics/Delete/' + id
            });
        }
    }

})();
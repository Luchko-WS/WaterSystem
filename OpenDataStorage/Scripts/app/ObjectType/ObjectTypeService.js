(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('ObjectTypeService', ObjectTypeService);

    ObjectTypeService.$inject = ['$http'];

    function ObjectTypeService($http) {
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
            return $http.get('/api/ObjectType/GetTree');
        }

        function _getSubTree(vm) {
            return $http.get('/api/ObjectType/GetSubTree', {
                params: vm
            });
        }

        function _get(id) {
            return $http.get('/api/ObjectType/Get/' + id);
        }

        function _create(parentId, vm) {
            return $http({
                method: 'POST',
                url: '/api/ObjectType/Create/' + parentId,
                data: vm
            });
        }

        function _update(vm) {
            return $http({
                method: 'PUT',
                url: '/api/ObjectType/Update',
                data: vm
            });
        }

        function _delete(id) {
            return $http({
                method: 'DELETE',
                url: '/api/ObjectType/Delete/' + id
            });
        }
    }

})();
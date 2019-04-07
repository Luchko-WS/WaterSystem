(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('HierarchyObjectService', HierarchyObjectService);

    HierarchyObjectService.$inject = ['$http'];

    function HierarchyObjectService($http) {
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
            return $http.get('/api/HierarchyObjects/GetTree');
        }

        function _getSubTree(vm) {
            return $http.get('/api/HierarchyObjects/GetSubTree', {
                params: vm
            });
        }

        function _get(id) {
            return $http.get('/api/HierarchyObjects/Get/' + id);
        }

        function _create(vm) {
            return $http({
                method: 'POST',
                url: '/api/HierarchyObjects/Create',
                data: vm
            });
        }

        function _update(vm) {
            return $http({
                method: 'PUT',
                url: '/api/HierarchyObjects/Update',
                data: vm
            });
        }

        function _delete(id) {
            return $http({
                method: 'DELETE',
                url: '/api/HierarchyObjects/Delete/' + id
            });
        }
    }

})();
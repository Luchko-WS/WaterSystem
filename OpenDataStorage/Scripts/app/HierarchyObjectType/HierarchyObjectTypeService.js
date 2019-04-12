﻿(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('HierarchyObjectTypeService', HierarchyObjectTypeService);

    HierarchyObjectTypeService.$inject = ['$http'];

    function HierarchyObjectTypeService($http) {
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
            return $http.get('/api/HierarchyObjectTypeService/GetTree');
        }

        function _getSubTree(vm) {
            return $http.get('/api/HierarchyObjectTypeService/GetSubTree', {
                params: vm
            });
        }

        function _get(id) {
            return $http.get('/api/HierarchyObjectTypeService/Get/' + id);
        }

        function _create(parentId, vm) {
            return $http({
                method: 'POST',
                url: '/api/HierarchyObjectTypeService/Create/' + parentId,
                data: vm
            });
        }

        function _update(vm) {
            return $http({
                method: 'PUT',
                url: '/api/HierarchyObjectTypeService/Update',
                data: vm
            });
        }

        function _delete(id) {
            return $http({
                method: 'DELETE',
                url: '/api/HierarchyObjectTypeService/Delete/' + id
            });
        }
    }

})();
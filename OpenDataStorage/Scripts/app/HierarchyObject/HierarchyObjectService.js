﻿(function () {
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
            move: _move,
            delete: _delete
        };
        return _service;

        function _getTree(vm) {
            return $http.get('/api/HierarchyObjects/GetTree', {
                params: vm
            });
        }

        function _getSubTree(vm) {
            return $http.get('/api/HierarchyObjects/GetSubTree', {
                params: vm
            });
        }

        function _get(id) {
            return $http.get('/api/HierarchyObjects/Get/' + id);
        }

        function _create(parentId, vm) {
            return $http({
                method: 'POST',
                url: '/api/HierarchyObjects/Create/' + parentId,
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

        function _move(id, parentId) {
            return $http({
                method: 'POST',
                url: '/api/HierarchyObjects/Move/' + id + '/' + parentId
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
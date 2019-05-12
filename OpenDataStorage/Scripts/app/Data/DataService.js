(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('DataService', DataService);

    DataService.$inject = ['$http'];

    function DataService($http) {
        var _service = {
            getDataForObject: _getDataForObject,
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
    }

})();
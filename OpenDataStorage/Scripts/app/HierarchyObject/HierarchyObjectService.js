(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('HierarchyObjectService', HierarchyObjectService);

    HierarchyObjectService.$inject = ['$http'];

    function HierarchyObjectService($http) {
        var service = {
            
        };

        return service;
    }

})();
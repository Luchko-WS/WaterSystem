(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('ServiceSyncService', ServiceSyncService);

    ServiceSyncService.$inject = ['$http'];

    function ServiceSyncService($http) {
        var _service = {
            sync: _sync
        };
        return _service;

        function _sync() {
            return $http({
                method: 'POST',
                url: '/api/SystemManagement/SyncWithTextyOrgUaWaterService'
            });
        }
    }

})();
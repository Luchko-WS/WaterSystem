(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('DataSyncService', DataSyncService);

    DataSyncService.$inject = ['$http'];

    function DataSyncService($http) {
        var _service = {
            sync: _sync,
            uploadSacmigDataFile: _uploadSacmigDataFile
        };
        return _service;

        function _sync() {
            return $http({
                method: 'POST',
                url: '/api/DataSynch/SyncWithTextyOrgUaWaterService'
            });
        }

        function _uploadSacmigDataFile(objectId, file) {
            var formData = new FormData();
            formData.append('file', file);

            return $http.post('/api/DataSynch/UploadSacmigDataFile/' + objectId, formData, {
                //transformRequest: angular.identity,
                headers: { 'Content-Type': undefined }
            });
        }
    }
})();
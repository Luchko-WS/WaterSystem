(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('ReportsService', ReportsService);

    ReportsService.$inject = ['$http'];

    function ReportsService($http) {
        var _service = {
            generateReport: _generateReport
        };
        return _service;

        function _generateReport(objectId, characteristicId, typeId, fromDate, toDate) {
            return $http.get('/api/Reports/Generate/', {
                params: {
                    objectId: objectId,
                    characterisitcId: characteristicId,
                    typeId: typeId,
                    fromDate: fromDate,
                    toDate: toDate
                },
                responseType: 'arraybuffer'
            });
        }
    }

})();
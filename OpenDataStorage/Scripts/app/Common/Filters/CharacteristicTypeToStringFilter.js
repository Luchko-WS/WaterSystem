(function () {
    'use strict';

    //service
    angular
        .module('MainApp')
        .filter('characteristicTypeToString', function () {
            return function (code) {
                switch (code) {
                    case 1: return 'number';
                    case 2: return 'string';
                    default: return code;
                }
            };
        });
})();
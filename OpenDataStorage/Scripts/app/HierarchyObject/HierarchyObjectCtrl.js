(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('HierarchyObjectCtrl', HierarchyObjectCtrl);

    HierarchyObjectCtrl.$inject = ['HierarchyObjectService', 'MessageService'];

    function HierarchyObjectCtrl(DictionariesService, MessageService) {
        var vm = this;

        vm.init = init;

        function init() {

        }

        function errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }
})();
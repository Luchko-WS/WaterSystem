(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('HierarchyObjectTreeCtrl', HierarchyObjectTreeCtrl);

    HierarchyObjectTreeCtrl.$inject = ['$uibModal', 'HierarchyObjectService', 'MessageService'];

    function HierarchyObjectTreeCtrl($uibModal, HierarchyObjectService, MessageService) {
        var vm = this;
        
        init();

        function init() {
        
        }

        function errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
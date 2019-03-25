(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('EditHierarchyObjectCtrl', EditHierarchyObjectCtrl);

    EditHierarchyObjectCtrl.$inject = ['$uibModalInstance', 'HierarchyObjectService', 'MessageService'];

    function EditHierarchyObjectCtrl($uibModalInstance, HierarchyObjectService, MessageService) {
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
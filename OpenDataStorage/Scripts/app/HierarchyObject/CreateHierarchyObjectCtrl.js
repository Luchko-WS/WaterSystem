(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CreateHierarchyObjectCtrl', CreateHierarchyObjectCtrl);

    CreateHierarchyObjectCtrl.$inject = ['$uibModalInstance', 'HierarchyObjectService', 'MessageService'];

    function CreateHierarchyObjectCtrl($uibModalInstance, HierarchyObjectService, MessageService) {
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
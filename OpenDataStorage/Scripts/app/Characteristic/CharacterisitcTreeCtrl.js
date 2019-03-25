(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CharacteristicTreeCtrl', CharacteristicTreeCtrl);

    CharacteristicTreeCtrl.$inject = ['$uibModal', 'CharacteristicService', 'MessageService'];

    function CharacteristicTreeCtrl($uibModal, CharacteristicService, MessageService) {
        var vm = this;
        init();

        function init() {
            vm.loaded = false;
            CharacteristicService.getCharacteristicTree()
                .success(function (data) {
                    vm.tree = data;
                    vm.loaded = true;
                })
                .error(function (error) {
                    errorHandler(error);
                    vm.loaded = true;
                });
        }

        function errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
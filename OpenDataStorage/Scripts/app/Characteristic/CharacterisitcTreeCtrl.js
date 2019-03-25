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
                .then(function (response) {
                    vm.tree = response.data;
                    console.dir(vm.tree);
                    vm.loaded = true;
                })
                .catch(function (error) {
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
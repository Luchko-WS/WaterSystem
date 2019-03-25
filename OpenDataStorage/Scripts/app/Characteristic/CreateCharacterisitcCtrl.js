﻿(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CreateCharacteristicCtrl', CreateCharacteristicCtrl);

    CreateCharacteristicCtrl.$inject = ['$uibModalInstance', 'CharacteristicService', 'MessageService'];

    function CreateCharacteristicCtrl($uibModalInstance, CharacteristicService, MessageService) {
        var vm = this;

        vm.characteristic = {};
        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); };
        vm.createCharacteristic = createCharacteristic;

        init();

        function init() {
            
        }

        function createCharacteristic() {
            CharacteristicService.createDictionary(vm.characteristic)
                .success(function (data) {
                    $uibModalInstance.close(data);
                })
                .error(errorHandler);
        }

        function errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
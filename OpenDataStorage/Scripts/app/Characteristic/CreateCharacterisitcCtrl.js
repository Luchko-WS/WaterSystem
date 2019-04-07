(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CreateCharacteristicCtrl', CreateCharacteristicCtrl);

    CreateCharacteristicCtrl.$inject = ['$uibModalInstance', 'CharacteristicService', 'MessageService', '_model'];

    function CreateCharacteristicCtrl($uibModalInstance, CharacteristicService, MessageService, _model) {
        var vm = this;

        vm.characteristic = {};
        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); };
        vm.createCharacteristic = createCharacteristic;

        init();

        function init() {
            console.dir(_model);
        }

        function createCharacteristic() {
            CharacteristicService.create(_model.parentNode.id, vm.characteristic)
                .then(function (data) {
                    $uibModalInstance.close(data);
                }, errorHandler);
        }

        function errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
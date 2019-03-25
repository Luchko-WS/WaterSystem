(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('EditCharacterisitcCtrl', EditCharacterisitcCtrl);

    EditCharacterisitcCtrl.$inject = ['$uibModalInstance', 'CharacterisitcService', '_model', 'MessageService'];

    function EditCharacterisitcCtrl($uibModalInstance, CharacterisitcService, _model, MessageService) {
        var vm = this;

        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); };

        vm.editDictionary = editDictionary;

        init();

        function init() {
            vm.characterisitc = model.characterisitc;
        }

        function editDictionary() {
            CharacterisitcService.editCharacterisitc(vm.characterisitc.id, vm.characterisitc)
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
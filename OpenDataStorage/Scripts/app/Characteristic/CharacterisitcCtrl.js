(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CharacteristicCtrl', CharacteristicCtrl);

    CharacteristicCtrl.$inject = ['CharacteristicService', 'MessageService'];

    function CharacteristicCtrl(CharacteristicService, MessageService) {
        var vm = this;

        vm.init = init;
        vm.editCharacteristic = editCharacteristic;
        vm.removeCharacteristic = removeCharacteristic;

        function init(characteristicId) {
            vm.loaded = false;

            CharacteristicService.getCharacteristic(characteristicId)
                .success(function (data) {
                    vm.characteristic = data;
                    vm.loaded = true;
                })
                .error(function (error) {
                    errorHandler(error);
                    vm.loaded = true;
                });
        }

        function editCharacteristic() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/EditCharacteristic',
                controller: 'EditCharacteristicCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        characteristic: vm.characteristic
                    }
                }
            });

            modalInstance.result.then(function (response) {
                init(response.id);
            });
        }

        function removeCharacteristic() {
            MessageService.showMessageYesNo('removeDictionaryQuestion', 'removeDictionary')
                .then(function (result) {
                    if (result === 'OK') {
                        CharacteristicService.removeCharacteristic(vm.characteristic.id)
                            .success(function (data) {
                                console.log('remove');
                            })
                            .error(errorHerrorHandlerandling(error));
                    }
                });
        }

        function errorHandler(error) {
            console.error(error);
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }
})();
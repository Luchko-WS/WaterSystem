(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CharacteristicTreeCtrl', CharacteristicTreeCtrl);

    CharacteristicTreeCtrl.$inject = ['$uibModal', 'CharacteristicService', 'MessageService'];

    function CharacteristicTreeCtrl($uibModal, CharacteristicService, MessageService) {
        var vm = this;
        vm.createCharacteristic = createCharacteristic;
        vm.editCharacteristic = editCharacteristic;
        vm.removeCharacteristic = removeCharacteristic;

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

        function createCharacteristic() {

            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/CreateCharacteristic',
                controller: 'CreateCharacteristicCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {

                    }
                }
            });

            modalInstance.result.then(function (response) {
                console.log(response);
            }, function () { });
        }

        function editCharacteristic(characteristic) {
            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/EditCharacteristic',
                controller: 'EditCharacteristicCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        characteristic: characteristic
                    }
                }
            });

            modalInstance.result.then(function (response) {
                init(response.id);
            }, function () { });
        }

        function removeCharacteristic(characteristic) {
            MessageService.showMessageYesNo('removeDictionaryQuestion', 'removeDictionary')
                .then(function (result) {
                    if (result === 'OK') {
                        CharacteristicService.removeCharacteristic(characteristic.id)
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
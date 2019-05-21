(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ObjectDataCtrl', ObjectDataCtrl);

    ObjectDataCtrl.$inject = ['$uibModal', 'common', 'DataService', 'CharacteristicService', 'MessageService', 'AppConstantsService', 'requestInfo'];

    function ObjectDataCtrl($uibModal, common, DataService, CharacteristicService, MessageService, AppConstantsService, requestInfo) {
        var vm = this;

        vm.create = create;
        vm.edit = update;
        vm.remove = remove;

        init();

        function init() {
            if (requestInfo.isAuthenticated === 'True') vm.enableEditing = true;
            vm.objectId = common.id;
            loadAllData();
        }

        function loadAllData() {
            vm.loaded = false;
            DataService.getDataForObject(vm.objectId)
                .success(function (data) {
                    vm.data = data;
                    vm.loaded = true;
                })
                .error(function (error) {
                    vm.loaded = true;
                    _errorHandler(error);
                });
        }

        function create() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Data/SelectCharacteristic',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'create',
                        init: {
                            initPromise: CharacteristicService.getTree,
                            initSuccessCallback: _selectCharacteristicModalwindowInitSuccessCallback
                        }
                    }
                }
            });

            function _selectCharacteristicModalwindowInitSuccessCallback(data, model) {
                return {
                    treeElement: {
                        tree: data,
                        treeParserConfig: AppConstantsService.getOnlySelectableFileFsTreeConfig(),
                        callbacks: {
                            nodeSelectedCallback: function (event, data) {
                                model.node = data;
                            },
                            nodeUnselectedCallback: function (event, data) {
                                model.node = null;
                            }
                        }
                    }
                };
            }

            modalInstance.result
                .then(function (model) {
                    var characteristic = model.node;
                    var value = _getValueByCaracteristic(characteristic);
                    _createValue(value, characteristic.id, vm.objectId);
                });

            function _createValue(value, characteristicId, objectId) {
                var modalInstance = $uibModal.open({
                    templateUrl: _getTemplateByValueType(value),
                    controller: 'CreateEditModelCtrl',
                    controllerAs: 'vm',
                    resolve: {
                        _model: {
                            mode: 'create',
                            value: value
                        }
                    }
                });

                modalInstance.result
                    .then(function (model) {
                        console.log(model.value);
                        _getServiceForValue(model.value).create(model.value, objectId, characteristicId)
                            .success(function (data) {
                                loadAllData();
                            })
                            .error(_errorHandler);
                    });
            }
        }

        function update(value) {
            console.log(value);
            value.creationDate = Date(value.creationDate);
            var modalInstance = $uibModal.open({
                templateUrl: _getTemplateByValueType(value),
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        mode: 'edit',
                        value: value
                    }
                }
            });

            modalInstance.result
                .then(function (model) {
                    console.log(model.value);
                    _getServiceForValue(model.value).update(mode.value)
                        .success(function (data) {
                            loadAllData();
                        })
                        .error(_errorHandler);
                });
        }

        function remove(value) {
            MessageService.showMessageYesNo('removeValueQuestion', 'removeValue')
                .then(function (result) {
                    if (result === 'OK') {
                        DataService.delete(value.id)
                            .success(function (data) {
                                loadAllData();
                            })
                            .error(_errorHandler);
                    }
                });
        }

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }

        function _getValueByCaracteristic(characteristic) {
            return {
                valueType: characteristic.characteristicType
            };
        }

        function _getTemplateByValueType(value) {
            switch (value.valueType) {
                case 1:
                    return '/Data/CreateEditNumber';
                case 2:
                    return '/Data/CreateEditString';
            }
            return;
        }

        function _getServiceForValue(value) {
            switch (value.valueType) {
                case 1:
                    return DataService.NumberService;
                case 2:
                    return DataService.StringService;
            }
            return;
        }
    }

})();
(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ReportCtrl', ReportCtrl);

    ReportCtrl.$inject = ['ReportsService', '$uibModal', 'MessageService', 'AppConstantsService', 'HierarchyObjectService', 'CharacteristicService', 'ObjectTypeService'];

    function ReportCtrl(ReportsService, $uibModal, MessageService, AppConstantsService, HierarchyObjectService, CharacteristicService, ObjectTypeService) {
        var vm = this;

        vm.objectData = null;
        vm.characteristicData = null;
        vm.typeData = null;
        vm.fromDate = null;
        vm.toDate = null;

        vm.loaded = true;

        vm.getObjectData = _getObjectData;
        vm.getCharacteristicData = _getCharacteristicData;
        vm.getTypeData = _getTypeData;
        vm.generate = _generate;

        init();

        function init() {
            //vm.toDate = Date.now();
        }

        function _getObjectData() {
            var modalInstance = $uibModal.open({
                templateUrl: '/HierarchyObject/SelectObject',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        init: {
                            initPromise: HierarchyObjectService.getTree,
                            initSuccessCallback: _selectObjectModalwindowInitSuccessCallback
                        }
                    }
                }
            });

            function _selectObjectModalwindowInitSuccessCallback(data, model) {
                return {
                    treeElement: {
                        tree: data,
                        treeParserConfig: AppConstantsService.getSelectableDefaultTreeConfig(),
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
                    vm.objectData = model.node;
                });
        }

        function _getCharacteristicData() {
            var modalInstance = $uibModal.open({
                templateUrl: '/Characteristic/SelectCharacteristic',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
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
                        treeParserConfig: AppConstantsService.getSelectableFsTreeConfig(),
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
                    vm.characteristicData = model.node;
                });
        }

        function _getTypeData() {
            var modalInstance = $uibModal.open({
                templateUrl: '/ObjectType/SelectType',
                controller: 'CreateEditModelCtrl',
                controllerAs: 'vm',
                resolve: {
                    _model: {
                        init: {
                            initPromise: ObjectTypeService.getTree,
                            initSuccessCallback: _selectObjectTypeModalwindowInitSuccessCallback
                        }
                    }
                }
            });

            function _selectObjectTypeModalwindowInitSuccessCallback(data, model) {
                return {
                    treeElement: {
                        tree: data,
                        treeParserConfig: AppConstantsService.getSelectableFsTreeConfig(),
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
                    vm.typeData = model.node;
                });
        }

        function _generate() {
            vm.loaded = false;
            ReportsService.generateReport(
                vm.objectData ? vm.objectData.id : null,
                vm.characteristicData ? vm.characteristicData.id : null,
                vm.typeData ? vm.typeData.id : null,
                vm.fromDate,
                vm.toDate
            )
                .success(function (data) {
                    console.log(data);
                    vm.loaded = true;
                })
                .error(_errorHandler);
        }

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
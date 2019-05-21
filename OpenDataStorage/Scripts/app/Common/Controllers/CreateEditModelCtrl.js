(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CreateEditModelCtrl', CreateEditModelCtrl);

    CreateEditModelCtrl.$inject = ['MessageService', '$uibModalInstance', '_model'];

    function CreateEditModelCtrl(MessageService, $uibModalInstance, _model) {
        var vm = this;

        vm.model = {};
        vm.save = function () { $uibModalInstance.close(vm.model); };
        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); };

        function init() {
            vm.model = _model ? angular.copy(_model) : {};
            if (!vm.model.node) vm.model.node = {};

            if (_model.init && _model.init.initPromise) {
                vm.model.loaded = false;
                _model.init.initPromise()
                    .success(function (data) {
                        vm.model.node = 0;
                        if (typeof (_model.init.initSuccessCallback) === 'function') {
                            vm.model = Object.assign(vm.model, _model.init.initSuccessCallback(data, vm.model));
                        }
                        vm.model.loaded = true;
                    })
                    .error(function (error) {
                        if (typeof (_model.init.initErrorCallback) === 'function') {
                            vm.model = Object.assign(vm.model, _model.init.initErrorCallback(error, vm.model));
                        }
                        vm.model.loaded = true;
                        MessageService.showMessage('commonErrorMessage', 'error');
                        console.error(error);
                    });
            }
        }

        init();
    }

})();
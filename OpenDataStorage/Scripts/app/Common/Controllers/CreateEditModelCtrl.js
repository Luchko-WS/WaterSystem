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
            vm.model.parentNode = Object.assign({}, _model.parentNode);
            vm.model.node = Object.assign({}, _model.node);
            vm.model.mode = _model.mode;

            if (_model.init && _model.init.initPromise) {
                _model.init.initPromise()
                    .success(function (data) {
                        if (typeof (_model.init.initSuccessCallback) === 'function') {
                            vm.model = Object.assign(vm.model, _model.init.initSuccessCallback(data, vm.model.node));
                        }
                    })
                    .error(function (error) {
                        if (typeof (_model.init.initErrorCallback) === 'function') {
                            vm.model = Object.assign(vm.model, _model.init.initErrorCallback(error, vm.model.node));
                            _errorHandler(error);
                        }
                    });
            }
        }

        init();

        function _errorHandler(error) {
            console.error(error);
            vm.loaded = true;
            MessageService.showMessage('commonErrorMessage', 'error');
        }
    }

})();
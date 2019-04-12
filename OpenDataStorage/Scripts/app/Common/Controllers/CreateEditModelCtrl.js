(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('CreateEditModelCtrl', CreateEditModelCtrl);

    CreateEditModelCtrl.$inject = ['$uibModalInstance', '_model'];

    function CreateEditModelCtrl($uibModalInstance, _model) {
        var vm = this;

        vm.model = {};
        vm.save = function () { $uibModalInstance.close(vm.model); };
        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); };

        function init() {
            vm.model.parentNode = _model.parentNode ? Object.assign({}, _model.parentNode) : null;
            vm.model.node = _model.node ? Object.assign({}, _model.node) : null;
        }

        init();
    }

})();
(function () {
    'use strict';

    angular
        .module('MainApp')
        .controller('ReadModelCtrl', ReadModelCtrl);

    ReadModelCtrl.$inject = ['$uibModalInstance', '_model'];

    function ReadModelCtrl($uibModalInstance, _model) {
        var vm = this;

        vm.model = {};
        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); };

        function init() {
            vm.model = _model ? Object.assign({}, _model) : {};
        }

        init();
    }

})();
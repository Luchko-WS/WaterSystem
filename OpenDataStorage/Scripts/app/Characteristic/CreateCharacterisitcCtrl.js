(function () {
    'use strict';

    angular
        .module('OnlineDictionary')
        .controller('CreateDictionaryCtrl', CreateDictionaryCtrl);

    CreateDictionaryCtrl.$inject = ['$uibModalInstance', 'DictionariesService', 'MessageService'];

    function CreateDictionaryCtrl($uibModalInstance, DictionariesService, MessageService) {
        var vm = this;

        vm.dictionary = {};
        vm.cancel = function () { $uibModalInstance.dismiss('cancel'); }
        vm.createDictionary = createDictionary;

        init();

        function init() {
            console.log('create dictionary ctrl init');
        }

        function createDictionary() {
            DictionariesService.createDictionary(vm.dictionary)
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
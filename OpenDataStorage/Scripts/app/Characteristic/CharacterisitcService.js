(function () {
    'use strict';

    angular
        .module('MainApp')
        .factory('CharacteristicService', CharacteristicService);

    CharacteristicService.$inject = ['$http'];

    function CharacteristicService($http) {
        var service = {
            getCharacteristicTree: getCharacteristicTree,
            getCharacteristicSubTree: getCharacteristicSubTree,
            getCharacteristic: getCharacteristic,
            createCharacteristic: createCharacteristic,
            editCharacteristic: editCharacteristic,
            removeCharacteristic: removeCharacteristic
        };

        return service;

        function getCharacteristicTree() {
            return $http.get('/api/Characteristic/GetCharacteristicTree', {
                /*params: {
                    Name: filter.name,
                    SourceLanguage: filter.sourceLanguage,
                    TargetLanguage: filter.targetLanguage,
                    OwnerId: filter.ownerId
                }*/
            });
        }

        function getCharacteristicSubTree() {
            return $http.get('/api/Characteristic/GetCharacteristicSubTree', {
                /*params: {
                    Name: filter.name,
                    SourceLanguage: filter.sourceLanguage,
                    TargetLanguage: filter.targetLanguage,
                    OwnerId: filter.ownerId
                }*/
            });
        }

        function getCharacteristic(characteristicId) {
            return $http.get('/api/Characteristic/Characteristic/' + characteristicId, {
                /*params: {
                    SourceLanguageValue: phrasesPairsFilter.sourceLanguageValue,
                    TargetLanguageValue: phrasesPairsFilter.targetLanguageValue
                }*/
            });
        }

        function createCharacteristic(characteristic) {
            return $http({
                method: 'POST',
                url: '/api/Characteristic/Create',
                data: characteristic
            });
        }

        function editCharacteristic(characteristicId, characteristicData) {
            return $http({
                method: 'PUT',
                url: '/api/Characteristic/Edit/' + characteristicId,
                data: characteristicData
            });
        }

        function removeCharacteristic(characteristicId) {
            return $http({
                method: 'DELETE',
                url: '/api/Characteristic/Remove/' + characteristicId
            });
        }
    }

})();
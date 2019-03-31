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
            createCharacteristicFolder: createCharacteristicFolder,
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
                url: '/api/Characteristic/CreateCharacteristic',
                data: characteristic
            });
        }

        function createCharacteristicFolder(folder) {
            return $http({
                method: 'POST',
                url: '/api/Characteristic/CreateFolder',
                data: folder
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
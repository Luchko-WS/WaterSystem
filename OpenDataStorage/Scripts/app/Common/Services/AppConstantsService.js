(function () {
    'use strict';

    //service
    angular
        .module('MainApp')
        .factory('AppConstantsService', AppConstantsService);

    function AppConstantsService() {
        const TEXT_FIELD_NAME = "name";
        const LEVEL_FIELD_NAME = "level";
        const NODE_TYPE_FIELD_NAME = "type";
        const FOLDER_NODE_TYPE_VALUE = 0;
        const FILE_NODE_TYPE_VALUE = 1;
        const FOLDERS_AND_FILE_MODE = 1;
        const FOLDERS_MODE = 2;

        function getFSNodeTypes() {
            return {
                folder: FOLDER_NODE_TYPE_VALUE,
                file: FILE_NODE_TYPE_VALUE
            };
        }

        function getCharactersiticTreeConfig() {
            return {
                fieldsNames: {
                    textFieldName: TEXT_FIELD_NAME,
                    levelFieldName: LEVEL_FIELD_NAME
                },
                fsConfig: {
                    nodeTypeFieldName: NODE_TYPE_FIELD_NAME,
                    folderNodeTypeValue: FOLDER_NODE_TYPE_VALUE,
                    fileNodeTypeValue: FILE_NODE_TYPE_VALUE,
                    mode: FOLDERS_AND_FILE_MODE
                }
            };
        }

        return {
            getFSNodeTypes: getFSNodeTypes,
            getCharactersiticTreeConfig: getCharactersiticTreeConfig
        };
    }
})();
﻿(function () {
    'use strict';

    //service
    angular
        .module('MainApp')
        .factory('AppConstantsService', AppConstantsService);

    function AppConstantsService() {
        const ID_FILE_NAME = "id";
        const TEXT_FIELD_NAME = "name";
        const LEVEL_FIELD_NAME = "level";
        const NODE_TYPE_FIELD_NAME = "entityType";
        const SELECT_NODE_EVENT_NAME = "selectNodeEvent";
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

        function getDefaultFsTreeConfig() {
            return {
                fieldsNames: {
                    idFieldName: ID_FILE_NAME,
                    textFieldName: TEXT_FIELD_NAME,
                    levelFieldName: LEVEL_FIELD_NAME
                },
                treeConfig: {
                    nodesConfig: {
                        expandEachNode: false
                    },
                    draggable: true
                },
                fsConfig: {
                    nodeTypeFieldName: NODE_TYPE_FIELD_NAME,
                    folderNodeTypeValue: FOLDER_NODE_TYPE_VALUE,
                    fileNodeTypeValue: FILE_NODE_TYPE_VALUE,
                    mode: FOLDERS_AND_FILE_MODE,
                    selectOnlyFiles: false
                },
                definedValues: {
                    selectedNode: undefined
                }
            };
        }

        function getOnlySelectableFileFsTreeConfig() {
            return {
                fieldsNames: {
                    idFieldName: ID_FILE_NAME,
                    textFieldName: TEXT_FIELD_NAME,
                    levelFieldName: LEVEL_FIELD_NAME
                },
                treeConfig: {
                    nodesConfig: {
                        expandEachNode: false
                    },
                    draggable: false
                },
                fsConfig: {
                    nodeTypeFieldName: NODE_TYPE_FIELD_NAME,
                    folderNodeTypeValue: FOLDER_NODE_TYPE_VALUE,
                    fileNodeTypeValue: FILE_NODE_TYPE_VALUE,
                    mode: FOLDERS_AND_FILE_MODE,
                    selectOnlyFiles: true
                },
                definedValues: {
                    selectedNode: undefined
                }
            };
        }

        function getDefaultTreeConfig() {
            return {
                fieldsNames: {
                    idFieldName: ID_FILE_NAME,
                    textFieldName: TEXT_FIELD_NAME,
                    levelFieldName: LEVEL_FIELD_NAME
                },
                treeConfig: {
                    nodesConfig: {
                        expandEachNode: false
                    },
                    draggable: true
                },
                definedValues: {
                    selectedNode: undefined
                }
            };
        }

        return {
            getFSNodeTypes: getFSNodeTypes,
            getDefaultTreeConfig: getDefaultTreeConfig,
            getDefaultFsTreeConfig: getDefaultFsTreeConfig,
            getOnlySelectableFileFsTreeConfig: getOnlySelectableFileFsTreeConfig
        };
    }
})();
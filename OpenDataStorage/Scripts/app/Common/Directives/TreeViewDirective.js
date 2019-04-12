(function () {
    'use strict';

    angular
        .module('MainApp')
        .directive('treeView', function (MessageService) {
            return {
                replace: true,
                link: function (scope, element, attrs) {

                    const FOLDERS_AND_FILE_MODE = 1;
                    const FOLDERS_MODE = 2;

                    $('#tree').treeview({
                        data: parseArrayToTree(scope.array, scope.fieldsNames, scope.fsConfig)
                    });
                    $('#tree').on('nodeUnselected', function (event, data) {
                        if (scope.nodeUnselectedCallback) {
                            scope.$apply(function () {
                                scope.nodeUnselectedCallback(event, data);
                            });
                        }
                    });
                    $('#tree').on('nodeSelected', function (event, data) {
                        if (scope.nodeSelectedCallback) {
                            scope.$apply(function () {
                                scope.nodeSelectedCallback(event, data);
                            });
                        }
                    });

                    function parseArrayToTree(array, fieldsNames, fsConfig) {

                        try {
                            var stack = [0];
                            var tagsStack = [0];
                            var currentLevel = 0;

                            for (var i = 0; i < array.length; i++) {
                                var node = array[i];
                                node._node_id = i;
                                node.nodes = null;
                                node.text = node[fieldsNames.textFieldName];
                                node.tags = fieldsNames.tagFields || [];

                                //aply fsConfig
                                if (fsConfig) {
                                    if (node[fsConfig.nodeTypeFieldName] === fsConfig.folderNodeTypeValue) {
                                        node.icon = "glyphicon glyphicon-folder-open";
                                    }
                                    else if (node[fsConfig.nodeTypeFieldName] === fsConfig.fileNodeTypeValue) {
                                        node.icon = "glyphicon glyphicon-file";
                                    }
                                }

                                if (currentLevel == node[fieldsNames.levelFieldName]) {
                                    node._parentId = stack[0];
                                    tagsStack[0]++;
                                }
                                else if (currentLevel < node[fieldsNames.levelFieldName]) {
                                    stack.unshift(array[i - 1]._node_id);
                                    node._parentId = stack[0];
                                    currentLevel++;
                                }
                                else if (currentLevel > node[fieldsNames.levelFieldName]) {
                                    for (var j = 0; j < currentLevel - node[fieldsNames.levelFieldName]; j++) {
                                        stack.shift();
                                    }
                                    node._parentId = stack[0];
                                    currentLevel -= currentLevel - node[fieldsNames.levelFieldName];
                                }
                            }

                            var root = array[0], map = {};
                            for (var i = 1; i < array.length; i++) {
                                if (fsConfig && fsConfig.mode === FOLDERS_MODE
                                    && array[i][fsConfig.nodeTypeFieldName] == fsConfig.fileNodeTypeValue) {
                                    //skip files
                                    continue;
                                }
                                var node = array[i];
                                map[node._node_id] = i;
                                if (node._parentId !== 0) {
                                    var parent = array[map[node._parentId]];
                                    if (!parent.nodes) {
                                        parent.nodes = [];
                                    }
                                    parent.nodes.push(node);
                                } else {
                                    if (!root.nodes) {
                                        root.nodes = [];
                                    }
                                    root.nodes.push(node);
                                }
                            }
                            return [root];
                        }
                        catch (ex) {
                            errorHandler(ex);
                        }
                    }

                    function errorHandler(error) {
                        console.error(error);
                        MessageService.showMessage('error', 'error');
                    }
                },
                scope: {
                    array: '=ngModel',
                    fieldsNames: '=',
                    fsConfig: '=',
                    filterPairsPromise: '=',
                    nodeSelectedCallback: '=',
                    nodeUnselectedCallback: '='
                },
                restrict: 'AE',
                templateUrl: "/Templates/DirectivesTemplates/TreeView.html"
            };
        });
})();
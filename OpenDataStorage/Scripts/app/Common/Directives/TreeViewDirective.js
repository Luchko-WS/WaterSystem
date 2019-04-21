(function () {
    'use strict';

    angular
        .module('MainApp')
        .directive('treeView', function ($rootScope, MessageService) {
            return {
                replace: true,
                link: function (scope, element, attrs) {

                    const FOLDERS_AND_FILE_MODE = 1;
                    const FOLDERS_MODE = 2;
                    const EXPANDED_LEVEL = 2;

                    $('#tree').treeview({
                        data: scope.config.fsConfig
                            ? parseArrayToTree(scope.array, scope.config.fieldsNames, scope.config.nodesConfig, scope.config.fsConfig)
                            : parseArrayToTree(scope.array, scope.config.fieldsNames, scope.config.nodesConfig),
                        levels: EXPANDED_LEVEL
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

                    initSelect();

                    function initSelect() {
                        if (scope.config.definedValues && scope.config.definedValues.selectedNode) {
                            var id = scope.config.definedValues.selectedNode[scope.config.fieldsNames.idFieldName];
                            var node = findNodeFromTreeById(id);
                            $('#tree').treeview('revealNode', [node.nodeId, { silent: true }]);
                            $('#tree').treeview('selectNode', [node.nodeId, { silent: true }]);
                        }
                    }

                    function findNodeFromTreeById(id) {
                        return $('#tree').treeview('findNodeByCondition', [function (node) {
                            return node.id == id;
                        }]);
                    }

                    function parseArrayToTree(array, fieldsNames, nodesConfig, fsConfig) {

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
                                    if (nodesConfig) {
                                        node.state = {
                                            expanded: nodesConfig.expandEachNode || currentLevel < EXPANDED_LEVEL
                                        };
                                    }
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
                        MessageService.showMessage('commonErrorMessage', 'error');
                    }
                },
                scope: {
                    array: '=ngModel',
                    config: '=',
                    filterPairsPromise: '=',
                    nodeSelectedCallback: '=',
                    nodeUnselectedCallback: '='
                },
                restrict: 'AE',
                templateUrl: "/Templates/DirectivesTemplates/TreeView.html"
            };
        });
})();
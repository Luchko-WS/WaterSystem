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
                        data: parseArrayToTree(
                            scope.array, scope.textFieldName, scope.levelFieldName, scope.nodeTypeFieldName,
                            scope.folderNodeTypeValue, scope.fileNodeTypeValue, scope.tagFields, scope.mode)
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

                    function parseArrayToTree(array, textFieldName, levelFieldName, nodeTypeFieldName,
                        folderNodeTypeValue, fileNodeTypeValue, tagFields, mode) {

                        try {
                            var root = array[0];
                            root.text = array[0][textFieldName];
                            root._node_id = 0;
                            root.tags = tagFields || [];
                            var stack = [0];
                            var tagsStack = [0];
                            var currentLevel = 1;

                            for (var i = 1; i < array.length; i++) {
                                var node = array[i];
                                node._node_id = i;
                                node.nodes = null;
                                node.text = node[textFieldName];
                                node.tags = [];

                                if (currentLevel == node[levelFieldName]) {
                                    node._parentId = stack[0];
                                    tagsStack[0]++;
                                }
                                else if (currentLevel < node[levelFieldName]) {
                                    stack.unshift(array[i - 1]._node_id);
                                    node._parentId = stack[0];
                                    currentLevel++;
                                }
                                else if (currentLevel > node[levelFieldName]) {
                                    for (var j = 0; j < currentLevel - node[levelFieldName]; j++) {
                                        stack.shift();
                                    }
                                    node._parentId = stack[0];
                                    currentLevel -= currentLevel - node[levelFieldName];
                                }
                            }

                            var map = {}, roots = [], items = array;
                            if (mode === FOLDERS_AND_FILE_MODE) {
                                for (var i = 1; i < items.length; i++) {
                                    var node = items[i];
                                    
                                    if (node[nodeTypeFieldName] == folderNodeTypeValue) {
                                        node.nodes = [];
                                    }
                                    else {
                                        node.nodes = null;
                                    }
                                    map[node._node_id] = i;
                                    if (node._parentId !== 0) {
                                        if (!items[map[node._parentId]].nodes) {
                                            items[map[node._parentId]].nodes = [];
                                        }
                                        items[map[node._parentId]].nodes.push(node);
                                    } else {
                                        roots.push(node);
                                    }
                                }
                            }
                            else if (mode === FOLDERS_MODE) {
                                for (var i = 1; i < items.length; i++) {
                                    if (items[i][nodeTypeFieldName] == fileNodeTypeValue) {
                                        continue;
                                    }
                                    var node = items[i];
                                    map[node._node_id] = i;
                                    if (node._parentId !== 0) {
                                        if (items[map[node._parentId]].nodes == null) {
                                            items[map[node._parentId]].nodes = [];
                                        }
                                        items[map[node._parentId]].nodes.push(node);
                                    } else {
                                        roots.push(node);
                                    }
                                }
                            }
                            root.nodes = roots;
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
                    textFieldName: '=',
                    levelFieldName: '=',
                    nodeTypeFieldName: '=',
                    folderNodeTypeValue: '=',
                    fileNodeTypeValue: '=',
                    tagFields: '=',
                    mode: '=',
                    filterPairsPromise: '=',
                    nodeSelectedCallback: '=',
                    nodeUnselectedCallback: '='
                },
                restrict: 'AE',
                templateUrl: "/Templates/DirectivesTemplates/TreeView.html"
            };
        });
})();
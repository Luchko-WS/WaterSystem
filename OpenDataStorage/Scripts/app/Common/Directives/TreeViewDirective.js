angular.module('MainApp').directive('treeView', function () {
    return {
        replace: true,
        link: function (scope, element, attrs) {

            const FOLDERS_AND_FILE_MODE = 1;
            const FOLDERS_MODE = 2;
            const EXPANDED_LEVEL = 2;

            $('#tree').treeview({
                data: scope.config.fsConfig
                    ? parseArrayToTree(scope.array, scope.config.fieldsNames, scope.config.treeConfig, scope.config.fsConfig)
                    : parseArrayToTree(scope.array, scope.config.fieldsNames, scope.config.treeConfig),
                levels: EXPANDED_LEVEL
            });
            $('#tree').on('nodeUnselected', function (event, data) {
                if (typeof (scope.nodeUnselectedCallback) === 'function' ) {
                    scope.$apply(function () {
                        scope.nodeUnselectedCallback(event, data);
                    });
                }
            });
            $('#tree').on('nodeSelected', function (event, data) {
                if (typeof (scope.nodeSelectedCallback) === 'function') {
                    scope.$apply(function () {
                        scope.nodeSelectedCallback(event, data);
                    });
                }
            });
            $('#tree').on('nodeDblClick', function (event, data) {
                if (typeof (scope.nodeDblClickCallback) === 'function' ) {
                    scope.$apply(function () {
                        scope.nodeDblClickCallback(event, data);
                    });
                }
            });
            $('#tree').on('nodeDropped', function (event, data) {
                if (typeof (scope.nodeDropCallback) === 'function') {
                    scope.$apply(function () {
                        scope.nodeDropCallback(event, data);
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

            function parseArrayToTree(array, fieldsNames, treeConfig, fsConfig) {

                try {
                    var stack = [0];
                    var tagsStack = [0];
                    var currentLevel = 0;

                    //config nodes
                    for (var i = 0; i < array.length; i++) {
                        var node = array[i];
                        node._node_id = i;
                        node.nodes = null;
                        node.text = node[fieldsNames.textFieldName];
                        node.tags = fieldsNames.tagFields || [];
                        node.state = {};

                        //aply fsConfig
                        if (fsConfig) {

                            if (fsConfig.mode === FOLDERS_MODE && array[i][fsConfig.nodeTypeFieldName] == fsConfig.fileNodeTypeValue) {
                                //remove node
                                array.splice(i, 1);
                                i--;
                                continue;
                            }

                            if (node[fsConfig.nodeTypeFieldName] === fsConfig.folderNodeTypeValue) {
                                node.icon = "glyphicon glyphicon-folder-open";
                                node.selectable = !fsConfig.selectOnlyFiles;
                            }
                            else if (node[fsConfig.nodeTypeFieldName] === fsConfig.fileNodeTypeValue) {
                                node.icon = "glyphicon glyphicon-file";
                            }
                        }
                        //end

                        //aply treeConfig
                        if (treeConfig) {
                            if (treeConfig.nodesConfig) {
                                node.state.expanded = treeConfig.nodesConfig.expandEachNode || currentLevel < EXPANDED_LEVEL;
                            }

                            if (treeConfig.draggable) {
                                node.draggable = true;
                                if (fsConfig && node[fsConfig.nodeTypeFieldName] === fsConfig.folderNodeTypeValue) {
                                    node.droppable = true;
                                }
                                else {
                                    node.droppable = true;
                                }
                            }
                        }
                        //end

                        //set parent id
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
                        //end
                    }

                    return parseAndGetTree(array);
                }
                catch (ex) {
                    errorHandler(ex);
                }
            }

            function parseAndGetTree(array) {
                var root = array[0], map = {};
                for (var i = 1; i < array.length; i++) {
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

            function errorHandler(error) {
                console.error(error);
            }
        },
        scope: {
            array: '=ngModel',
            config: '=',
            filterPairsPromise: '=',
            nodeDblClickCallback: '=',
            nodeDropCallback: '=',
            nodeSelectedCallback: '=',
            nodeUnselectedCallback: '='
        },
        restrict: 'AE',
        template: '<div id="tree" style="overflow-x:auto; overflow-y:auto; max-height: 450px;"></div>'
    };
});
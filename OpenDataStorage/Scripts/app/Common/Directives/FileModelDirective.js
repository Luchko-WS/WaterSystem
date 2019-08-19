angular.module('MainApp').directive("fileModel", [function () {
    return {
        scope: {
            fileModel: "="
        },
        restrict: 'A',
        link: function (scope, element, attributes) {
            element.bind("change", function () {
                scope.$apply(function () {
                    var file = element[0].files[0];
                    scope.fileModel = file;
                });
            });
        }
    };
}]);
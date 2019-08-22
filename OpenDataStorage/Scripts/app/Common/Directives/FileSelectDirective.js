angular.module('MainApp').directive("fileSelect", function () {
    return {
        scope: {
            fileModel: '=ngModel',
            fileFilter: '='
        },
        restrict: 'E',
        replace: true,
        link: function (scope, element, attributes) {
            scope.changeCallback = function (el) {
                scope.$apply(function () {
                    var file = el.files[0];
                    scope.fileModel = file;
                });
            };
        },
        templateUrl: "/Templates/DirectivesTemplates/FileSelect.html"
    };
});
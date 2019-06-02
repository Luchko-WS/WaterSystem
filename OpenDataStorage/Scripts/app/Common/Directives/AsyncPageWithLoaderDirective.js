angular.module('MainApp').directive('asyncPageWithLoader', function () {
    return {
        transclude: true,
        replace: true,
        link: function (scope, element, attrs) { },
        scope: {
            isLoaded: '='
        },
        restrict: 'E',
        template:
            '<div>' +
                '<div ng-if= "!isLoaded" style = "text-align: center; margin-top: 50px; margin-bottom: 50px" >' +
                     '<img style="height: 80px" src="/Content/images/ajax-loader.gif" />' +
                '</div>' +
                '<div ng-if="isLoaded">' +
                    '<div ng-transclude></div>' +
                '</div>' +
            '</div>'
    };
});
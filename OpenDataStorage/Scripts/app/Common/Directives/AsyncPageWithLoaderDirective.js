angular.module('MainApp').directive('asyncPageWithLoader', function () {
    return {
        transclude: true,
        replace: true,
        link: function (scope, element, attrs) { },
        scope: {
            isLoaded: '='
        },
        restrict: 'E',
        templateUrl: "/Templates/DirectivesTemplates/AsyncPageWithLoader.html"
    };
});
angular.module('MainApp').directive('asyncPageWithLoader', function ($compile, $templateRequest) {
    return {
        transclude: true,
        replace: true,
        link: function (scope, element, attrs) { },
        scope: {
            isLoaded: '='
        },
        restrict: 'E',
        templateUrl: '/Templates/DirectivesTemplates/LoadingSpinnerTemplate.html'
    };
});
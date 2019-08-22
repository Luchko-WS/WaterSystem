angular.module('MainApp').directive('extraBootstrapNavbar', function () {
    return {
        transclude: true,
        replace: true,
        restrict: 'E',
        templateUrl: "/Templates/DirectivesTemplates/ExtraBootstrapNavbar.html"
    };
});
﻿angular.module('OnlineDictionary').directive('extraBootstrapNavbar', function ($compile, $templateRequest) {
    return {
        transclude: true,
        replace: true,
        restrict: 'E',
        templateUrl: '/Templates/DirectivesTemplates/ExtraBootstrapNavbar.html'
    };
});
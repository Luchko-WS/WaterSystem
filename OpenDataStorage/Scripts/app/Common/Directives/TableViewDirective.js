angular.module('MainApp').directive('tableView', function () {
    return {
        replace: true,
        link: function (scope, element, attrs) {
            scope.filterValues = {};
            scope.toggleFilter = toggleFilter;

            if (typeof (scope.filterCallback) !== 'function') {
                scope.filterCallback = undefined;
                scope.toggleFilter = undefined;
            }
            if (typeof (scope.createCallback) !== 'function') {
                scope.createCallback = undefined;
            }
            if (typeof (scope.editCallback) !== 'function') {
                scope.editCallback = undefined;
            }
            if (typeof (scope.removeCallback) !== 'function') {
                scope.removeCallback = undefined;
            }

            function toggleFilter() {
                scope.filterIsShowed = !scope.filterIsShowed;
                if (!scope.filterIsShowed) {
                    scope.filterValues.date = null;
                    scope.filterValues.name = null;
                    scope.filterCallback();
                }
            }
        },
        scope: {
            data: '=ngModel',
            filterCallback: '=',
            createCallback: '=',
            editCallback: '=',
            removeCallback: '=',
            enableEditing: '='
        },
        restrict: 'AE',
        templateUrl: "/Templates/DirectivesTemplates/TableView.html"
    };
});
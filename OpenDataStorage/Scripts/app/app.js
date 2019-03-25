(function () {
    'use strict';
    window.app = angular.module('MainApp',
        [
            'ui.bootstrap',
            'ngCookies',
            'pascalprecht.translate'
        ]);

    app.config(['$translateProvider', function ($translateProvider) {
        $translateProvider.useUrlLoader('/api/Languages');
        $translateProvider.useSanitizeValueStrategy('escapeParameters');
        
        var $cookies;
        angular.injector(['ngCookies']).invoke(['$cookies', function (_$cookies_) {
            $cookies = _$cookies_;
        }]);

        var lang = $cookies.get('culture');
        if (!lang) {
            lang = 'uk';
        }
        $translateProvider.preferredLanguage(lang);
    }]);
})();
(function (window, angular) {
    'use strict';

    window.app = angular.module('app', [
        'ngRoute',
        'ngAnimate',
        'ngResource',
        'angularCharts',
        'resources',
        'controllers',
        'services'
    ]);

    //#region Routes
    app.config(['$routeProvider', '$locationProvider',
        function ($routeProvider, $locationProvider) {
            $routeProvider.
                when('/current', { templateUrl: 'App/views/home.html', controller: 'RegionOffersCtrl' }).
                when('/about', { templateUrl: 'App/views/about.html', controller: 'AboutCtrl' }).
                when('/hist/:regionName/:daysCount', { templateUrl: 'App/views/hist.html', controller: 'HistCtrl' }).
                otherwise({ redirectTo: '/current' });


            $locationProvider.html5Mode(false);
        }]);
    //#endregion Routes

})(window, angular);
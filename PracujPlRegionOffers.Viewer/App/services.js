(function (angular) {
    'use strict';

    var services = angular.module('services', []);

    //#region $path
    services.factory('$path', function () {
        var uri = window.location.href;
        var ind = uri.indexOf('#');

        var baseUrl = uri;

        if (ind > 0) {
            baseUrl = uri.substr(0, ind);
        }

        if (baseUrl[baseUrl.length - 1] != '/') {
            baseUrl = baseUrl + '/';
        }

        return function (url) {
            return baseUrl + url;
        };
    });
    //#endregion


})(window.angular);
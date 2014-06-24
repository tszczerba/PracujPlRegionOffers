(function (angular) {
    'use strict';

    var resources = angular.module('resources', []);

    resources.factory('JobOffers', ['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/JobOffers/:id'), { id: '@Id' }, {
            getAll: { method: 'GET', isArray: false }
        });
    }]);

    resources.factory('RegionHistory', ['$resource', '$path', function ($resource, $path) {
        return $resource($path('api/JobOffers/:name/:count'), { name: '@name', count: '@count' }, {
            get: { method: 'GET', isArray: true }
        });
    }]);

})(window.angular);

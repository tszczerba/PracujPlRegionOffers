(function(angular) {
    'use strict';

    var controllers = angular.module('controllers', []);

    //#region HomeCtrl
    controllers.controller('RegionOffersCtrl', [
        '$rootScope', '$scope', '$interval', 'JobOffers', function ($rootScope, $scope, $interval, JobOffers) {

            $rootScope.homePage = true;
            $rootScope.histPage = false;
            $rootScope.aboutPage = false;
            $rootScope.loadingInd = true;

            $scope.title = 'Oferty pracy wg. województw';

            $scope.refreshData = function(values) {
                $scope.regionOffer = values;
                $rootScope.loadingInd = false;
            }

            $scope.regionOffer = JobOffers.getAll(function (values) {
                $scope.regionOffer = values;
                $rootScope.loadingInd = false;
            });
            $scope.intervalRefreshS = 300;
            $scope.intervalRefreshMs = $scope.intervalRefreshS * 1000;

            $scope.intervalRefresh = function () {
                if ($rootScope.homePage && !$rootScope.histPage) {
                    $rootScope.loadingInd = true;
                    JobOffers.getAll(function (values) {
                        $scope.regionOffer = values;
                        $rootScope.loadingInd = false;
                    });
                }
            }

            $interval($scope.intervalRefresh, $scope.intervalRefreshMs);

        }
    ]);
    //#endregion

    //#region AboutCtrl
    controllers.controller('AboutCtrl', [
        '$rootScope', '$scope', function ($rootScope, $scope) {
            $rootScope.homePage = false;
            $rootScope.histPage = false;
            $rootScope.aboutPage = true;
            $rootScope.loadingInd = false;
            $scope.title = 'O Aplikacji';

        }
    ]);
    //#endregion

    //#region HistCtrl
    controllers.controller('HistCtrl', [
        '$rootScope', '$scope', '$routeParams', 'RegionHistory', function ($rootScope, $scope, $routeParams, RegionHistory) {
            $rootScope.homePage = true;
            $rootScope.aboutPage = false;
            $rootScope.histPage = true;
            $rootScope.loadingInd = false;

            $scope.capitaliseFirstLetter = function (string) {
                return string.charAt(0).toUpperCase() + string.slice(1);
            };

            $scope.regionName = $scope.capitaliseFirstLetter($routeParams.regionName);
            $scope.title = 'Średnia z dni dla województwa: ' + $scope.regionName;
            $scope.daysCount = $routeParams.daysCount;

            $scope.data = {
                series: [$scope.regionName],
                data: []
            }

            RegionHistory.get({ name: $scope.regionName, count: $scope.daysCount }, function(histData) {
                angular.forEach(histData, function(value, key) {
                    $scope.data.data.push({
                        x: value.loadDateTime.substring(0, 10),
                        y: [ value.jobOffers ]
                    });
                });
            });


            $scope.chartType = 'bar';

            $scope.config = {
                labels: false,
                legend: {
                    display: false,
                    position: 'right'
                },
                innerRadius: 0,
                lineLegend: 'lineEnd',
            }

        }
    ]);
    //#endregion

})(window.angular)
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.DeveloperChargeListService = function ($http, $q, $compile, $rootScope) {
        var DeveloperChargeListService = {};

        DeveloperChargeListService.GetDailyChargList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/ChargBill/GetDeveloperChargeList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        return DeveloperChargeListService;
    };

    ckFramework.DeveloperChargeListService.$inject = injectParams;
    app.register.factory('DeveloperChargeListService', ckFramework.DeveloperChargeListService);
});
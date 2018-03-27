'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.NotificeConfigService = function ($http, $q, $compile, $rootScope) {
        var NotificeConfigService = {};

        NotificeConfigService.GetNotificeConfigByComDeptId = function (deptId, deptName, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/NotificeConfig/GetNotificeConfig',
                    data: {
                        deptId: deptId, deptName: deptName
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        NotificeConfigService.SaveNotice = function (info, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/NotificeConfig/SaveNotice',
                    data: {
                        inputData: info
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        return NotificeConfigService;
    };

    ckFramework.NotificeConfigService.$inject = injectParams;
    app.register.factory('NotificeConfigService', ckFramework.NotificeConfigService);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.PersonalizedConfigurationService = function ($http, $q, $compile, $rootScope) {
        var PersonalizedConfigurationService = {};

        PersonalizedConfigurationService.GetCommunityConfig = function (deptId, callback) {
            $http({
                method: 'POST',
                data: { CommunityDeptId: deptId },
                url: 'PropertyMgr/PersonalizedConfiguration/GetCommunityConfig',
            }).success(function (data) {
                if (callback) {
                    callback(data)
                }
            });
        }

        PersonalizedConfigurationService.SaveCommunityConfig = function (config, callback) {
            $http({
                method: 'POST',
                data: {
                    model: config 
                },
                url: '/PropertyMgr/PersonalizedConfiguration/SaveCommunityConfig',
            }).success(function (data) {
                if (callback) {
                    callback(data);
                }
            });
        }

        return PersonalizedConfigurationService;
    };

    ckFramework.PersonalizedConfigurationService.$inject = injectParams;
    app.register.factory('PersonalizedConfigurationService', ckFramework.PersonalizedConfigurationService);
});
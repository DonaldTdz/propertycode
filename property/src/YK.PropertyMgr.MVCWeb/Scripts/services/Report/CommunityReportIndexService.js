'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.CommunityReportIndexService = function ($http, $q, $compile, $rootScope) {
        var CommunityReportIndexService = {};

        CommunityReportIndexService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return CommunityReportIndexService;
    };

    ckFramework.CommunityReportIndexService.$inject = injectParams;
    app.register.factory('CommunityReportIndexService', ckFramework.CommunityReportIndexService);
});
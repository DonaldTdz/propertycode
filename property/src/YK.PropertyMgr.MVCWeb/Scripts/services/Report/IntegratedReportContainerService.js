'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.IntegratedReportContainerService = function ($http, $q, $compile, $rootScope) {
        var IntegratedReportContainerService = {};

        IntegratedReportContainerService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return IntegratedReportContainerService;
    };

    ckFramework.IntegratedReportContainerService.$inject = injectParams;
    app.register.factory('IntegratedReportContainerService', ckFramework.IntegratedReportContainerService);
});
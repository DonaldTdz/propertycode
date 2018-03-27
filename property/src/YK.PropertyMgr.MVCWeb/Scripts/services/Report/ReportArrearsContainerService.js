'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ReportArrearsContainerService = function ($http, $q, $compile, $rootScope) {
        var ReportArrearsContainerService = {};

        ReportArrearsContainerService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl,function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return ReportArrearsContainerService;
    };

    ckFramework.ReportArrearsContainerService.$inject = injectParams;
    app.register.factory('ReportArrearsContainerService', ckFramework.ReportArrearsContainerService);
});
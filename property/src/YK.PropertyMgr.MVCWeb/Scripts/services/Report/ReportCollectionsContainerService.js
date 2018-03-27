'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ReportCollectionsContainerService = function ($http, $q, $compile, $rootScope) {
        var ReportCollectionsContainerService = {};

        ReportCollectionsContainerService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return ReportCollectionsContainerService;
    };

    ckFramework.ReportCollectionsContainerService.$inject = injectParams;
    app.register.factory('ReportCollectionsContainerService', ckFramework.ReportCollectionsContainerService);
});
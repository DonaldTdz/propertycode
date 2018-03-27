'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.SubjectHouseRefListService = function ($http, $q, $compile, $rootScope) {
        var subjectHouseRefListService = {};

        subjectHouseRefListService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
           
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return subjectHouseRefListService;
    };

    ckFramework.SubjectHouseRefListService.$inject = injectParams;
    app.register.factory('SubjectHouseRefListService', ckFramework.SubjectHouseRefListService);
});
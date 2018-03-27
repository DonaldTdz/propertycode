'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.CarParkContainerService = function ($http, $q, $compile, $rootScope) {
        var carParkContainerService = {};
        carParkContainerService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext, param) {
            if (param == undefined || param == null) {
                param = {};
            }
            param.DeptId = $('#SelectDeptId').val();
            param.DeptName = $('#SelectDeptName').val();
            param.DeptType = $('#SelectDeptType').val();
            param.IsDeveloper = $("#hidIsDeveloper").val();
            require(controllerUrl, function (app) {
                $("#" + divPageId).load(loadUrl, param, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return carParkContainerService;
    };

    ckFramework.CarParkContainerService.$inject = injectParams;
    app.register.factory('CarParkContainerService', ckFramework.CarParkContainerService);
});
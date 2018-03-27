'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.DeptContainerService = function ($http, $q, $compile, $rootScope) {
        var deptContainerService = {};

        deptContainerService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext, param) {
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
        return deptContainerService;
    };

    ckFramework.DeptContainerService.$inject = injectParams;
    app.register.factory('DeptContainerService', ckFramework.DeptContainerService);
});
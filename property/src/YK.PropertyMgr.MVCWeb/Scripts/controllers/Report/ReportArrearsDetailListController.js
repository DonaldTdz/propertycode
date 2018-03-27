'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ReportArrearsDetailListController = function ($http, $scope, $rootScope) {

        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ReportArrearsDetailListData;
        $scope.ComDeptList = ckFramework.ReportArrearsDetailListData.ReportDeptinfo;
        $scope.ComDeptId;

    };

    ckFramework.ReportArrearsDetailListController.$inject = injectParams;
    app.register.controller('ReportArrearsDetailListController', ckFramework.ReportArrearsDetailListController);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.IntegratedReportByHouseController = function ($http, $scope, $rootScope) {

        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.IntegratedReportByHouseData;
        $scope.ComDeptList = ckFramework.IntegratedReportByHouseData.ReportDeptinfo;
        $scope.ComDeptId = $scope.ComDeptList[0].Id;



    };

    ckFramework.IntegratedReportByHouseController.$inject = injectParams;
    app.register.controller('IntegratedReportByHouseController', ckFramework.IntegratedReportByHouseController);
});
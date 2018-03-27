'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ArrearsCollComparisonChartController = function ($http, $scope, $rootScope) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ArrearsCollComparisonChartData;
        $scope.ComDeptList = ckFramework.ArrearsCollComparisonChartData.ReportDeptinfo;
        $scope.ComDeptId;


        $scope.SeachEcharts = function ()
        {
            if ($scope.ComDeptId > 0)
                seachEchartsByComDeptId($scope.ComDeptId);
            else
                {
                ckFramework.ModalHelper.Alert("请选择小区后提交");
                return;
            }
        }
    };

    ckFramework.ArrearsCollComparisonChartController.$inject = injectParams;
    app.register.controller('ArrearsCollComparisonChartController', ckFramework.ArrearsCollComparisonChartController);
});
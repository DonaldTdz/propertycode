'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReportService'];
    ckFramework.MonthReportDetailController = function ($http, $scope, $rootScope, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.MonthReportDetailData;
        $scope.ComDeptList = ckFramework.MonthReportDetailData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.MonthReportDetailData.DefaultDeptId;

        $scope.MonthDetailReportExportData = function () {

            var LouyuIdStr = $("#SearchContainerDaytDetailReport input:hidden[name='LouyuIdStr']").val();
            var parameters = "?ComDeptIdStr=" + $('#dllComDeptIdStr').val() + "&ChargeDate=" + $('#txtDetailChargeDate').val() + "&DefaultComDeptId" + $('#hidDetailDefaultComDeptId').val() + "&LouyuIdStr=" + LouyuIdStr;
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/GetMonthDetailReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }
    };

    ckFramework.MonthReportDetailController.$inject = injectParams;
    app.register.controller('MonthReportDetailController', ckFramework.MonthReportDetailController);
});
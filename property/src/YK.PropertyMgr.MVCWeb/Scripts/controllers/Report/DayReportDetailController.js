'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReportService'];
    ckFramework.DayReportDetailController = function ($http, $scope, $rootScope, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.DayReportDetailData;
        $scope.ComDeptList = ckFramework.DayReportDetailData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.DayReportDetailData.DefaultDeptId;

        $scope.DayDetailReportExportData = function () {

            var LouyuIdStr = $("#SearchContainerDaytDetailReport input:hidden[name='LouyuIdStr']").val();
            var parameters = "?ComDeptIdStr=" + $('#dllComDeptIdStr').val() + "&ChargeDate=" + $('#txtDetailChargeDate').val() + "&DefaultComDeptId=" + $('#hidDetailDefaultComDeptId').val() + "&LouyuIdStr=" + LouyuIdStr;
          
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/GetDayDetailReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }
    };

    ckFramework.DayReportDetailController.$inject = injectParams;
    app.register.controller('DayReportDetailController', ckFramework.DayReportDetailController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReportService'];
    ckFramework.DayReportContainerController = function ($http, $scope, $rootScope, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.DayReportData;
        $scope.ComDeptList = ckFramework.DayReportData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.DayReportData.DefaultDeptId;
        $scope.NMonth = (new Date()).getMonth() + 1;
        $scope.DayReportDataList = [];
        $scope.Search = {};
        $scope.TotalStyle = {
            "background-color": "#ddd",
            "font-weight":"bold"
        };

        $scope.GetDayReportDataList = function () {
            $scope.Search.ComDeptIdStr = $('#ComDeptIdStr').val();
            $scope.Search.ChargeDate = $('#ChargeDate').val();
            $scope.Search.DefaultComDeptId = $('#DefaultComDeptId').val();
            $scope.Search.LouyuIdStr = $("#SearchContainerDayReport input:hidden[name='LouyuIdStr']").val();


            ckFramework.ModalHelper.OpenWait();
            ReportService.GetDayReportDataList($scope.Search, function (data) {
                $scope.DayReportDataList = data;
                $scope.NMonth = (new Date($scope.Search.ChargeDate)).getMonth() + 1;
                ckFramework.ModalHelper.CloseWait();
            });
        }

        $scope.GetDayReportDataList();

        $scope.DayReportExportData = function () {
            var LouyuIdStr = $("#SearchContainerDayReport input:hidden[name='LouyuIdStr']").val();
            var parameters = "?ComDeptIdStr=" + $('#ComDeptIdStr').val() + "&ChargeDate=" + $('#ChargeDate').val() + "&DefaultComDeptId=" + $('#DefaultComDeptId').val() + "&LouyuIdStr=" + LouyuIdStr;
         
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/DayReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }
    };

    ckFramework.DayReportContainerController.$inject = injectParams;
    app.register.controller('DayReportContainerController', ckFramework.DayReportContainerController);
});
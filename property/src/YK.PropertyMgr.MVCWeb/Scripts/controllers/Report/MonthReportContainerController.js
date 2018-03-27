'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReportService'];
    ckFramework.MonthReportContainerController = function ($http, $scope, $rootScope, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.MonthReportData;
        $scope.ComDeptList = ckFramework.MonthReportData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.MonthReportData.DefaultDeptId;
        $scope.NMonth = (new Date()).getMonth() + 1;
        $scope.MonthReportDataList = [];
        $scope.Search = {};
        $scope.ComDeptId;
        $scope.TotalStyle = {
            "background-color": "#ddd",
            "font-weight": "bold"
        };

        $scope.GetMonthReportDataList = function () {
            $scope.Search.ComDeptIdStr = $('#ComDeptIdStr').val();
            $scope.Search.ChargeDate = $('#ChargeDate').val();
            $scope.Search.DefaultComDeptId = $('#DefaultComDeptId').val();
            $scope.Search.LouyuIdStr = $("#SearchContainerMonthReport input:hidden[name='LouyuNameIdStr']").val();
            ckFramework.ModalHelper.OpenWait();
            ReportService.GetMonthReportDataList($scope.Search, function (data) {
                $scope.MonthReportDataList = data;
                $scope.NMonth = (new Date($scope.Search.ChargeDate)).getMonth() + 1;
                ckFramework.ModalHelper.CloseWait();
            });
        }

        $scope.GetMonthReportDataList();

        $scope.MonthReportExportData = function () {
            var parameters = "?ComDeptIdStr=" + $('#ComDeptIdStr').val() + "&ChargeDate=" + $('#ChargeDate').val() + "&DefaultComDeptId=" + $('#DefaultComDeptId').val() +"&LouyuIdStr="+ $("#SearchContainerMonthReport input:hidden[name='LouyuNameIdStr']").val();
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/MonthReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }
    };

    ckFramework.MonthReportContainerController.$inject = injectParams;
    app.register.controller('MonthReportContainerController', ckFramework.MonthReportContainerController);
});
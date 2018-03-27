'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReportService'];
    ckFramework.IntegratedReportByChargeSubjectController = function ($http, $scope, $rootScope, ReportService) {

        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.IntegratedReportByChargeSubjectData;
        $scope.ComDeptList = ckFramework.IntegratedReportByChargeSubjectData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.IntegratedReportByChargeSubjectData.DefaultDeptId
        $scope.IntegratedReportByChargeSubjectList = [];
        $scope.Search = {};
        $scope.TotalStyle = {
            "background-color": "#ddd",
            "font-weight": "bold"
        };

        $scope.GetIntegratedReportByChargeSubjectDataList = function () {
            $scope.Search.ComDeptIdStr = $("#SearchContainerIntegratedReportByChargeSubject select[name='ComDeptIdStr']").val();
            $scope.Search.Paydate = $("#SearchContainerIntegratedReportByChargeSubject input[name='Paydate']").val();
            $scope.Search.DefaultComDeptId = $("#SearchContainerIntegratedReportByChargeSubject input:hidden[name='DefaultComDeptId']").val();
            $scope.Search.BeginDate = $("#SearchContainerIntegratedReportByChargeSubject input[name='BeginDate']").val();
            $scope.Search.EndDate = $("#SearchContainerIntegratedReportByChargeSubject input[name='EndDate']").val();
            $scope.Search.LouyuIdStr = $("#SearchContainerIntegratedReportByChargeSubject input:hidden[name='LouyuIdStr']").val();
             


            ckFramework.ModalHelper.OpenWait();
            ReportService.GetIntegratedReportByChargeSubjectDataList($scope.Search, function (data) {
                $scope.IntegratedReportByChargeSubjectList = data;

                ckFramework.ModalHelper.CloseWait();
            });
        }

        $scope.GetIntegratedReportByChargeSubjectDataList();

        $scope.IntegratedReportByChargeSubjectExportData = function () {

            var ComDeptIdStr = $("#SearchContainerIntegratedReportByChargeSubject select[name='ComDeptIdStr']").val();
            var Paydate = $("#SearchContainerIntegratedReportByChargeSubject input[name='Paydate']").val();
            var DefaultComDeptId = $("#SearchContainerIntegratedReportByChargeSubject input:hidden[name='DefaultComDeptId']").val();
            var BeginDate = $("#SearchContainerIntegratedReportByChargeSubject input[name='BeginDate']").val();
            var EndDate = $("#SearchContainerIntegratedReportByChargeSubject input[name='EndDate']").val();
            var LouyuIdStr = $("#SearchContainerIntegratedReportByChargeSubject input:hidden[name='LouyuIdStr']").val();
            var parameters = "?ComDeptIdStr=" + ComDeptIdStr + "&Paydate=" + Paydate + "&DefaultComDeptId=" + DefaultComDeptId + "&BeginDate=" + BeginDate + "&EndDate=" + EndDate + "&LouyuIdStr=" + LouyuIdStr;
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/IntegratedReportByChargeSubjectExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }






    };

    ckFramework.IntegratedReportByChargeSubjectController.$inject = injectParams;
    app.register.controller('IntegratedReportByChargeSubjectController', ckFramework.IntegratedReportByChargeSubjectController);
});
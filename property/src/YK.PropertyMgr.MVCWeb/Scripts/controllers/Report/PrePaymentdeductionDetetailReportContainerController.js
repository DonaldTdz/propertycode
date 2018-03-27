'use strict';
define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', '$compile', 'ReportService'];
    ckFramework.PrePaymentdeductionDetetailReportContainerController = function ($http, $scope, $rootScope, $compile, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.PrePaydeductionDetailRepListData;
        $scope.ComDeptList = ckFramework.PrePaydeductionDetailRepListData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.PrePaydeductionDetailRepListData.DefaultDeptId;
        $scope.ChargeSubjectList = ckFramework.PrePaydeductionDetailRepListData.ReportChargeSubjectInfo;
        $scope.ChargeSubjectId = ckFramework.PrePaydeductionDetailRepListData.DefaultChargeSubjectId;


        $scope.PrePayDetailRepExportData = function () {
            var parameters = "?ComDeptIdStr=" + $('#ComDeptIdStr').val()
                + "&ResourceName=" + $('#ResourceName').val()
                + "&OwnerName=" + $('#OwnerName').val()
                + "&ReceiptNum=" + $('#ReceiptNum').val()
                + "&ChargeSubjectIdStr=" + $('#ChargeSubjectIdStr').val()
                + "&ChargeSubjectId=" + $('#DefaultChargeSubjectId').val()
                + "&ChargeBeginDate=" + $('#ChargeBeginDate').val()
                + "&ChargeEndDate=" + $('#ChargeEndDate').val()
                + "&DefaultComDeptId=" + $('#DefaultComDeptId').val()
                + "&LouyuIdStr=" + $("#SearchContainerPrePayDetailRep input:hidden[name='LouyuIdStr']").val();
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/PrePaymentdeductionDetailReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }

        $scope.PreComDeptIdChange = function () {
            ReportService.GetPrePaymentDetailChargeSubjectList($scope.ComDeptId, function (data) {
                $scope.ChargeSubjectList = data;
                $scope.ChargeSubjectId = data[0].Id;
            });
        }

    };

    ckFramework.PrePaymentdeductionDetetailReportContainerController.$inject = injectParams;
    app.register.controller('PrePaymentdeductionDetetailReportContainerController', ckFramework.PrePaymentdeductionDetetailReportContainerController);
});
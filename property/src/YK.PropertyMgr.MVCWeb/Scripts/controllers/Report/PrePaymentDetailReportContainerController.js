'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', '$compile', 'ReportService'];
    ckFramework.PrePaymentDetailReportContainerController = function ($http, $scope, $rootScope, $compile, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.PrePayDetailRepListData;
        $scope.ComDeptList = ckFramework.PrePayDetailRepListData.ReportDeptinfo;
        $scope.ChargeSubjectList = ckFramework.PrePayDetailRepListData.ReportChargeSubjectInfo;
        $scope.ComDeptId = ckFramework.PrePayDetailRepListData.DefaultDeptId;
        $scope.ChargeSubjectId = ckFramework.PrePayDetailRepListData.DefaultChargeSubjectId;
     






        $scope.PrePayDetailRepExportData = function () {
            var parameters = "?ComDeptIdStr=" + $('#ComDeptIdStr').val()
                + "&ResourceName=" + $('#ResourceName').val()
                + "&OwnerName=" + $('#OwnerName').val()
                + "&ReceiptNum=" + $('#ReceiptNum').val()
                + "&ChargeSubjectIdStr=" + $('#ChargeSubjectIdStr').val()
                + "&DefaultChargeSubjectId=" + $('#DefaultChargeSubjectId').val()
                + "&ChargeBeginDate=" + $('#ChargeBeginDate').val()
                + "&ChargeEndDate=" + $('#ChargeEndDate').val()
                + "&DefaultComDeptId=" + $('#DefaultComDeptId').val()
                + "&LouyuIdStr=" + $("#SearchContainerPrePayDetailRep input:hidden[name='LouyuIdStr']").val();
             
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/PrePaymentDetailReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }

        $scope.PreComDeptIdChange = function ()
        {
            ReportService.GetPrePaymentDetailChargeSubjectList($scope.ComDeptId, function (data) {
                $scope.ChargeSubjectList = data;
                $scope.ChargeSubjectId = data[0].Id;
            });
        }





    };

    ckFramework.PrePaymentDetailReportContainerController.$inject = injectParams;
    app.register.controller('PrePaymentDetailReportContainerController', ckFramework.PrePaymentDetailReportContainerController);
});
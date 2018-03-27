'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', '$compile', 'ReportService'];
    ckFramework.PaymentDiscountInfoReportContainerController = function ($http, $scope, $rootScope, $compile, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.PayDisInfRepListData;
        $scope.ComDeptList = ckFramework.PayDisInfRepListData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.PayDisInfRepListData.DefaultDeptId;
        $scope.PayDisInfRepDataList = [];
        $scope.Search = {};
        $scope.TotalStyle = {
            "background-color": "#ddd",
            "font-weight": "bold"
        };

        //$scope.GetPayDisInfRepDataList = function () {
        //    $scope.Search.ComDeptIdStr = $('#ComDeptIdStr').val();
        //    $scope.Search.ResourceName = $('#ResourceName').val();
        //    $scope.Search.OwnerName = $('#OwnerName').val();
        //    $scope.Search.ReceiptNum = $('#ReceiptNum').val();
        //    $scope.Search.DiscountDesc = $('#DiscountDesc').val();
        //    $scope.Search.Status = $('#Status').val();
        //    $scope.Search.BeginDate = $('#BeginDate').val();
        //    $scope.Search.EndDate = $('#EndDate').val();
        //    $scope.Search.DefaultComDeptId = $('#DefaultComDeptId').val();
        //    ckFramework.ModalHelper.OpenWait();
        //    ReportService.GetPayDisInfRepDataList($scope.Search, function (data) {
        //        $scope.PayDisInfRepDataList = data.data;
        //        ckFramework.ModalHelper.CloseWait();
        //    });
        //}

        //$scope.GetPayDisInfRepDataList();

        $scope.PayDisInfRepExportData = function () {
            var parameters = "?ComDeptIdStr=" + $('#ComDeptIdStr').val()
                + "&ResourceName=" + $('#ResourceName').val()
                + "&OwnerName=" + $('#OwnerName').val()
                + "&ReceiptNum=" + $('#ReceiptNum').val()
                + "&DiscountDesc=" + $('#DiscountDesc').val()
                + "&BeginDate=" + $('#BeginDate').val()
                + "&EndDate=" + $('#EndDate').val()
                + "&DefaultComDeptId=" + $('#DefaultComDeptId').val()
                + "&Status=" + $("select[name='Status']").val()
                + "&LouyuIdStr=" + $("#SearchContainerPayDisInfRep input:hidden[name='LouyuIdStr']").val();

            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/PaymentDiscountInfoReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }

    };

    ckFramework.PaymentDiscountInfoReportContainerController.$inject = injectParams;
    app.register.controller('PaymentDiscountInfoReportContainerController', ckFramework.PaymentDiscountInfoReportContainerController);
});
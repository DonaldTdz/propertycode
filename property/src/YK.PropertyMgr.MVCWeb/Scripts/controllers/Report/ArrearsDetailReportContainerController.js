'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', '$compile', 'ReportService'];
    ckFramework.ArrearsDetailReportContainerController = function ($http, $scope, $rootScope, $compile, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ArrearsDetailRepListData;
        $scope.ComDeptList = ckFramework.ArrearsDetailRepListData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.ArrearsDetailRepListData.DefaultDeptId;








        $scope.ArrearsReportExportData = function () {

            var parameters = "?ComDeptIdStr=" + $scope.ComDeptId
                + "&ResourceName=" + $(" #SearchContainerArrearsDetailRep input[name='ResourceName']").val()
                + "&OwnerName=" + $(" #SearchContainerArrearsDetailRep input[name='OwnerName']").val()
                + "&ChargeDate=" + $("#SearchContainerArrearsDetailRep input[name='ChargeDate']").val()
                + "&DefaultComDeptId=" + $("#SearchContainerArrearsDetailRep input:hidden[name='DefaultComDeptId']").val()
                + "&LouyuIdStr=" + $("#SearchContainerArrearsReport input:hidden[name='LouyuIdStr']").val();
       
            var iframe = document.createElement("iframe");


            iframe.src = "PropertyMgr/Report/GetArrearsDetailReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }







    };

    ckFramework.ArrearsDetailReportContainerController.$inject = injectParams;
    app.register.controller('ArrearsDetailReportContainerController', ckFramework.ArrearsDetailReportContainerController);
});
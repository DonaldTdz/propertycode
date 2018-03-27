'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', '$compile', 'ReportService'];
    ckFramework.MeterDetailReportContainerController = function ($http, $scope, $rootScope, $compile, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.MeterDetailRepListData;
        $scope.ComDeptList = ckFramework.MeterDetailRepListData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.MeterDetailRepListData.DefaultDeptId;

        $scope.MeterReportExportData = function () {

            var parameters = "?ComDeptIdStr=" + $scope.ComDeptId
                + "&ResourceName=" + $(" #SearchContainerMeterDetailRep input[name='ResourceName']").val()
                + "&OwnerName=" + $(" #SearchContainerMeterDetailRep input[name='OwnerName']").val()
                + "&BeginDate=" + $("#SearchContainerMeterDetailRep input[name='BeginDate']").val()
                + "&EndDate=" + $("#SearchContainerMeterDetailRep input[name='EndDate']").val()
                + "&DefaultComDeptId=" + $("#SearchContainerMeterDetailRep input:hidden[name='DefaultComDeptId']").val()
                + "&LouyuIdStr=" + $("#SearchContainerMeterDetailRep input:hidden[name='LouyuIdStr']").val();

            var iframe = document.createElement("iframe");

            iframe.src = "PropertyMgr/Report/GetMeterDetailReportExportData" + parameters;
            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }

    };

    ckFramework.MeterDetailReportContainerController.$inject = injectParams;
    app.register.controller('MeterDetailReportContainerController', ckFramework.MeterDetailReportContainerController);
});
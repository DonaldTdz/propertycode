'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', '$compile', 'ReportService'];
    ckFramework.ExternalchargeDetailContainerController = function ($http, $scope, $rootScope, $compile, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ExternalchargeDetailRepListData;
        $scope.ComDeptList = ckFramework.ExternalchargeDetailRepListData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.ExternalchargeDetailRepListData.DefaultDeptId;

        $scope.ExternalchargeReportExportData = function () {

            var parameters = "?ComDeptIdStr=" + $scope.ComDeptId
                + "&ResourceName=" + $(" #SearchContainerExternalchargeDetailRep input[name='ResourceName']").val()
                + "&Number=" + $(" #SearchContainerExternalchargeDetailRep input[name='Number']").val()
                + "&BeginDate=" + $("#SearchContainerExternalchargeDetailRep input[name='BeginDate']").val()
                + "&EndDate=" + $("#SearchContainerExternalchargeDetailRep input[name='EndDate']").val()
                + "&DefaultComDeptId=" + $("#SearchContainerExternalchargeDetailRep input:hidden[name='DefaultComDeptId']").val();

            var iframe = document.createElement("iframe");

            iframe.src = "PropertyMgr/Report/GetExternalchargeDetailReportExportData" + parameters;
            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }

    };

    ckFramework.ExternalchargeDetailContainerController.$inject = injectParams;
    app.register.controller('ExternalchargeDetailContainerController', ckFramework.ExternalchargeDetailContainerController);
});
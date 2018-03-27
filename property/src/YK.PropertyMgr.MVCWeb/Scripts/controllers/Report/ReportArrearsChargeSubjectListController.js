'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ReportArrearsChargeSubjectListController = function ($http, $scope, $rootScope) {
     
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ReportArrearsChargeSubjectListData;
        $scope.ComDeptList = ckFramework.ReportArrearsChargeSubjectListData.ReportDeptinfo;
        $scope.ComDeptId;
      
    };

    ckFramework.ReportArrearsChargeSubjectListController.$inject = injectParams;
    app.register.controller('ReportArrearsChargeSubjectListController', ckFramework.ReportArrearsChargeSubjectListController);
});
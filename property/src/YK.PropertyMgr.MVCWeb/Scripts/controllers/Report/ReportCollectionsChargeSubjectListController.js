'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ReportCollectionsChargeSubjectListController = function ($http, $scope, $rootScope) {

        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ReportCollectionsChargeSubjectListData;
        $scope.ComDeptList = ckFramework.ReportCollectionsChargeSubjectListData.ReportDeptinfo;
        $scope.ComDeptId;

    };

    ckFramework.ReportCollectionsChargeSubjectListController.$inject = injectParams;
    app.register.controller('ReportCollectionsChargeSubjectListController', ckFramework.ReportCollectionsChargeSubjectListController);
});
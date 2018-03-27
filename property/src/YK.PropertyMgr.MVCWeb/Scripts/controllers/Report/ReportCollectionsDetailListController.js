'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ReportCollectionsDetailListController = function ($http, $scope, $rootScope) {

        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ReportCollectionsDetailListData;
        $scope.ComDeptList = ckFramework.ReportCollectionsDetailListData.ReportDeptinfo;
        $scope.ComDeptId;

    };

    ckFramework.ReportCollectionsDetailListController.$inject = injectParams;
    app.register.controller('ReportCollectionsDetailListController', ckFramework.ReportCollectionsDetailListController);
});
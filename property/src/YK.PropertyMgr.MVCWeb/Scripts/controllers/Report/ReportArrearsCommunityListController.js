'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ReportArrearsCommunityListController = function ($http, $scope, $rootScope) {

        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ReportArrearsCommunityListData;
        $scope.ComDeptList = ckFramework.ReportArrearsCommunityListData.ReportDeptinfo;
        $scope.ComDeptId;

    };

    ckFramework.ReportArrearsCommunityListController.$inject = injectParams;
    app.register.controller('ReportArrearsCommunityListController', ckFramework.ReportArrearsCommunityListController);
});
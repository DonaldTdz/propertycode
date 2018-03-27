'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ReportCollectionsCommunityListController = function ($http, $scope, $rootScope) {

        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.ReportCollectionsCommunityListData;
        $scope.ComDeptList = ckFramework.ReportCollectionsCommunityListData.ReportDeptinfo;
        $scope.ComDeptId;

    };

    ckFramework.ReportCollectionsCommunityListController.$inject = injectParams;
    app.register.controller('ReportCollectionsCommunityListController', ckFramework.ReportCollectionsCommunityListController);
});
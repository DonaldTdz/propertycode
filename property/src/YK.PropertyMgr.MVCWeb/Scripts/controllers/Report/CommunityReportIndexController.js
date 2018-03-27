'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/CommunityReportIndexService.js'], function (app) {

    var injectParams = ['$scope', 'CommunityReportIndexService'];

    ckFramework.CommunityReportIndexController = function ($scope, CommunityReportIndexService) {
        $scope.CommunityReportIndexData = ckFramework.CommunityReportIndexData;
        $scope.IsShowSearch = true;

    }

    ckFramework.CommunityReportIndexController.$inject = injectParams;

    app.register.controller('CommunityReportIndexController', ckFramework.CommunityReportIndexController);
});
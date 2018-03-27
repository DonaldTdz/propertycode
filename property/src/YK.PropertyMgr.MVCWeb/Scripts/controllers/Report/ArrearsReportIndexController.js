'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {

    var injectParams = ['$scope', 'ReportService'];

    ckFramework.ArrearsReportIndexController = function ($scope, ReportService) {
        $scope.ArrearsReportIndexData = ckFramework.ArrearsReportIndexData;
        $scope.IsShowSearch = true;
    }

    ckFramework.ArrearsReportIndexController.$inject = injectParams;

    app.register.controller('ArrearsReportIndexController', ckFramework.ArrearsReportIndexController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {

    var injectParams = ['$scope', 'ReportService'];

    ckFramework.TemporaryChargesReportIndexController = function ($scope, ReportService) {
        $scope.TemporaryChargesReportIndexData = ckFramework.TemporaryChargesReportIndexData;
        $scope.IsShowSearch = true;
    }

    ckFramework.TemporaryChargesReportIndexController.$inject = injectParams;

    app.register.controller('TemporaryChargesReportIndexController', ckFramework.TemporaryChargesReportIndexController);
});
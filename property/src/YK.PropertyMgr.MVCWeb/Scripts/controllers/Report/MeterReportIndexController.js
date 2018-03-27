'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {

    var injectParams = ['$scope', 'ReportService'];

    ckFramework.MeterReportIndexController = function ($scope) {
        //$scope.ArrearsReportIndexData = ckFramework.ArrearsReportIndexData;
        $scope.IsShowSearch = true;
    }

    ckFramework.MeterReportIndexController.$inject = injectParams;

    app.register.controller('MeterReportIndexController', ckFramework.MeterReportIndexController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {

    var injectParams = ['$scope', 'ReportService'];

    ckFramework.ExternalchargeIndexController = function ($scope) {
        //$scope.ArrearsReportIndexData = ckFramework.ArrearsReportIndexData;
        $scope.IsShowSearch = true;
    }

    ckFramework.ExternalchargeIndexController.$inject = injectParams;

    app.register.controller('ExternalchargeIndexController', ckFramework.ExternalchargeIndexController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/IntegratedReportContainerService.js'], function (app) {

    var injectParams = ['$scope', 'IntegratedReportContainerService'];

    ckFramework.IntegratedReportContainerController = function ($scope, IntegratedReportContainerService) {
        $scope.IntegratedReportContainerData = ckFramework.IntegratedReportContainerData;
        $scope.IsShowSearch = true;
    }

    ckFramework.IntegratedReportContainerController.$inject = injectParams;

    app.register.controller('IntegratedReportContainerController', ckFramework.IntegratedReportContainerController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportArrearsContainerService.js'], function (app) {

    var injectParams = ['$scope', 'ReportArrearsContainerService'];

    ckFramework.ReportArrearsContainerController = function ($scope, ReportArrearsContainerService) {
        $scope.ReportArrearsContainerData = ckFramework.ReportArrearsContainerData;
        $scope.IsShowSearch = true;
    }

    ckFramework.ReportArrearsContainerController.$inject = injectParams;

    app.register.controller('ReportArrearsContainerController', ckFramework.ReportArrearsContainerController);
});
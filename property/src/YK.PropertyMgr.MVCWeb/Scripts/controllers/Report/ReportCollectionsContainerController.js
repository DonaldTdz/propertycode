'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportCollectionsContainerService.js'], function (app) {

    var injectParams = ['$scope', 'ReportCollectionsContainerService'];

    ckFramework.ReportCollectionsContainerController = function ($scope, ReportCollectionsContainerService) {
        $scope.ReportCollectionsContainerData = ckFramework.ReportCollectionsContainerData;
        $scope.IsShowSearch = true;
    }

    ckFramework.ReportCollectionsContainerController.$inject = injectParams;

    app.register.controller('ReportCollectionsContainerController', ckFramework.ReportCollectionsContainerController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Dept/DeptContainerService.js'], function (app) {

    var injectParams = ['$scope','DeptContainerService'];

    ckFramework.DeptContainerController = function ($scope, DeptContainerService) {
        $scope.DeptContainerData = ckFramework.DeptContainerData;
    }

    ckFramework.DeptContainerController.$inject = injectParams;

    app.register.controller('DeptContainerController', ckFramework.DeptContainerController);
});
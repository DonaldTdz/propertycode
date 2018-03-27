'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/CarPark/CarParkContainerService.js'], function (app) {

    var injectParams = ['$scope', 'CarParkContainerService'];

    ckFramework.CarParkContainerController = function ($scope, CarParkContainerService) {
        $scope.CarParkContainerData = ckFramework.CarParkContainerData;
    }

    ckFramework.CarParkContainerController.$inject = injectParams;

    app.register.controller('CarParkContainerController', ckFramework.CarParkContainerController);
});
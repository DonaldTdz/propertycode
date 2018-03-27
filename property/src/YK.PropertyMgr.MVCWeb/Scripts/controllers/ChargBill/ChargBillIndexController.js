'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ChargBillIndexController = function ($http, $scope, $rootScope) {
        $scope.Testdata = "就是这里 Index";
     




    };

    ckFramework.ChargBillIndexController.$inject = injectParams;
    app.register.controller('ChargBillIndexController', ckFramework.ChargBillIndexController);
});
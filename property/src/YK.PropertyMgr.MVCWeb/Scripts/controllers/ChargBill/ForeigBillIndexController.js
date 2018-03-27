'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ForeigBillIndexController = function ($http, $scope, $rootScope) {
        $scope.Testdata = "就是这里 Index";
    };

    ckFramework.ForeigBillIndexController.$inject = injectParams;
    app.register.controller('ForeigBillIndexController', ckFramework.ForeigBillIndexController);
}); 
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.BatchGenerateBillIndexController = function ($http, $scope, $rootScope) {
        $scope.Testdata = "就是这里 Index";
    };

    ckFramework.BatchGenerateBillIndexController.$inject = injectParams;
    app.register.controller('BatchGenerateBillIndexController', ckFramework.BatchGenerateBillIndexController);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.DeveloperIndexController = function ($http, $scope, $rootScope) {
        $scope.Testdata = "就是这里 Index";
    };

    ckFramework.DeveloperIndexController.$inject = injectParams;
    app.register.controller('DeveloperIndexController', ckFramework.DeveloperIndexController);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.PrepayAccountViewController = function ($http, $scope, $rootScope) {
        $scope.IsShowSearch = true;
        $scope.BalanceAmount = ckFramework.PrepayAccountViewData.BalanceAmount;
    };

    ckFramework.PrepayAccountViewData.$inject = injectParams;
    app.register.controller('PrepayAccountViewController', ckFramework.PrepayAccountViewController);
});

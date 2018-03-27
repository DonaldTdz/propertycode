'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.PaymentTasksAddListService = function ($http, $q, $compile, $rootScope) {
        var PaymentTasksAddListService = {};




        return PaymentTasksAddListService;
    };

    ckFramework.PaymentTasksAddListService.$inject = injectParams;
    app.register.factory('PaymentTasksAddListService', ckFramework.PaymentTasksAddListService);
});
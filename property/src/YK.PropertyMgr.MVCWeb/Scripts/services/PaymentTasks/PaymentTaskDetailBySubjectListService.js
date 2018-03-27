'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.PaymentTaskDetailBySubjectListService = function ($http, $q, $compile, $rootScope) {
        var PaymentTaskDetailBySubjectListService = {};

        PaymentTaskDetailBySubjectListService.GetPaymentTaskSubjectDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/PaymentTasks/GetPaymentTaskSubjectList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        };


        return PaymentTaskDetailBySubjectListService;
    };

    ckFramework.PaymentTaskDetailBySubjectListService.$inject = injectParams;
    app.register.factory('PaymentTaskDetailBySubjectListService', ckFramework.PaymentTaskDetailBySubjectListService);
});
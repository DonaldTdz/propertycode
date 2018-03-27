'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.PaymentTaskDetailListService = function ($http, $q, $compile, $rootScope) {
        var PaymentTaskDetailListService = {};

        PaymentTaskDetailListService.GetPayTypeMthodDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/PaymentTasks/GetPaymentTaskPayMthodIdList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        };

        return PaymentTaskDetailListService;
    };

    ckFramework.PaymentTaskDetailListService.$inject = injectParams;
    app.register.factory('PaymentTaskDetailListService', ckFramework.PaymentTaskDetailListService);
}); 
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PaymentTasks/PaymentTaskDetailListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PaymentTaskDetailListService', '$http'];

    ckFramework.PaymentTaskDetailListController = function ($compile, $rootScope, $scope, PaymentTaskDetailListService, $http) {
        $scope.ChargeRecordListData = ckFramework.ChargeRecordListData;
  





    }

    ckFramework.PaymentTaskDetailListController.$inject = injectParams;

    app.register.controller('PaymentTaskDetailListController', ckFramework.PaymentTaskDetailListController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeRecord/ChargeRecordListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ChargeRecordListService'];
    ckFramework.BillChargeRecordViewController = function ($http, $scope, $rootScope, ChargeRecordListService) {
        $scope.IsShowSearch = true;
        $scope.BalanceAmount = ckFramework.BillChargeRecordViewData.BalanceAmount;
        $scope.RecordId = ckFramework.BillChargeRecordViewData.RecordId;
        $scope.ReceiptNum = ckFramework.BillChargeRecordViewData.ReceiptNum;
        $scope.PayInfoDesc = ckFramework.BillChargeRecordViewData.PayInfoDesc;
        $scope.DiscountInfoDesc = ckFramework.BillChargeRecordViewData.DiscountInfoDesc;

    };

    ckFramework.BillChargeRecordViewController.$inject = injectParams;
    app.register.controller('BillChargeRecordViewController', ckFramework.BillChargeRecordViewController);
});

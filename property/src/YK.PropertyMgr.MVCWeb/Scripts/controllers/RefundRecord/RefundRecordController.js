'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeRecord/ChargeRecordListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ChargeRecordListService'];
    ckFramework.RefundRecordController = function ($http, $scope, $rootScope, ChargeRecordListService) {
        $scope.FormData = ckFramework.RefundRecordData.RefundRecordInfo;
        $scope.IsBillDetail = ckFramework.RefundRecordData.PageType == 1? true : false;
        $scope.SaveData = function() {
            ChargeRecordListService.Refund($scope.FormData, function (data) {
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                ckFramework.ModalHelper.Alert(data.Msg);
            });
        };
    };

    ckFramework.RefundRecordController.$inject = injectParams;
    app.register.controller('RefundRecordController', ckFramework.RefundRecordController);
});

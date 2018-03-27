'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/DailyChargListService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'DailyChargListService'];
    ckFramework.DeleteBillViewController = function ($http, $window, $scope, $rootScope, DailyChargListService) {
        $scope.FormData = ckFramework.DeleteBillViewData.ChargBillInfo;
        
        $scope.DeleteBill = function () {
            DailyChargListService.DeleteBill($scope.FormData, ckFramework.DeleteBillViewData.DelteBillList, function (data) {
                if (data.IsSuccess) {

                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.DeleteBillData = data.Data;
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                else {
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.DeleteBillData = data.Data;
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        };
    };

    ckFramework.DeleteBillViewController.$inject = injectParams;
    app.register.controller('DeleteBillViewController', ckFramework.DeleteBillViewController);
});

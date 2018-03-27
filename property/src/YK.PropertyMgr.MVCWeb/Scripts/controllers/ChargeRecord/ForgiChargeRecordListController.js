'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeRecord/ChargeRecordListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ChargeRecordListService'];
    ckFramework.ForegiChargeRecordListController = function ($http, $scope, $rootScope, ChargeRecordListService) {
        $scope.IsShowSearch = true;
        $scope.IsDeveloper = $("#hidIsDeveloper").val();
        $scope.ShowRefundModal = function () {
            var ids = ckFramework.fnGetSelectdChargeRecordIds();
            if (ids == "") {
                ckFramework.ModalHelper.Alert("请选择收费记录");
                return;
            }
            if (ids.indexOf(',') > 0) {
                ckFramework.ModalHelper.Alert("退款不能批量操作");
                return;
            }
            var info = ckFramework.fnGetChargeRecordInfo(ids);
            if (info.ReceiptStatus == 2) {
                ckFramework.ModalHelper.Alert("该收费记录已退款");
                return;
            }
            //如果没有财务退款权限，只能退款临时收费
            if (!$rootScope.CheckPermission("FinanceRefund")) {
                if (info.ChargeType != 2) {
                    ckFramework.ModalHelper.Alert("你没有收费退款权限");
                    return;
                }
            }
            ChargeRecordListService.ShowForegiRefundModal(info);
        }
    
        $scope.ShowChargeRecordViewModal = function () {
            var ids = ckFramework.fnGetSelectdChargeRecordIds();
            if (ids == "") {
                ckFramework.ModalHelper.Alert("请选择收费记录");
                return;
            }
            if (ids.indexOf(',') > 0) {
                ckFramework.ModalHelper.Alert("只能选择一条收费记录");
                return;
            }
            var info = ckFramework.fnGetChargeRecordInfo(ids);
            info.ValidationType = 1;
            ChargeRecordListService.ShowForegiChargeRecordViewModal(info);
        }

        $scope.ShowReceiptPrintViewModal = function () {
            var ids = ckFramework.fnGetSelectdChargeRecordIds();
            if (ids == "") {
                ckFramework.ModalHelper.Alert("请选择收费记录");
                return;
            }
            if (ids.indexOf(',') > 0) {
                ckFramework.ModalHelper.Alert("只能选择一条收费记录");
                return;
            }
            var info = ckFramework.fnGetChargeRecordInfo(ids);
            if (info.ReceiptStatus == 2) {
                ckFramework.ModalHelper.Alert("已退款不能补打票据");
                return;
            }
            ChargeRecordListService.ShowForegiReceiptPrintViewModal(info);
        }

        $scope.ShowBillChargeRecordViewModal = function (id, rnum) {
            ChargeRecordListService.ShowBillChargeRecordViewModal($('#SelectDeptId').val(),id,rnum);
        }
    };

    ckFramework.ForegiChargeRecordListController.$inject = injectParams;
    app.register.controller('ForegiChargeRecordListController', ckFramework.ForegiChargeRecordListController);
});

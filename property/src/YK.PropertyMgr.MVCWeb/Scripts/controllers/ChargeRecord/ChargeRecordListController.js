'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeRecord/ChargeRecordListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ChargeRecordListService'];
    ckFramework.ChargeRecordListController = function ($http, $scope, $rootScope, ChargeRecordListService) {
        $scope.IsShowSearch = true;
        $scope.IsDeveloper = $("#hidIsDeveloper").val();

        if ($('#SelectDeptContainerType').val() == 14) {
            $scope.IsDeveloper = false;

        }


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
            if (info.ChargeType == 6) {
                ckFramework.ModalHelper.Alert("预存转移不能退款");
                return;
            }
            //alert(JSON.stringify(info))
            if (info.ReceiptStatus == 2) {
                ckFramework.ModalHelper.Alert("该收费记录已退款");
                return;
            }
            //如果没有财务退款权限，只能退款临时收费
            if (!$rootScope.CheckPermission("FinanceRefund")) {
                if (info.ChargeType != 2) {
                    ckFramework.ModalHelper.Alert("你没有日常收费退款权限");
                    return;
                }
            }

            ChargeRecordListService.ShowRefundModal(info);
        }
        $scope.SettleAccount = function () {
            var that = $('#table_ckFramework_ChargeRecordList tbody :checkbox:checked');
            if (that.length > 1) {
                ckFramework.ModalHelper.Alert("只能选择一条记录！");
                return false;
            }
            else if (that.length == 0) {
                ckFramework.ModalHelper.Alert("请选择收费记录！");
                return false;
            }
            else {
                if (that.attr("settleAccount") != "SettleAccount") {
                    if (that.attr("settleAccount") == 1) {

                        ckFramework.ModalHelper.Alert("当前记录不适用");
                    } else if (that.attr("settleAccount") == 3) {
                        ckFramework.ModalHelper.Alert("当前记录已生成预结算，不能重复生成");
                    }
                    return false;
                }
                var ChargeRecordId = that.attr("data");
                var url = "PropertyMgr/ChargeRecord/SettleAccount";
                ckFramework.ModalHelper.Confirm("确定要结算当前记录吗?", function () {
                    $.post(url, { ChargeRecordId: ChargeRecordId }, function (data) {
                        if (data.IsSuccess) {
                            ckFramework.ModalHelper.Alert(data.Msg);
                            $("#btnChargeRecordSearch").click();
                        } else {
                            ckFramework.ModalHelper.Alert(data.Msg);
                        }
                    });
                });
            }
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
            if (info.ChargeType == 6) {
                ckFramework.ModalHelper.Alert("预存转移不能修改");
                return;
            }
            info.ValidationType = 1;
            ChargeRecordListService.ShowChargeRecordViewModal(info);
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
            if (info.ChargeType == 6) {
                ckFramework.ModalHelper.Alert("预存转移不能补打票据");
                return;
            }
            if (info.ReceiptStatus == 2) {
                ckFramework.ModalHelper.Alert("已退款不能补打票据");
                return;
            }
            ChargeRecordListService.ShowReceiptPrintViewModal(info);
        }

        $scope.ShowBillChargeRecordViewModal = function (id, rnum) {
            ChargeRecordListService.ShowBillChargeRecordViewModal($('#SelectDeptId').val(), id, rnum);
        }
    };

    ckFramework.ChargeRecordListController.$inject = injectParams;
    app.register.controller('ChargeRecordListController', ckFramework.ChargeRecordListController);
});

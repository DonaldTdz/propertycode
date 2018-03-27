'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeRecord/ChargeRecordListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ChargeRecordListService'];
    ckFramework.BillDetailListController = function ($http, $scope, $rootScope, ChargeRecordListService) {
        $scope.ChargeSubjectList = ckFramework.BillDetailListData.ChargeSubjectList;
        //$scope.PayTypeList = ckFramework.BillDetailListData.PayTypeList;
        $scope.BillStatusList = ckFramework.BillDetailListData.BillStatusList;
        //$scope.ChargeTypeList = ckFramework.BillDetailListData.ChargeTypeList;
        $scope.IsShowSearch = true;
        $scope.BillStatus = 0;
        var currentdate = new Date();
        var y = currentdate.getFullYear();
        var m = currentdate.getMonth() + 1;
        var d = currentdate.getDate();
        var sdate = y + '-' + (m > 9 ? m : '0' + m) + '-01';
        var edate = y + '-' + (m > 9 ? m : '0' + m) + '-' + (d > 9 ? d : '0' + d);
        $scope.sm = { BillStatus: '', StartDate: sdate, EndDate: edate };

        $scope.GetChargeSubjectList = function () {
            $http({
                method: 'POST',
                url: '/PropertyMgr/BillDetail/GetSubjectList',
                data: {
                    DeptId:$('#SelectDeptId').val(),
                    DeptType: $('#SelectDeptType').val()
                }
            }).success(function (data) {
                $scope.ChargeSubjectList = data;
            });
        }

        $scope.GetChargeSubjectList();

        //退款
        $scope.ShowRefundModal = function () {
            var ids = ckFramework.fnGetSelectdBillDetailIds();
            if (ids == "") {
                ckFramework.ModalHelper.Alert("请选择记录");
                return;
            }
            if (ids.indexOf(',') > 0) {
                ckFramework.ModalHelper.Alert("退款不能批量操作");
                return;
            }
            var info = ckFramework.fnGetBillDetailInfo(ids);
            //alert(info.ReceiptStatus)
            if (info.ReceiptStatus == 2) {
                ckFramework.ModalHelper.Alert("该收费记录已退款");
                return;
            }
            if (info.ChargeType == 6) {
                ckFramework.ModalHelper.Alert("预存转移不能退款");
                return;
            }
            //if (info.BillStatus == 3) {
            //    ckFramework.ModalHelper.Alert("该记录已退款");
            //    return;
            //}
            //如果没有财务退款权限，只能退款临时收费
            if (!$rootScope.CheckPermission("FinanceRefund")) {
                if (info.ChargeType != 2) {
                    ckFramework.ModalHelper.Alert("你没有日常收费退款权限");
                    return;
                }
            }
            var cinfo = { Id: ids, PageType:1 };
            //对外收费
            if (info.ChargeType == 5) {
                ChargeRecordListService.ShowForegiRefundModal(cinfo);
            }
            else {
                ChargeRecordListService.ShowRefundModal(cinfo);
            } 
        }
        //修改
        $scope.ShowChargeRecordViewModal = function () {
            var ids = ckFramework.fnGetSelectdBillDetailIds();
            if (ids == "") {
                ckFramework.ModalHelper.Alert("请选择记录");
                return;
            }
            if (ids.indexOf(',') > 0) {
                ckFramework.ModalHelper.Alert("只能选择一条记录");
                return;
            }
            var info = ckFramework.fnGetBillDetailInfo(ids);
            var cinfo = { Id: ids, ValidationType: 1 };
            if (info.ChargeType == 6) {
                ckFramework.ModalHelper.Alert("预存转移不能修改");
                return;
            }
            //对外收费
            if (info.ChargeType == 5) {
                ChargeRecordListService.ShowForegiChargeRecordViewModal(cinfo);
            } else {
                ChargeRecordListService.ShowChargeRecordViewModal(cinfo);
            }
        }

        //票据补打
        $scope.ShowReceiptPrintViewModal = function () {
            var ids = ckFramework.fnGetSelectdBillDetailIds();
            if (ids == "") {
                ckFramework.ModalHelper.Alert("请选择记录");
                return;
            }
            if (ids.indexOf(',') > 0) {
                ckFramework.ModalHelper.Alert("只能选择一条记录");
                return;
            }
            var info = ckFramework.fnGetBillDetailInfo(ids);
            if (info.ReceiptStatus == 2) {
                ckFramework.ModalHelper.Alert("已退款不能补打票据");
                return;
            }
            if (info.ChargeType == 6) {
                ckFramework.ModalHelper.Alert("预存转移不能补打票据");
                return;
            }
            //if (info.BillStatus == 3) {
            //    ckFramework.ModalHelper.Alert("已退款不能补打票据");
            //    return;
            //}
           
            var cinfo = { Id: ids };
            //对外收费
            if (info.ChargeType == 5) {
                ChargeRecordListService.ShowForegiReceiptPrintViewModal(cinfo);
            }
            else {
                ChargeRecordListService.ShowReceiptPrintViewModal(cinfo);
            }
        }

        //生成预结算
        $scope.SettleAccount = function () {
            var ids = ckFramework.fnGetSelectdBillDetailIds();
            if (ids == "") {
                ckFramework.ModalHelper.Alert("请选择记录");
                return;
            }
            if (ids.indexOf(',') > 0) {
                ckFramework.ModalHelper.Alert("只能选择一条记录");
                return;
            }
            var info = ckFramework.fnGetBillDetailInfo(ids);
            if (info.AccountingStatus == 1) {
                ckFramework.ModalHelper.Alert("当前记录不适用");
                return;
            }
            if (info.AccountingStatus == 3) {
                ckFramework.ModalHelper.Alert("当前记录已生成预结算，不能重复生成");
                return;
            }

            var url = "PropertyMgr/ChargeRecord/SettleAccount";
            ckFramework.ModalHelper.Confirm("确定要结算当前记录吗?", function () {
                $.post(url, { ChargeRecordId: ids }, function (data) {
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.Alert(data.Msg);
                        $("#btnBillDetailSearch").click();
                    } else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            });
        }

        //导出
        $scope.ExportData = function () {
            var ComDeptId = $('#SelectDeptId').val();
            var ComDeptType = $('#SelectDeptType').val()
           
            if (ComDeptId == "undefined" || ComDeptId == null) {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            var parameters = "?DeptId=" + ComDeptId + "&DeptType=" + ComDeptType;
            if ($scope.sm.ResourcesName != null && $scope.sm.ResourcesName != "") {
                parameters += ("&ResourcesName=" + $scope.sm.ResourcesName);
            }
            if ($scope.sm.ReceiptNum != null && $scope.sm.ReceiptNum != "") {
                parameters += ("&ReceiptNum=" + $scope.sm.ReceiptNum);
            }
            if ($scope.sm.CustomerName != null && $scope.sm.CustomerName != "") {
                parameters += ("&CustomerName=" + $scope.sm.CustomerName);
            }
            if ($scope.sm.OperatorName != null && $scope.sm.OperatorName != "") {
                parameters += ("&OperatorName=" + $scope.sm.OperatorName);
            }
            if ($scope.sm.ChargeSubjectIdName != null && $scope.sm.ChargeSubjectIdName != "") {
                parameters += ("&ChargeSubjectIdName=" + $scope.sm.ChargeSubjectIdName);
            }
            $scope.sm.ChargeType = $("#divChargeType").find("select").val();
            //alert($scope.sm.ChargeType)
            if ($scope.sm.ChargeType != null && $scope.sm.ChargeType != "") {
                parameters += ("&ChargeType=" + $scope.sm.ChargeType);
            }
            if ($scope.sm.BillStatus != null && $scope.sm.BillStatus != "") {
                parameters += ("&BillStatus=" + $scope.sm.BillStatus);
            }
            $scope.sm.PayType = $("#divPayType").find("select").val();
            //alert($scope.sm.PayType)
            if ($scope.sm.PayType != null && $scope.sm.PayType != "") {
                parameters += ("&PayType=" + $scope.sm.PayType);
            }
            if ($scope.sm.StartDate != null && $scope.sm.StartDate != "") {
                parameters += ("&StartDate=" + $scope.sm.StartDate);
            }
            if ($scope.sm.EndDate != null && $scope.sm.EndDate != "") {
                parameters += ("&EndDate=" + $scope.sm.EndDate);
            }
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/BillDetail/ExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }

    };

    ckFramework.BillDetailListController.$inject = injectParams;
    app.register.controller('BillDetailListController', ckFramework.BillDetailListController);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.FullChargeRecordListController = function ($http, $scope, $rootScope) {
        $scope.IsShowSearch = true;
        var currentdate = new Date();
        var y = currentdate.getFullYear();
        var m = currentdate.getMonth() + 1;
        var d = currentdate.getDate();
        var sdate = y + '-' + (m > 9 ? m : '0' + m) + '-01';
        var edate = y + '-' + (m > 9 ? m : '0' + m) + '-' + (d > 9 ? d : '0' + d);
        $scope.sm = { BillStatus: '', StartDate: sdate, EndDate: edate };

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
            iframe.src = "PropertyMgr/ChargeRecord/ExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }
    };

    ckFramework.FullChargeRecordListController.$inject = injectParams;
    app.register.controller('FullChargeRecordListController', ckFramework.FullChargeRecordListController);
});

'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/ForeigBillChargeListService.js'], function (app) {
    var injectParams = ['$compile', '$rootScope', '$scope', '$http', 'ForeigBillChargeListService'];
    ckFramework.ForeigBillChargeViewController = function ($compile, $rootScope, $scope, $http, ForeigBillChargeListService) {
        $scope.ForeigBillChargeViewData = ckFramework.ForeigBillChargeViewData;
        $scope.PageViewModel = ckFramework.ForeigBillChargeViewData.PageViewModel;
        $scope.SubjectList = ckFramework.ForeigBillChargeViewData.SubjectList;
        $scope.CustomerNameRead = false;

        if (ckFramework.ForeigBillCustomerName.length > 0) {
            $scope.CustomerNameRead = true;
            $scope.PageViewModel.CustomerName = ckFramework.ForeigBillCustomerName;
        }
        else {
            $scope.CustomerNameRead = false;
        }



        $scope.Save = function () {
            var selectedId = $('#deptTree').jstree().get_selected() + "";
            var buildId = $("#deptTree").jstree().get_parent(selectedId) + "";
            var villageId = $("#deptTree").jstree().get_parent(buildId) + "";

            if (selectedId.split('_')[1] != 11) {
                ckFramework.ModalHelper.Alert("请选择小区信息!");
                return false;
            }
            var msg = $scope.CheckValue()
            if (msg.length == 0) {
                $scope.PageViewModel.BeginDate = $("input[name='BeginDate']").val();
                $scope.PageViewModel.EndDate = $("input[name='EndDate']").val();
                $scope.PageViewModel.ComVillageDeptId = selectedId.split('_')[0]

                ForeigBillChargeListService.Save($scope.PageViewModel, function (data) {
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.ForeigBillData = data.Data;
                    ckFramework.ModalHelper.CloseOpenModal1();
                });
            } else {
                ckFramework.ModalHelper.Alert(msg);
                return;
            }
        }
        $scope.CheckValue = function () {
            var msg = "";



            $scope.PageViewModel.CustomerName = $.trim($scope.PageViewModel.CustomerName);
            if ($scope.PageViewModel.CustomerName == null || $scope.PageViewModel.CustomerName.length <= 0) {

                msg = "请填写收费对象名称";
                return msg;
            }

            if ($scope.PageViewModel.SubjectId == null || $scope.PageViewModel.SubjectId.length <= 0) {

                msg = "请选择科目、若没有请添加一次性计费科目";
                return msg;
            }
            var beginDate = $scope.PageViewModel.BeginDate;
            var endDate = $scope.PageViewModel.EndDate;

            if (beginDate != null && beginDate != "" && beginDate.length != 0) {
                if (endDate == null || endDate.length == 0 || endDate == "") {
                    msg = "请填写结束时间";
                    return msg;
                }
                var beginCompare = new Date(beginDate)
                var endCompare = new Date(endDate)
                if (endCompare < beginCompare) {
                    msg = "结束时间不能小于初始时间";
                    return msg;
                }
            }
            else {
                msg = "请填写开始或者结束时间";
                return msg;
            }
            if (!($("input[name='Money']").val() > 0)) {
                msg = "请输入大于0的金额";
                return msg;
            }
            return msg;
        }
        //限制金额输入：保留小数点后两位
        $scope.toDecimal = function (event) {
            var txt = $(event.target);
            var val = ckFramework.toDecimal(txt.val());
            if (val)
                txt.val(val);
            else
                txt.val('');
        }
        $scope.isNumber = function (event) {
            var txt = $(event.target);
            if (isNaN(txt.val()))
                txt.val('');
        }


    };

    ckFramework.ForeigBillChargeViewController.$inject = injectParams;

    app.register.controller('ForeigBillChargeViewController', ckFramework.ForeigBillChargeViewController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/ForeigBillChargeListService.js'], function (app) {
    var injectParams = ['$compile', '$rootScope', '$scope', '$http', 'ForeigBillChargeListService', '$window'];
    ckFramework.ForeigBillChargeListController = function ($compile, $rootScope, $scope, $http, ForeigBillChargeListService, $window) {
        //$scope.ForeigBillChargeListData = ckFramework.ForeigBillChargeListData;
        $scope.IsShowSearch = false;
        $scope.PageModel = ckFramework.ForeigBillChargeListData.PageModel;
        $scope.PayTypeList = ckFramework.ForeigBillChargeListData.PayTypeList;
        $scope.ChargBillSearch = ckFramework.ForeigBillChargeListData.ChargBillSearch;
        $scope.ChargeBillList = ckFramework.ForeigBillChargeListData.ChargeBillList;
        //alert(JSON.stringify($scope.ChargeBillList))
        $scope.IsCheckedAll = true;
        $scope.PageModel.PayTypeId = String($scope.PageModel.PayTypeId);

        //查询账单
        $scope.GetForeigBillChargList = function () {
            //alert(99)
            $scope.ChargBillSearch.DeptType = $('#SelectDeptType').val();
            $scope.ChargBillSearch.DeptId = $('#SelectDeptId').val();
            $scope.ChargBillSearch.DeptName = $('#SelectDeptName').val();
            ckFramework.ModalHelper.OpenWait();
            ForeigBillChargeListService.GetForeigBillChargList($scope.ChargBillSearch, function (data) {
                //alert(JSON.stringify(data.ChargeBillList))
                ckFramework.ForeigBillCustomerName = "";
                $scope.ChargeBillList = data.ChargeBillList;
                $scope.PageModel = data.PageModel;
                $scope.PageModel.PayTypeId = String($scope.PageModel.PayTypeId);
                $scope.IsCheckedAll = true;
                ckFramework.ModalHelper.CloseWait();
            });
        }

        //checkbox 全选
        $scope.fnDailyCheckedAll = function () {
            $scope.ChargeBillList.forEach(function (item) {
                item.IsChecked = $scope.IsCheckedAll;
            });
            if ($scope.IsCheckedAll == true) {
                $scope.PageModel.AmountShouldTotal = $scope.PageModel.AmountShouldAllTotal;
            } else {
                $scope.PageModel.AmountShouldTotal = 0;
            }
            $scope.PageModel.ReceivedAmountTotal = Math.ceil($scope.PageModel.AmountShouldTotal);
        }

        //checkbox 单选
        $scope.fnDailyChecked = function (item) {
            if (item.IsChecked == true) {
                $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal + item.AmountShould) * 100) / 100;
            } else {
                $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal - item.AmountShould) * 100) / 100;
                $scope.IsCheckedAll = false;
            }
            $scope.PageModel.ReceivedAmountTotal = Math.ceil($scope.PageModel.AmountShouldTotal);
        }

        $scope.$watch('PageModel.ReceivedAmountTotal', function (newValue, oldValue) {
            var Result = $scope.PageModel.ReceivedAmountTotal - $scope.PageModel.AmountShouldTotal;
            if (isNaN(Result)) {
                $scope.PageModel.SmallChange = "";
            }
            else {
                $scope.PageModel.SmallChange = Math.round(Result * 100) / 100;
            }

            if (!$scope.$$phase) {
                $scope.$apply();
            }
        });

        $scope.$watch('PageModel.AmountShouldTotal', function (newValue, oldValue) {
            var Result = $scope.PageModel.ReceivedAmountTotal - $scope.PageModel.AmountShouldTotal;
            if (isNaN(Result)) {
                $scope.PageModel.SmallChange = "";
            }
            else {
                $scope.PageModel.SmallChange = Math.round(Result * 100) / 100;
            }
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        });

        $scope.SaveData = function () {
            $scope.PageModel.NewBillList = [];
            $scope.PageModel.BillIds = [];
            $scope.PageModel.CustomerName = ckFramework.ForeigBillCustomerName;
            $scope.ChargeBillList.forEach(function (item) {
                if (item.IsChecked == true) {
                    //获取新增临时账单
                    if (item.ActionStatus == 1 || item.ActionStatus == 2) {
                        item.BeginDate = item.BeginDateFormat;
                        item.EndDate = item.EndDateFormat;
                        $scope.PageModel.NewBillList.push(item);
                    }
                    else {
                        $scope.PageModel.BillIds.push(item.Id);
                    }
                }
            });
            if ($scope.PageModel.BillIds.length <= 0 && $scope.PageModel.NewBillList.length <= 0) {
                ckFramework.ModalHelper.Alert("请选择账单后再提交");
                return;
            }
            if ($scope.PageModel.PayTypeId == "") {
                ckFramework.ModalHelper.Alert("请选择付款方式");
                return;
            }


            ckFramework.ModalHelper.OpenWait();
            $http({
                method: 'POST',
                data: $scope.PageModel,
                url: '/PropertyMgr/ChargBill/PayForeigChargBill',
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.isRefresh = true;
                if (data.IsSuccess) {
                    //刷新账单
                    $scope.GetForeigBillChargList();
                    ckFramework.ModalHelper.CloseWait();
                    ckFramework.ModalHelper.Alert(data.Msg);
                    if (data.ErrorCode != "-5") {
                        //ckFramework.ModalHelper.Confirm(data.Msg + ",是否要打印票据", function () {
                        var href = ckFramework.ForeigBillPrintPDFChargeRecordUrl + "?chargeRecordId=" + data.Data;
                        //打印票据
                        $window.open(href, "_blank");
                        //});
                    }

                }
                else {
                    //ckFramework.ChargSubjectTable.draw();
                    ckFramework.ModalHelper.CloseWait();
                    ckFramework.ModalHelper.Alert(data.Msg);

                }
            });
        }

        $scope.fnGetSelectdChargBillIds = function () {
            var ids = [];
            $scope.ChargeBillList.forEach(function (item) {
                if (item.IsChecked == true) {
                    //获取新增临时账单
                    ids.push(item.Id);
                }
            });
            return ids;
        }

        $scope.fnGetSelectdChargBillInfo = function (id) {
            var info;
            $scope.ChargeBillList.forEach(function (item) {
                if (item.Id == id) {
                    info = item;
                }
            });
            return info;
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

        //显示添加临时账单视图
        $scope.ShowForeigBillViewModal = function () {
            var type = $("#SelectDeptType").val();
            if (type != DeptTypeInfos.XiaoQu) {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            ForeigBillChargeListService.ShowForeigBillViewModal($("#SelectDeptId").val(), function (data) {
                //alert(JSON.stringify(data))
                //默认选中
                data.IsChecked = true;
                data.HouseDoorNo = $("#SelectDeptName").val()
                //新添加放到第一个
                $scope.ChargeBillList.unshift(data);
                //触发选中事件
                $scope.fnDailyChecked(data);
                //更改全选总数

                $scope.PageModel.AmountShouldAllTotal = Math.round(($scope.PageModel.AmountShouldAllTotal + data.AmountShould) * 100) / 100;
            });
        }
    }

    ckFramework.ForeigBillChargeListController.$inject = injectParams;
    app.register.controller('ForeigBillChargeListController', ckFramework.ForeigBillChargeListController);
}); 
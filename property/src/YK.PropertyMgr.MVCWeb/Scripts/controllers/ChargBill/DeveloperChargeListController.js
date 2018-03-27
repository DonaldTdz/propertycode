'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/DeveloperChargeListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'DeveloperChargeListService', '$http', '$window'];

    ckFramework.DeveloperChargeListController = function ($compile, $rootScope, $scope, DeveloperChargeListService, $http,$window) {
        $scope.IsShowSearch = false;
        $scope.PageModel = ckFramework.DeveloperChargeListData.PageModel;
        $scope.PayTypeList = ckFramework.DeveloperChargeListData.PayTypeList;
        $scope.ChargeBillList = ckFramework.DeveloperChargeListData.ChargeBillList;
        $scope.ChargBillSearch = ckFramework.DeveloperChargeListData.ChargBillSearch;
        $scope.IsCheckedAll = true;
        $scope.PageModel.PayTypeId = String($scope.PageModel.PayTypeId);


        /*隐藏显示账单*/
        $scope.ShowDetailBill = function (m) {
            var myStyle =
            {
                "background-color": "#ededd2",
            }

            $scope.ChargeBillList.forEach(function (item) {

                if (item.GroupId == m.Id) {
                    item.CccordionClass = myStyle;
                    if (item.IsShow == false) {
                        item.IsShow = true;
                        m.CccordionClass = "fa fa-minus";
                    } else {
                        item.IsShow = false;
                        m.CccordionClass = "fa fa-plus";
                    }
                }
            })
        }

        //查询账单
        $scope.GetDailyChargList = function () {
            $scope.ChargBillSearch.DeptType = $('#SelectDeptType').val();
            $scope.ChargBillSearch.DeptId = $('#SelectDeptId').val();
            $scope.ChargBillSearch.DeptName = $('#SelectDeptName').val();
            ckFramework.ModalHelper.OpenWait();
            DeveloperChargeListService.GetDailyChargList($scope.ChargBillSearch, function (data) {
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

        //检查全选
        $scope.SetCheckedAll = function () {
            if ($scope.PageModel.AmountShouldTotal == $scope.PageModel.AmountShouldAllTotal) {
                $scope.IsCheckedAll = true;
            }
            else {
                $scope.IsCheckedAll = false;
            }
        }

        //检查组全选
        $scope.SetCheckedGroupAll = function (gid) {
            var group;
            var bo = true;
            $scope.ChargeBillList.forEach(function (m) {
                if (m.Id == gid && m.RowType == 1) {
                    group = m;
                }
                else if (m.GroupId == gid) {
                    if (m.IsChecked == false) {
                        bo = false;
                    }
                }
            });

            group.IsChecked = bo;
        }

        //checkbox 单选
        $scope.fnDailyChecked = function (item) {
            if (item.RowType == 1) {
                $scope.ChargeBillList.forEach(function (m) {
                    if (m.GroupId == item.Id && m.RowType == 2) {
                        if (item.IsChecked == false) {
                            $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal - m.AmountShould) * 100) / 100;
                            //$scope.IsCheckedAll = false;
                        } else {
                            if (m.IsChecked) {
                                $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal - m.AmountShould) * 100) / 100;
                            }
                            $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal + m.AmountShould) * 100) / 100;
                        }
                        m.IsChecked = item.IsChecked;
                    }
                });
            }
            else {


                if (item.IsChecked == true) {
                    $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal + item.AmountShould) * 100) / 100;
                } else {
                    $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal - item.AmountShould) * 100) / 100;
                    //$scope.IsCheckedAll = false;
                    //if (item.GroupId != "") {
                    //    /*找父节点IsChecked为false*/
                    //    $scope.ChargeBillList.forEach(function (m) {
                    //        if (m.Id == item.GroupId && m.RowType == 1) {
                    //            m.IsChecked = false;
                    //        }
                    //    });
                    //}
                }
                if (item.GroupId != null && item.GroupId != "") {
                    $scope.SetCheckedGroupAll(item.GroupId);
                }
            }
            $scope.PageModel.ReceivedAmountTotal = Math.ceil($scope.PageModel.AmountShouldTotal);
            $scope.SetCheckedAll();


            //if (item.IsChecked == true) {
            //    $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal + item.AmountShould) * 100) / 100;
            //} else {
            //    $scope.PageModel.AmountShouldTotal = Math.round(($scope.PageModel.AmountShouldTotal - item.AmountShould) * 100) / 100;
            //    $scope.IsCheckedAll = false;
            //}
            //$scope.PageModel.ReceivedAmountTotal = Math.ceil($scope.PageModel.AmountShouldTotal);
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
                url: '/PropertyMgr/ChargBill/PayChargBill',
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.isRefresh = true;
                if (data.IsSuccess) {
                    //刷新账单
                    $scope.GetDailyChargList();
                    ckFramework.ModalHelper.CloseWait();
                    ckFramework.ModalHelper.Alert(data.Msg);
                    if (data.ErrorCode != "-5")
                    {
                        //ckFramework.ModalHelper.Confirm(data.Msg + ",是否要打印票据", function () {
                        var href = ckFramework.printPDFChargeRecordUrl + "?chargeRecordId=" + data.Data;
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
    }

    ckFramework.DeveloperChargeListController.$inject = injectParams;

    app.register.controller('DeveloperChargeListController', ckFramework.DeveloperChargeListController);
});
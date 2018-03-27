'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/DailyChargListService.js'], function (app) {
    var injectParams = ['$compile', '$rootScope', '$scope', 'DailyChargListService', '$http', '$window'];
    ckFramework.DailyChargListController = function ($compile, $rootScope, $scope, DailyChargListService, $http, $window) {
        //$scope.DailyChargListData = ckFramework.DailyChargListData;
        $scope.ShowbtnPrepare = true;
        $scope.IsShowSearch = false;
        $scope.PageModel = ckFramework.DailyChargListData.PageModel;
        $scope.PayTypeList = ckFramework.DailyChargListData.PayTypeList;
        $scope.ChargeBillList = ckFramework.DailyChargListData.ChargeBillList;
        $scope.ChargBillSearch = ckFramework.DailyChargListData.ChargBillSearch;
        $scope.ChargeSubjectList = ckFramework.DailyChargListData.ChargeSubjectList;
        $scope.IsCheckedAll = true;
        $scope.PageModel.PayTypeId = String($scope.PageModel.PayTypeId);
        $scope.IsChargeSubject = ckFramework.DailyChargListData.IsChargeSubject;
        $scope.IsCarPark = ckFramework.DailyChargListData.IsCarPark;
        $scope.ComConfig = ckFramework.DailyChargListData.ComConfig;
        $scope.PreAccountList = ckFramework.DailyChargListData.PreAccountList;

        //查询账单
        $scope.GetDailyChargList = function () {
            $scope.ChargBillSearch.DeptType = $('#SelectDeptType').val();
            $scope.ChargBillSearch.DeptId = $('#SelectDeptId').val();
            $scope.ChargBillSearch.DeptName = $('#SelectDeptName').val();
            $scope.ChargBillSearch.IsCarPark = $scope.IsCarPark;
            ckFramework.ModalHelper.OpenWait();
            DailyChargListService.GetDailyChargList($scope.ChargBillSearch, function (data) {
                $scope.ChargeBillList = data.ChargeBillList;
                $scope.PageModel = data.PageModel;
                ckFramework.DailyChargListData.ChargeSubjectList = data.ChargeSubjectList;
                $scope.PageModel.PayTypeId = String($scope.PageModel.PayTypeId);
                $scope.ComConfig = data.ComConfig;
                $scope.PreAccountList = data.PreAccountList;
                $scope.IsCheckedAll = true;
                ckFramework.DailyChargListData.IsChargeSubject = data.IsChargeSubject;
                $scope.IsChargeSubject = ckFramework.DailyChargListData.IsChargeSubject;
                $scope.ChargeSubjectList = ckFramework.DailyChargListData.ChargeSubjectList;
                //v2.9 计算预存抵扣
                $scope.CalculationPreDeduction();
                //重新计算实收金额
                $scope.ReSetReceivedAmountTotal();
                ckFramework.ModalHelper.CloseWait();
            });
        }

        //获取选中账单
        $scope.GetCheckedChargBillList = function() {
            var cBills = [];
            $scope.ChargeBillList.forEach(function (item) {
                //排除组记录
                if (item.RowType == 2) {
                    if (item.IsChecked == true) {
                        cBills.push(item);
                    } 
                }
            });
            return cBills;
        }

        $scope.ReSetReceivedAmountTotal = function () {
            //实收金额 = 应收金额 - 预存抵扣金额
            $scope.PageModel.ReceivedAmountTotal = Math.ceil($scope.PageModel.AmountShouldTotal - $scope.PageModel.PreDeductibleAmount);
        }

        //v2.9 计算预存抵扣 2017-9-15
        $scope.CalculationPreDeduction = function () {
            //开发商代缴不能预存抵扣
            if ($scope.ChargBillSearch.IsDevPay) {
                $scope.PageModel.PreDeductibleAmount = 0.00;
                $scope.PageModel.IsPreDeductible = false;
                return;
            }
            if ($scope.PreAccountList == null || !$scope.PageModel.IsPreDeductible || $scope.PreAccountList == undefined || $scope.PreAccountList.length < 1) {
                $scope.PageModel.PreDeductibleAmount = 0.00;
                return;
            }
            //1.先获取选中账单
            var billList = $scope.GetCheckedChargBillList();
            var allSubjectAccount = null;
            var totalPreDeduction = 0.00;      //总预存抵扣
            //2.计算收费项目账户需要抵扣的金额
            $scope.PreAccountList.forEach(function (subAccount) {
                if (subAccount.SubjectId == 0) {
                    allSubjectAccount = subAccount;
                } else {
                    subAccount.DeductionAmount = 0.00;
                    subAccount.ActualDeductionAmount = 0.00;
                    billList.forEach(function (bill) {
                        if (bill.ChargeSubjectId == subAccount.SubjectId) {
                            subAccount.DeductionAmount = Math.round((subAccount.DeductionAmount + bill.AmountShould)*100)/100;
                        }
                    });
                    //如收费项目预存费 > 需要抵扣的账单金额
                    if (subAccount.PreAmount > subAccount.DeductionAmount) {
                        //实际抵扣金额 = 需要抵扣的账单金额
                        subAccount.ActualDeductionAmount = subAccount.DeductionAmount;
                    } else {
                        //实际抵扣金额 = 预存金额
                        subAccount.ActualDeductionAmount = subAccount.PreAmount;
                    }
                    //加到总的可抵扣
                    totalPreDeduction = Math.round((totalPreDeduction + subAccount.ActualDeductionAmount)*100)/100;
                }
            });

            //3.全部收费项目
            if (allSubjectAccount != null) {
                //计算总需要抵扣金额
                var totalDeductionAmount = 0.00;
                billList.forEach(function (bill) {
                    //排除预存费自身
                    if (bill.RefType != 4){
                        //加到总的需要抵扣
                        totalDeductionAmount = Math.round((totalDeductionAmount + bill.AmountShould)*100)/100;
                    }
                });

                //还需要抵扣 = 总的需要抵扣 - 收费项目账户已抵扣
                var stillAmount = Math.round((totalDeductionAmount - totalPreDeduction)*100)/100;
                allSubjectAccount.DeductionAmount = 0.00;
                allSubjectAccount.ActualDeductionAmount = 0.00;
                //如果 > 0 通过全部收费项目抵扣
                if (stillAmount > 0) {
                    allSubjectAccount.DeductionAmount = stillAmount;
                    if (allSubjectAccount.PreAmount > stillAmount) {
                        allSubjectAccount.ActualDeductionAmount = stillAmount;
                    } else {
                        allSubjectAccount.ActualDeductionAmount = allSubjectAccount.PreAmount;
                    }
                    //加到总的可抵扣
                    totalPreDeduction = Math.round((totalPreDeduction + allSubjectAccount.ActualDeductionAmount)*100)/100;
                }
            }
            //赋值给视图模板 总可预存抵扣
            $scope.PageModel.PreDeductibleAmount = totalPreDeduction;
        }

        $scope.InitDailyChargPage = function () {
            if ($('#SelectDeptContainerType').val() == 14) {
                $scope.ShowbtnPrepare = false;
            }

            //v2.9 计算预存抵扣
            $scope.CalculationPreDeduction();
            //重新计算实收金额
            $scope.ReSetReceivedAmountTotal();
        }

        $scope.InitDailyChargPage();

        /*隐藏显示账单*/
        $scope.ShowDetailBill = function (m) {
            var myStyle = { "background-color": "#ededd2" };
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

        //点击资源查询
        $scope.SearchDailyChargList = function () {
            $scope.ChargBillSearch.IsDevPay = false;
            $scope.GetDailyChargList();
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
            //v2.9 计算预存抵扣
            $scope.CalculationPreDeduction();
            $scope.ReSetReceivedAmountTotal();
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
        $scope.fnDailyChecked = function (item, obj) {
            //组账单
            if (item.RowType == 1) {
                //alert(1)
                var total = 0;
                $scope.ChargeBillList.forEach(function (m) {
                    if (m.GroupId == item.Id && m.RowType == 2) {
                        m.IsChecked = item.IsChecked;
                    }
                    if (m.IsChecked && m.RowType == 2) {
                        total = Math.round((total + m.AmountShould) * 100) / 100;
                    }
                });
                $scope.PageModel.AmountShouldTotal = total;
            }
                //组账单
            else {
                
                if (item.GroupId != null && item.GroupId != "") {
                    $scope.SetCheckedGroupAll(item.GroupId);
                }

                var total = 0;
                $scope.ChargeBillList.forEach(function (it) {
                    if (it.IsChecked && it.RowType == 2) {
                        total = Math.round((total + it.AmountShould) * 100) / 100;
                    }
                });

                $scope.PageModel.AmountShouldTotal = total;
            }
            //v2.9 计算预存抵扣
            $scope.CalculationPreDeduction();
            $scope.ReSetReceivedAmountTotal();
            $scope.SetCheckedAll();
        }

        //重置找零
        $scope.ReSetSmallChange = function () {
            //找零 = 实收金额 + 预存抵扣 - 应收金额
            var Result = (Math.round($scope.PageModel.ReceivedAmountTotal * 100) / 100 + Math.round($scope.PageModel.PreDeductibleAmount * 100) / 100 - Math.round($scope.PageModel.AmountShouldTotal*100)/100);
            //alert(Result)
            if (isNaN(Result)) {
                $scope.PageModel.SmallChange = "";
            }
            else {
                $scope.PageModel.SmallChange = (Math.round(Result * 100) / 100);
            }
        }

        //实收金额变化
        $scope.$watch('PageModel.ReceivedAmountTotal', function (newValue, oldValue) {
            $scope.ReSetSmallChange();
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        });
        //应收金额变化
        $scope.$watch('PageModel.AmountShouldTotal', function (newValue, oldValue) {
            $scope.ReSetSmallChange();
            if (!$scope.$$phase) {
                $scope.$apply();
            }
        });

        $scope.$watch('PageModel.IsPreDeductible', function (newValue, oldValue) {
            if (newValue) {
                //v2.9 计算预存抵扣
                $scope.CalculationPreDeduction();
                
            } else {
                $scope.PageModel.PreDeductibleAmount = 0;
            }
            //重新计算实际支付
            $scope.ReSetReceivedAmountTotal();
            $scope.ReSetSmallChange();
        });

        $scope.AddRefIdNoExists = function (refId, refIdArr) {
            var flag = false;
            refIdArr.forEach(function (item) {
                if (refId == item) {
                    flag = true;
                }
            });

            if (flag == false) {
                refIdArr.push(refId);
            }
        }

        $scope.SaveData = function () {
            $scope.PageModel.NewBillList = [];
            $scope.PageModel.BillIds = [];
            var checkedBills = [];
            var noCheckedBills = [];

            $scope.ChargeBillList.forEach(function (item) {
                //排除组记录
                if (item.RowType == 2) {
                    if (item.IsChecked == true) {
                        checkedBills.push(item);
                    } else {
                        noCheckedBills.push(item);
                    }
                }
            });

            var refIdArr = [];
            checkedBills.forEach(function (item) {
                //获取新增临时账单
                if (item.ActionStatus == 1) {
                    item.BeginDate = item.BeginDateFormat;
                    item.EndDate = item.EndDateFormat;
                    $scope.PageModel.NewBillList.push(item);
                    if (item.RefId != null && item.RefId != "") {
                        $scope.AddRefIdNoExists(item.RefId, refIdArr);
                    }
                }
                else if (item.ActionStatus == 2) {
                    item.BeginDate = item.BeginDateFormat;
                    item.EndDate = item.EndDateFormat;
                    $scope.PageModel.NewBillList.push(item);
                    $scope.AddRefIdNoExists(item.Id, refIdArr);
                }
                else {
                    $scope.PageModel.BillIds.push(item.Id);
                }
            });

            refIdArr.forEach(function (rid) {
                noCheckedBills.forEach(function (citem) {
                    if (citem.RefId == rid || citem.Id == rid) {
                        citem.BeginDate = citem.BeginDateFormat;
                        citem.EndDate = citem.EndDateFormat;
                        $scope.PageModel.NewBillList.push(citem);
                    }
                });
            });

            if ($scope.PageModel.BillIds.length <= 0 && $scope.PageModel.NewBillList.length <= 0) {
                ckFramework.ModalHelper.Alert("请选择账单后再提交");
                return;
            }
            if ($scope.PageModel.PayTypeId == "") {
                ckFramework.ModalHelper.Alert("请选择付款方式");
                return;
            }
            //弹出确认框 2017-09-12
            if ($scope.ComConfig.IsChargeConfirm) {
                var confirmData = {};
                confirmData.UserName = $("#CBI_OwnerName").text();
                confirmData.ReceiptNo = '';
                confirmData.IsPrintReceipt = $scope.ComConfig.IsDefaultPrintReceipt;
                confirmData.PayAmount = $scope.PageModel.ReceivedAmountTotal;
                //如果收金额为0 则不需要打印票据
                if (confirmData.PayAmount == 0) {
                    confirmData.IsPrintReceipt = false;
                }
                confirmData.PreDeductibleAmount = $scope.PageModel.PreDeductibleAmount;
                confirmData.SmallChange = $scope.PageModel.SmallChange;
                confirmData.SubjectDesc = "预存至" + $("#selectSmallToPrepay").find("option:selected").text();
                confirmData.ChargeBillList = checkedBills;
                DailyChargListService.ShowConfirmViewModal(confirmData, function (result) {
                    //alert(JSON.stringify(result));
                    //用户确定 提交付款
                    if (result.YN == "Y") {
                        $scope.fnPayAmount(result.IsPrintReceipt, result.ReceiptNo);
                    }
                });
            } else {
                $scope.fnPayAmount($scope.ComConfig.IsDefaultPrintReceipt, '');
            }  
        }

        //缴费
        $scope.fnPayAmount = function (isPrintReceipt, printNum) {
            ckFramework.ModalHelper.OpenWait();
            $scope.PageModel.IsPrintReceipt = isPrintReceipt;
            $scope.PageModel.ReceiptNo = printNum;
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
                    if (data.ErrorCode != "-5") {
                        //个性化配置获取 2017-09-12
                        if (isPrintReceipt) {
                            //ckFramework.ModalHelper.Confirm(data.Msg + ",是否要打印票据", function () {
                            var href = ckFramework.printPDFChargeRecordUrl + "?chargeRecordId=" + data.Data;
                            //打印票据
                            $window.open(href, "_blank");
                        }
                    }
                }
                else {
                    //ckFramework.ChargSubjectTable.draw();
                    ckFramework.ModalHelper.CloseWait();
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        }

        /*****/
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

        //add by donald 2016-9-22
        $scope.ShowSplitBillView = function () {
            var myStyle = {
                "background-color": "#ededd2",
            }

            var ids = $scope.fnGetSelectdChargBillIds();
            if (ids.length < 1) {
                ckFramework.ModalHelper.Alert("请选择账单");
                return;
            }
            //alert(ids)
            if (ids.length > 1) {
                ckFramework.ModalHelper.Alert("拆分账单不能批量操作");
                return;
            }
            var info = $scope.fnGetSelectdChargBillInfo(ids[0]);
            //alert(JSON.stringify(info))
            info.BeginDate = info.BeginDateFormat;
            info.EndDate = info.EndDateFormat;
        
            $http({
                method: 'POST',
                url: '/PropertyMgr/ChargBill/CheckSplitBill',
                data: {
                    bill: info
                }
            }).success(function (data) {

                // alert(JSON.stringify(data));
                if (data.IsSuccess) {
                    DailyChargListService.ShowSplitBillViewModal(info, function (data) {
                        //默认选中
                        //alert(JSON.stringify(data.Bill))
                        //添加新账单
                        //  $scope.ChargeBillList.unshift(data.NewBill);
                        //删除再添加原来账单 更新

                        for (var i = 0; i < $scope.ChargeBillList.length; i++) {
                            if ($scope.ChargeBillList[i].Id == data.Bill.Id) {
                                // $scope.ChargeBillList.splice(i, 1);
                                data.NewBill.IsShow = true;
                                data.Bill.IsShow = data.NewBill.IsShow;

                                data.NewBill.GroupId = $scope.ChargeBillList[i].GroupId;
                                data.Bill.GroupId = data.NewBill.GroupId;

                                data.Bill.RowType = 2;
                                data.NewBill.RowType = 2

                                $scope.ChargeBillList[i] = data.NewBill;
                                /*删除0个元素，新增一个元素在位置i后*/
                                $scope.ChargeBillList.splice(i, 0, data.Bill);
                                $scope.ChargeBillList.forEach(function (item) {
                                    if (item.GroupId != null && item.GroupId == data.Bill.GroupId) {
                                        item.CccordionClass = myStyle;
                                    }
                                })
                            }
                        }
                        //    $scope.ChargeBillList.unshift(data.Bill);
                    });
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }

            })

        }

        $scope.RefreshBill = function () {
            var type = $("#SelectDeptType").val();
            if (type != DeptTypeInfos.FangWu) {
                ckFramework.ModalHelper.Alert("请选择房屋");
                return;
            }
            ckFramework.ModalHelper.Confirm("确定要生成本月账单吗？成功后将会刷新页面", function () {
                var hid = $("#SelectDeptId").val();
                DailyChargListService.RefreshBill(hid, function (data) {
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.Alert(data.Msg, 3000);//弹出提示信息,3秒后关闭
                        $scope.GetDailyChargList();
                        $scope.IsCheckedAll = true;
                        $scope.fnDailyCheckedAll();
                        //ckFramework.ChargSubjectTable.draw();
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            });
        }

        $scope.ShowPrepareAmountViewModal = function () {
            var type = $("#SelectDeptType").val();
            if (type != DeptTypeInfos.FangWu) {
                ckFramework.ModalHelper.Alert("请选择房屋");
                return;
            }
            DailyChargListService.ShowPrepareAmountViewModal($("#SelectDeptId").val(), $('#SelectDeptName').val(), function (data) {
                //alert(JSON.stringify(data))
                //默认选中
                data.IsChecked = true;
                //新添加放到第一个
                $scope.ChargeBillList.unshift(data);
                //更改全选总数
                $scope.PageModel.AmountShouldAllTotal = Math.round(($scope.PageModel.AmountShouldAllTotal + data.AmountShould) * 100) / 100;
                //触发选中事件
                $scope.fnDailyChecked(data);
            });
        }

        $scope.ShowDeleteBillView = function () {
            var ids = $scope.fnGetSelectdChargBillIds();
            if (ids.length < 1) {
                ckFramework.ModalHelper.Alert("请选择账单");
                return;
            }
            //alert(ids)
            // if (ids.length > 1) {
            //     ckFramework.ModalHelper.Alert("账单作废不能批量操作");
            //     return;
            // }

            /***********找到数据*************/
            $scope.PageModel.NewBillList = [];
            $scope.PageModel.BillIds = [];
            var checkedBills = [];
            var noCheckedBills = [];

            $scope.ChargeBillList.forEach(function (item) {
                //排除组记录
                if (item.RowType == 2) {
                    if (item.IsChecked == true) {
                        checkedBills.push(item);
                    } else {
                        noCheckedBills.push(item);
                    }
                }
            });

            var refIdArr = [];
            checkedBills.forEach(function (item) {
                //获取新增临时账单
                if (item.ActionStatus == 1) {
                    item.BeginDate = item.BeginDateFormat;
                    item.EndDate = item.EndDateFormat;
                    $scope.PageModel.NewBillList.push(item);
                    if (item.RefId != null && item.RefId != "") {
                        $scope.AddRefIdNoExists(item.RefId, refIdArr);
                    }
                }
                else if (item.ActionStatus == 2) {
                    item.BeginDate = item.BeginDateFormat;
                    item.EndDate = item.EndDateFormat;
                    $scope.PageModel.NewBillList.push(item);
                    $scope.AddRefIdNoExists(item.Id, refIdArr);
                }
                else {
                    $scope.PageModel.BillIds.push(item.Id);
                }
            });

            refIdArr.forEach(function (rid) {
                noCheckedBills.forEach(function (citem) {
                    if (citem.RefId == rid || citem.Id == rid) {
                        citem.BeginDate = citem.BeginDateFormat;
                        citem.EndDate = citem.EndDateFormat;
                        $scope.PageModel.NewBillList.push(citem);
                    }
                });
            });
            /*************************/
            ckFramework.ModalHelper.DeleteModel = $scope.PageModel;
            var DeleteCheckedBills = [];
            ids.forEach(function(idsobj)
            {
                var info1 = $scope.fnGetSelectdChargBillInfo(idsobj);
                info1.BeginDate = info1.BeginDateFormat;
                info1.EndDate = info1.EndDateFormat;
                DeleteCheckedBills.push(info1);
            }); 
            //var info = $scope.fnGetSelectdChargBillInfo(ids[0]);
         
            //info.BeginDate = info.BeginDateFormat;
            //info.EndDate = info.EndDateFormat;

            $http({
                method: 'POST',
                url: '/PropertyMgr/ChargBill/CheckDeleteBill',
                data: {
                    bill: DeleteCheckedBills,
                    deleteModel: ckFramework.ModalHelper.DeleteModel
                }
            }).success(function (data) {

                if (data.IsSuccess) {
                    DailyChargListService.ShowDeleteBillViewModal(DeleteCheckedBills, function (data) {
                        $scope.SearchDailyChargList();
                    });
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            })
        }

        $scope.ShowManuallyGenerateBillView = function () {
            var type = $("#SelectDeptType").val();
            if (type != DeptTypeInfos.FangWu && type != DeptTypeInfos.CheWei) {
                ckFramework.ModalHelper.Alert("请选择房屋或者车位");
                return;
            }

            if ($scope.IsChargeSubject == false) {
                ckFramework.ModalHelper.Alert("没有需要手动生成的收费项目");
                return;
            }

            var hid = $("#SelectDeptId").val();
            DailyChargListService.ShowManuallyGenerateBillViewModal(hid, function (data) {
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.Alert(data.Msg, 3000);//弹出提示信息,3秒后关闭
                    $scope.GetDailyChargList();
                    //$scope.IsCheckedAll = true;
                    //$scope.fnDailyCheckedAll();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
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

    ckFramework.DailyChargListController.$inject = injectParams;

    app.register.controller('DailyChargListController', ckFramework.DailyChargListController);
});
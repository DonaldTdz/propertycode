'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/DailyChargListService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'DailyChargListService'];
    ckFramework.ConfirmViewController = function ($http, $window, $scope, $rootScope, DailyChargListService) {
        $scope.ConfirmData = ckFramework.ModalHelper.BillConfirmData;
        $scope.IsReadonlyReceiptNo = false;
        $scope.NoReceiptNo = false;
        $scope.ShowChargeBillList = [];

        //从新设置显示账单
        $scope.ReSetChargeBillList = function () {
            var temp = null;
            $scope.ConfirmData.ChargeBillList.forEach(function (item) {
                if (temp == null) {
                    temp = item;
                } else {
                    var edate = new Date();
                    edate.setDate(temp.EndDate + 1);
                    alert(temp.EndDate)
                    alert(edate)
                    //同一个收费项目 且 账单结束日期+1 = 下个账单开始日期
                    if (temp.ChargeSubjectId == item.ChargeSubjectId && edate == item.BeginDate) {
                        temp.BillAmount = Math.round((temp.BillAmount + item.BillAmount) * 100) / 100;//账单金额
                        temp.ReceivedAmount = Math.round((temp.ReceivedAmount + item.ReceivedAmount) * 100) / 100;//已收金额
                        temp.AmountShould = Math.round((temp.AmountShould + item.AmountShould) * 100) / 100;//应收金额
                        temp.ReliefAmount = Math.round((temp.ReliefAmount + item.ReliefAmount) * 100) / 100;//优惠金额
                        temp.EndDate = item.EndDate;
                        temp.EndDateFormat = item.EndDateFormat;
                        temp.Remark = '';//
                    } else {
                        var sitem = angular.copy(temp);
                        $scope.ShowChargeBillList.push(sitem);
                        temp = item;
                    }
                }
            });
            if (temp != null){
                $scope.ShowChargeBillList.push(temp);
            } 
        }

        $scope.InitConfirmPage = function () {
            //实收金额为0 不需要打印票据
            if ($scope.ConfirmData.PayAmount == 0) {
                $scope.ConfirmData.IsPrintReceipt = false;
                $scope.ConfirmData.ReceiptNo = '';
                $scope.NoReceiptNo = true;
                $scope.NoReceiptMsg = '实收金额为零不可打印票据';
            }
            //如果是打印票据号 和 生成的票据号 为空
            else if ($scope.ConfirmData.IsPrintReceipt && $scope.ConfirmData.ReceiptNo == '') {
                $scope.ConfirmData.IsPrintReceipt = false;
                $scope.NoReceiptNo = true;
                $scope.NoReceiptMsg = '没有可用的票据号';
            }

            $scope.ReSetChargeBillList();
        }

        $scope.InitConfirmPage();

        $scope.ConfirmBill = function () {
            if ($scope.ConfirmData.IsPrintReceipt && $scope.ConfirmData.ReceiptNo == '') {
                ckFramework.ModalHelper.Alert('打印的票据号不能为空');
                return;
            }
            ckFramework.ModalHelper.isRefresh = true;
            ckFramework.ModalHelper.ConfirmResult = {};
            ckFramework.ModalHelper.ConfirmResult.YN = 'Y';
            ckFramework.ModalHelper.ConfirmResult.IsPrintReceipt = $scope.ConfirmData.IsPrintReceipt;
            ckFramework.ModalHelper.ConfirmResult.ReceiptNo = $scope.ConfirmData.ReceiptNo;
            ckFramework.ModalHelper.CloseOpenModal1();
        }

        $scope.GetReceiptBookNumber = function () {
            ckFramework.ModalHelper.OpenWait();
            DailyChargListService.GetReceiptBookNumber(function (receiptNo) {
                $scope.ConfirmData.ReceiptNo = receiptNo;
                if (receiptNo == '') {
                    $scope.IsReadonlyReceiptNo = true;
                    $scope.NoReceiptNo = true;
                    $scope.NoReceiptMsg = '没有可用的票据号';
                    $scope.ConfirmData.IsPrintReceipt = false;
                } else {
                    $scope.IsReadonlyReceiptNo = false;
                    $scope.NoReceiptNo = false;
                }
                ckFramework.ModalHelper.CloseWait();
            });
        }

        $scope.$watch('ConfirmData.IsPrintReceipt', function (newValue, oldValue) {
            //打印票据
            if (newValue) {
                //获取票据号
                $scope.GetReceiptBookNumber();
            } else {
                $scope.ConfirmData.ReceiptNo = '';
                $scope.IsReadonlyReceiptNo = true;
            }
        });
    };

    ckFramework.ConfirmViewController.$inject = injectParams;
    app.register.controller('ConfirmViewController', ckFramework.ConfirmViewController);
});

'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/DailyChargListService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'DailyChargListService'];
    ckFramework.PrepareAmountViewController = function ($http, $window, $scope, $rootScope, DailyChargListService) {
        $scope.FormData = ckFramework.PrepareAmountViewData.ChargBillInfo;
        $scope.MonthPreAmount = ckFramework.PrepareAmountViewData.MonthPreAmount;
        $scope.MonthPrePaymentList = ckFramework.PrepareAmountViewData.MonthPrePaymentList;
        $scope.IsDepPay = ckFramework.PrepareAmountViewData.IsDepPay;
        $scope.TempBeginDate = ckFramework.PrepareAmountViewData.ChargBillInfo.BeginDateFormat;
        //if ($scope.IsDepPay) {
        //    $scope.FormData.BeginDateFormat = "";
        //    $scope.FormData.EndDateFormat = "";
        //}
        //设置账单结束日期
        $scope.SetEndDate = function () {
            if ($scope.IsDepPay) {
                return;
            }
            if ($scope.FormData.Months != "") {
                $scope.FormData.BeginDateFormat = $scope.TempBeginDate;
                var months = Math.floor($scope.FormData.Months);
                var flag = false;
                //表示整月
                if ($scope.FormData.Months == months) {
                    months = months - 1;
                    flag = true;
                }
                var eDate = new Date($scope.FormData.BeginDateFormat);
                //alert(months)
                //设置年月
                if (months > 0) {
                    var year = Math.floor((months + eDate.getMonth() + 1) / 12);
                    if (isNaN(year)) {
                        year = 0;
                    }
                    //alert(year)
                    if (year > 0) {
                        var years = eDate.getFullYear() + year;
                        eDate.setFullYear(years);
                    }
                    var month = (months + eDate.getMonth() + 1) - (year * 12) - 1;
                    if (isNaN(month)) {
                        month = 0;
                    }
                    //alert(month)
                    if (month > -1) {
                        eDate.setMonth(month);
                    } else if (month == -1) {
                        //-1 时减去一个月 = 上一年12月
                        eDate.setFullYear(eDate.getFullYear() - 1);
                        eDate.setMonth(11);
                    }
                }
                //设置日
                if (flag) {
                    var m = eDate.getMonth() + 1;
                    if (m == 1 || m == 3 || m == 5 || m == 7 || m == 8 || m == 10 || m == 12) {
                        eDate.setDate(31);
                    }
                    else if (m == 2) {
                        eDate.setDate(28);
                    }
                    else {
                        eDate.setDate(30);
                    }
                }
                else {
                    var day = Math.floor(($scope.FormData.Months - months) * 30);
                    if (isNaN(day)) {
                        day = 0;
                    }
                    if (day > 0) {
                        //如果是二月
                        if (eDate.getMonth() == 2 && day > 28) {
                            day = 28;
                        }
                        eDate.setDate(day);
                    }
                }
                //alert(eDate);
                var em = eDate.getMonth() + 1;
                var ed = eDate.getDate();
                $scope.FormData.EndDateFormat = eDate.getFullYear() + "-" + (em > 9 ? em : "0" + em) + "-" + (ed > 9 ? ed : "0" + ed);
            } else {
                $scope.FormData.BeginDateFormat = "";
                $scope.FormData.EndDateFormat = "";
            }
        }
        //监控月份
        $scope.MonthsChange = function () {
            var Result = parseFloat($scope.FormData.Months * $scope.MonthPreAmount).toFixed(2);
            if (isNaN(Result)) {
                $scope.FormData.BillAmount = "";
            }
            else {
                $scope.FormData.BillAmount = Result;
            }
            $('#' + ckFramework.createPrepareAmountViewFormData.FormId).formValidation('revalidateField', 'BillAmount');
            //$scope.SetEndDate();
        }
        ////监控金额
        $scope.BillAmountChange = function () {
            if ($scope.MonthPreAmount == null) {
                $scope.MonthPreAmount = 0;
            }
            if (!($scope.IsDepPay || $scope.MonthPreAmount == 0)) {
                var Result = parseFloat($scope.FormData.BillAmount / $scope.MonthPreAmount).toFixed(2);
                //alert(Result)
                if (isNaN(Result)) {
                    $scope.FormData.Months = "";
                }
                else {
                    $scope.FormData.Months = Result;
                }
            }
            else {
                $scope.FormData.Months = "";
            }

            $('#' + ckFramework.createPrepareAmountViewFormData.FormId).formValidation('revalidateField', 'Months');

            //$scope.SetEndDate();
        };
        $scope.PrepareAmount = function () {
            $scope.FormData.BeginDate = $scope.FormData.BeginDateFormat;
            $scope.FormData.EndDate = $scope.FormData.EndDateFormat;
            DailyChargListService.PrepareAmount($scope.FormData, function (data) {
                if (data.IsSuccess) {
                    //ckFramework.ModalHelper.Alert(data.Msg);
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.PrepareBillData = data.Data;
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        };

        $scope.SubjectChange = function () {
            $scope.MonthPrePaymentList.forEach(function (item) {
                if (item.SubjectId == $scope.FormData.PreChargeSubjectId) {
                    $scope.MonthPreAmount = item.PreAmount;
                    return;
                }
            });

            $scope.MonthsChange();
        }
    };

    ckFramework.PrepareAmountViewController.$inject = injectParams;
    app.register.controller('PrepareAmountViewController', ckFramework.PrepareAmountViewController);
});

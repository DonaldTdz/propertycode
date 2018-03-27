'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeSubject/ChargeSubjectViewService.js'], function (app) {
    var injectParams = ['$scope', 'ChargeSubjectViewService'];
    ckFramework.ChargeSubjectViewController = function ($scope, ChargeSubjectViewService) {
        $scope.ClientMessage = ckFramework.ClientMessage.GetMessage();
        $scope.ChargeSubjectViewData = ckFramework.ChargeSubjectViewData;
        $scope.IsAutomatic = [{ "Id": 0, "Name": "否" }, { "Id": 1, "Name": "是" }];/*是否生成自动账单*/
        $scope.FormData = ckFramework.ChargeSubjectViewData.ChargeSubject;
        $scope.BillBillPeriodList = ckFramework.ChargeSubjectViewData.BillBillPeriodList;
        $scope.SubjectTypeListInit = ckFramework.ChargeSubjectViewData.SubjectTypeList;
        $scope.SubjectTypeList = ckFramework.ChargeSubjectViewData.SubjectTypeList;
        $scope.IsReadonlySubjectType = false;
        $scope.BillDayAutomaticBill = false;

        $scope.IsAutoBillDrop = ckFramework.ChargeSubjectViewData.ChargeSubject.IsAutoBillDrop
        if (!($scope.FormData.Id > 0)) {
            $scope.FormData.AutomaticBill = 1;/*默认为是*/;
        } else {
            if ($scope.FormData.AutomaticBill == 0) {/*否*/
                $scope.BillDayDisabled = true;
                $scope.BillDay = null;
            }
        }

        $scope.AutomaticBillChange = function (val) {
            if (val == 0) {
                $scope.FormData.BillDay = null;
                $scope.BillDayDisabled = true;
            } else {
                $scope.FormData.BillDay = 1;
                $scope.BillDayDisabled = false;
            }
        }

        if (!(ckFramework.ChargeSubjectViewData.ChargeSubject.Id > 0)) {
            $scope.FormData.BillDay = 1
        } else {
            $scope.FormData.BeginDate = ckFramework.ChargeSubjectViewData.ChargeSubject.BeginDate.substr(0, 10);
        }

        if ($scope.FormData.BillPeriod == 1 || $scope.FormData.BillPeriod == 2) {
            var arr = new Array();
            var typeData = angular.copy($scope.SubjectTypeListInit);
            for (var i = 0; i < typeData.length; i++) {
                if (typeData[i].Code != "1" && typeData[i].Code != "2") {
                    arr.push(typeData[i].Code);
                }
            }
            for (var i = 0; i < arr.length; i++) {
                for (var m = 0; m < typeData.length; m++) {
                    if (typeData[m].Code == arr[i]) {
                        typeData.splice(m, 1);
                    }
                }
            }
        }

        if ($scope.FormData.BillPeriod == 3) {//一次性收费
            /*排除三表*/
            var sbtype = angular.copy($scope.SubjectTypeListInit);
            for (var i = 0; i < sbtype.length; i++) {
                if (sbtype[i].Code == '3') {
                    sbtype.splice(i, 1);
                    break;
                }
            }
            $scope.IsReadonlySubjectType = false;
            $scope.FormData.AutomaticBill = 0;/*自动账单为否*/
            $scope.BillDayDisabled = true;
            $scope.FormData.BillDay = null;
            $scope.BillDayAutomaticBill = true;
            $scope.SubjectTypeList = sbtype;
        }
     
        else {
            var tData = angular.copy($scope.SubjectTypeListInit);
            for (var i = 0; i < tData.length; i++) {
                if (tData[i].Code == '5') {
                    tData.splice(i, 1);
                    break;
                }
            }
            $scope.SubjectTypeList = tData;
            $scope.IsReadonlySubjectType = false;
        }

        /*编辑时不可更改的项目*/
        if (ckFramework.ChargeSubjectViewData.IsAllowdEdit) {
            if ($scope.FormData.BillPeriod == 4) {
                $scope.IsReadonlySubjectType = true;
            } else {
                $scope.IsReadonlySubjectType = false;
            }
            $scope.IsReadonlySubjectName = false;
            $scope.IsReadonlySubjectCode = false;
            $scope.IsReadonlySubjectBillPeriod = false;
            $scope.IsReadonlyChargeFormulaShow = false;
            $scope.IsReadonlyStartTime = false;
            $scope.HaveBill = false;
        } else {
            $scope.IsReadonlySubjectType = true;
            $scope.IsReadonlySubjectName = true;
            $scope.IsReadonlySubjectCode = true;
            $scope.IsReadonlySubjectBillPeriod = true;
            $scope.IsReadonlyChargeFormulaShow = true;
            $scope.IsReadonlyStartTime = true;
            $scope.HaveBill = true;
        }


        $scope.Validate = function (formId) {
            return ChargeSubjectViewService.Validate(formId);
        }
        if (!(ckFramework.ChargeSubjectViewData.ChargeSubject.Id > 0)) {
            $scope.FormData.IsOnline = true;/*整数1的时候默认无法选中*/
            $scope.FormData.IsDel = 0;
        }



        $scope.SaveData = function (callback, arr) {
            for (var key in $scope.FormData) {
                if (key == "ComDeptId") {
                    $scope.FormData[key] = arr[0]
                }
                if (key == "ChargeFormula") {
                    $scope.FormData[key] = arr[1]
                }
                if (key == "ChargeFormulaShow") {
                    $scope.FormData[key] = arr[2]
                }
            }
            $scope.FormData.BeginDate = $("input[name='BeginDate']").val();
            $scope.FormData.IsOnline = $("#txtIsOnline").is(":checked");
            $scope.FormData.IsDel = !$("#txtIsDel").is(":checked");
            ChargeSubjectViewService.SaveData($scope.FormData, $scope.ChargeSubjectViewData.ViewType, callback);
        };

        $scope.ChangeSubjectType = function () {
            $scope.FormData.ChargeFormulaShow = "";
            ckFramework.codes = [];
        }
        $scope.ChangeBillPeriod = function (obj) {/*收费周期*/
            $scope.FormData.ChargeFormulaShow = "";
            ckFramework.codes = [];

            $scope.FormData.AutomaticBill = 1;/*默认自动账单*/
            $scope.BillDayDisabled = false;
            $scope.FormData.BillDay = 1;
            $scope.BillDayAutomaticBill = false;

            if (obj == 1 || obj == 2) {
                var arr = new Array();
                var typeData = angular.copy($scope.SubjectTypeListInit);
                for (var i = 0; i < typeData.length; i++) {
                    if (typeData[i].Code != "1" && typeData[i].Code != "2") {
                        arr.push(typeData[i].Code);
                    }
                }
                for (var i = 0; i < arr.length; i++) {
                    for (var m = 0; m < typeData.length; m++) {
                        if (typeData[m].Code == arr[i]) {
                            typeData.splice(m, 1);
                        }
                    }
                }
                $scope.SubjectTypeList = typeData;
                $scope.IsReadonlySubjectType = false;
            }
            else if (obj == 3)/*一次性科目*/ {
                $scope.SubjectTypeList = angular.copy($scope.SubjectTypeListInit);
                $scope.FormData.SubjectType = 5;
                $scope.IsReadonlySubjectType = false;
                /*排除三表*/
                var typeData = angular.copy($scope.SubjectTypeListInit);
                for (var i = 0; i < typeData.length; i++) {
                    if (typeData[i].Code == '3') {
                        typeData.splice(i, 1);
                        break;
                    }
                }
                $scope.FormData.AutomaticBill = 0;/*自动账单为否*/
                $scope.BillDayDisabled = true;
                $scope.FormData.BillDay = null;
                $scope.SubjectTypeList = typeData;
                $scope.BillDayAutomaticBill = true;
            }
            else if (obj == 4) {
                $scope.SubjectTypeList = angular.copy($scope.SubjectTypeListInit);
                $scope.FormData.SubjectType = 3;
                $scope.IsReadonlySubjectType = true;
            }
            else {
                var typeData = angular.copy($scope.SubjectTypeListInit);
                for (var i = 0; i < typeData.length; i++) {
                    if (typeData[i].Code == '5') {
                        typeData.splice(i, 1);
                        break;
                    }
                }
                $scope.SubjectTypeList = typeData;
                $scope.IsReadonlySubjectType = false;
                if ($scope.FormData.SubjectType == 5) {
                    $scope.FormData.SubjectType = null;
                }
                //alert(JSON.stringify($scope.SubjectTypeList))
                //$("#drpSubjectType").removeAttr("disabled", "disabled");
            }
        }
        $scope.ConfirmChargeFormula = function () {
            var selDept = $("#SelectDeptTypeAndId").val();
            selDept = selDept.split("_");
            if (selDept.length == 2) {
                if (selDept[1] != 11) {
                    ckFramework.ModalHelper.Alert("请选择小区!");
                    return false;
                }
            } else {
                ckFramework.ModalHelper.Alert("请选择小区!");
                return false;
            }



            var operate = $("#txtFormula").val().substr($("#txtFormula").val().length - 1, $("#txtFormula").val().length);
            var left = $("#txtFormula").val().split('(').length - 1;
            var right = $("#txtFormula").val().split(')').length - 1;
            if (left != right) {
                ckFramework.ModalHelper.Alert("括号必须成对出现!");
                return false;
            }
            if (operate == "+" || operate == "-" || operate == "*" || operate == "/" || operate == "(") {
                ckFramework.ModalHelper.Alert("公式设置有误请重新设置!");
                return false;
            }
            /*三表类型*/
            if ($scope.FormData.SubjectType == 3) {
                var billPeriod = $("#drpBillPeriod").val();
                if (!(billPeriod.indexOf("3") > 0) && $("#txtFormula").val() == "单价") {
                    ckFramework.ModalHelper.Alert("三表计算公式不能为单价，请选择三表具体计算项目!");
                    return false;
                }
            }
            $("#content").hide();
            $("#btnCloseFormula").show();
            $("#hidComDeptId").val(selDept[0]);
            $("#hidKeyFormual").val($("#hidFormual").val());
            $scope.FormData.ChargeFormulaShow = $("#txtFormula").val();
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

    ckFramework.ChargeSubjectViewController.$inject = injectParams;

    app.register.controller('ChargeSubjectViewController', ckFramework.ChargeSubjectViewController);
});
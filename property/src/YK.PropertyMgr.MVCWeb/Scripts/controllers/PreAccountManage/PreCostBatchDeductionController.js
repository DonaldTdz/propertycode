'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PreAccountManage/PreAccountManageService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PreAccountManageService'];

    ckFramework.PreCostBatchDeductionController = function ($compile, $rootScope, $scope, PreAccountManageService) {

        $scope.GetBatchDeductionBillList = function () {
            $(".chkALL").prop("checked", false);
            $("#btnBatchDeduction").click();
        }

        $scope.PreCostBatchDeduction = function () {
            var hsids = [];
            $("#" + ckFramework.createBatchDeductionData.TableId).find("input[class='DeductionCheckbox']:checked").each(function () {
                var val = $(this).val();
                if (val != undefined && val.length > 0) {
                    hsids.push(val);
                }
            });
            //alert(hsids.length);
            if (hsids.length <= 0) {
                ckFramework.ModalHelper.Alert("请选择批量抵扣房屋");
                return;
            }
            PreAccountManageService.PreCostBatchDeduction(hsids, function (data) {
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.Alert(data.Msg, 3000);//弹出提示信息,3秒后关闭
                    $scope.GetBatchDeductionBillList();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        }

    }

    ckFramework.PreCostBatchDeductionController.$inject = injectParams;

    app.register.controller('PreCostBatchDeductionController', ckFramework.PreCostBatchDeductionController);
});
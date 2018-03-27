'use strict';
define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/EntrancePower/EntrancePowerViewService.js'], function (app) {
    var injectParams = ['$scope', '$http', 'EntrancePowerViewService'];
    ckFramework.EntrancePowerViewController = function ($scope, $http, EntrancePowerViewService) {
        $scope.PowerViewData = ckFramework.EntrancePowerViewData.EntrancePowerViewData;
        $scope.IsShowSearch = true;
        $scope.Save = function () {
            var EntrancesIds = "";
            var UserIds = "";
            $("#table_ckFramework_EntrancePowerView").find("input[type='checkbox']").each(function () {
                if ($(this).prop("class") == "EntrancesInfo" && $(this).is(":checked")) {
                    EntrancesIds = EntrancesIds + $(this).attr("EntrancesId") + ",";
                }
            });
            $("#table_ckFramework_EntrancePowerList").find("input[type='checkbox']").each(function () {
                if ($(this).prop("class") == "UserInfo" && $(this).is(":checked")) {
                    UserIds = UserIds + $(this).attr("userId") + ",";
                }
            });

            $scope.PowerViewData.EntrancesIds = EntrancesIds;
            if ($scope.PowerViewData.UserIds == "" || $scope.PowerViewData.UserIds == null) {
                $scope.PowerViewData.UserIds = UserIds;/*批量勾选时；如果单独点击授权列表中的按钮会自动传入*/
            }
            $scope.PowerViewData.KeyExpressTimes = $("#txtKeyExpressTime").val();

            if ($scope.PowerViewData.UserIds == "" || $scope.PowerViewData.UserIds == null) {
                ckFramework.ModalHelper.Alert("请选择授权用户信息!");
                return false;
            }
            if ($scope.PowerViewData.EntrancesIds == "" || $scope.PowerViewData.EntrancesIds == null) {
                ckFramework.ModalHelper.Alert("请选择授权设备信息!");
                return false;
            }

            if (($scope.PowerViewData.KeyExpressTime) == "" || $scope.PowerViewData.KeyExpressTime == null) {
                ckFramework.ModalHelper.Alert("请设置权限过期时间!");
                return false;
            }
            //添加发送通知 所需参数批量授权
            if (ckFramework.ShowPopData.EntranceSendMsgList) {
                $scope.PowerViewData.EntranceSendMsgList = ckFramework.ShowPopData.EntranceSendMsgList;
            }
          
            EntrancePowerViewService.Save($scope.PowerViewData)
        }
    };
    ckFramework.EntrancePowerViewController.$inject = injectParams;
    app.register.controller('EntrancePowerViewController', ckFramework.EntrancePowerViewController);
});
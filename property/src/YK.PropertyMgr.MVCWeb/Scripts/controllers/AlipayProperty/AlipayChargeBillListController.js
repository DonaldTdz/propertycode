'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {

    var injectParams = ['$scope', 'AlipayPropertyService'];

    ckFramework.AlipayChargeBillListController = function ($scope, AlipayPropertyService) {

        $scope.IsShowSearch = true;
        $scope.CommunityDeptList = ckFramework.AlipayChargeBillListData.CommunityDeptList
        $scope.ComDeptId = ckFramework.AlipayChargeBillListData.CommunityDeptList[0].Id;
        $scope.GetCommunityList = function () {
            var selDeptId = $("#SelectDeptId").val();
            AlipayPropertyService.GetCommunityListByPropertyDeptId(selDeptId, function (data) {
                $scope.CommunityDeptList = data;
                $scope.ComDeptId = data[0].Id;
                $("#SearchContainerAlipayRoom input:hidden[name='DefaultComDeptId']").val(data[0].Id);
            });
        }
        // $scope.GetCommunityList();
        $scope.ShowUploadAlipayChargeBillView = function () {

            if ($scope.ComDeptId == 0) {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            AlipayPropertyService.ShowUploadAlipayChargeBillView($scope.ComDeptId);
        }

        $scope.BatchDelete = function () {
            var ids = "";
            $("#table_ckFramework_AlipayChargeBillList").find("input[type='checkbox']:checked").each(function () {


                ids = ids + $(this).attr("data") + ",";
            });
            if (ids.length > 0) {
                ids = ids.substr(0, ids.length - 1);
            }
            if ($scope.ComDeptId == 0) {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            if (ids.length == 0) {
                ckFramework.ModalHelper.Alert("请选择房屋");
                return;
            }
            AlipayPropertyService.AlipayChargeBillBatchDelete(ids, $scope.ComDeptId, function (data) {

            });

        }

        $scope.SynchronizationChargeBillInfo = function () {
            ckFramework.ModalHelper.Alert("已提交，请稍后刷新");
        }


    }

    ckFramework.AlipayChargeBillListController.$inject = injectParams;

    app.register.controller('AlipayChargeBillListController', ckFramework.AlipayChargeBillListController);
}); 
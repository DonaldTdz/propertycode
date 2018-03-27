'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {

    var injectParams = ['$scope', 'AlipayPropertyService'];

    ckFramework.AlipayRoomListController = function ($scope, AlipayPropertyService) {
        $scope.IsShowSearch = true;
        $scope.CommunityDeptList = ckFramework.AlipayRoomListData.CommunityDeptList;
        $scope.ComDeptId = ckFramework.AlipayRoomListData.CommunityDeptList[0].Id;

        $scope.GetCommunityList = function () {
            var selDeptId = $("#SelectDeptId").val();
            AlipayPropertyService.GetCommunityListByPropertyDeptId(selDeptId, function (data) {
                $scope.CommunityDeptList = data;
                $scope.ComDeptId = data[0].Id;
                $("#SearchContainerAlipayRoom input:hidden[name='DefaultComDeptId']").val(data[0].Id);
            });
        }
      //  $scope.GetCommunityList();


        $scope.ShowUploadAlipayRoomView = function () {

            if ($scope.ComDeptId == 0) {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            AlipayPropertyService.ShowUploadAlipayRoomView($scope.ComDeptId);
        }


        $scope.BatchDelete = function () {
            var ids = "";
            $("#table_ckFramework_AlipayRoomList").find("input[type='checkbox']:checked").each(function () {


                ids = ids + $(this).attr("data") + ",";
            });
            if (ids.length > 0) {
                ids = ids.substr(0, ids.length - 1);
            }
            if ($scope.ComDeptId == 0)
            {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            if (ids.length == 0)
            {
                ckFramework.ModalHelper.Alert("请选择房屋");
                return;
            }
            AlipayPropertyService.BatchDelete(ids, $scope.ComDeptId, function (data)
            {

            });

        }

        $scope.SynchronizationRoomInfo = function () {
            if ($scope.ComDeptId == 0) {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            AlipayPropertyService.SynchronizationRoomInfo($scope.ComDeptId, function (data) {
                ckFramework.ModalHelper.Alert(data.Msg);
            });

        }

        


    }

    ckFramework.AlipayRoomListController.$inject = injectParams;

    app.register.controller('AlipayRoomListController', ckFramework.AlipayRoomListController);
});
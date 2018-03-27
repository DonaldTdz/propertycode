'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/EntrancePower/EntrancePowerListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'EntrancePowerListService'];
    ckFramework.EntrancePowerListController = function ($http, $scope, $rootScope, EntrancePowerListService) {
        $scope.IsShowSearch = true;
        $scope.EntrancePowerListData = ckFramework.EntrancePowerListData;

        $scope.ShowEntrancePowerView = function (viewType, userIds) {
            if (viewType == "Add") {
                ckFramework.ShowPopData = {};
                ckFramework.ShowPopData.EntranceSendMsgList = [];
                $("#table_ckFramework_EntrancePowerList").find("input[type='checkbox']").not(".chkALL").each(function () {
                    if ($(this).prop("checked")) {
                        var data = ckFramework.EntrancePowerTable.rows($(this).parents('tr')).data();
                        ckFramework.ShowPopData.EntranceSendMsgList.push({ "HouseDeptId": data[0].HouseDeptId, "DoorNo": data[0].AllRoomNo, "Phone": data[0].Telephone });
                    }
                });
                //alert(JSON.stringify(ckFramework.ShowPopData.EntranceSendMsgList));
            }
            EntrancePowerListService.ShowEntrancePowerView(viewType, userIds);
        }
    };


    ckFramework.EntrancePowerListController.$inject = injectParams;
    app.register.controller('EntrancePowerListController', ckFramework.EntrancePowerListController);
});

'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Entrance/EntranceListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', "EntranceListService"];
    ckFramework.EntranceListController = function ($http, $scope, $rootScope, EntranceListService) {

        $scope.IsShowSearch = true;
        $scope.EntranceListData = ckFramework.EntranceListData;

        $scope.ShowEntranceView = function (viewType, EntranceId) {
            if (viewType == "Add") {
                var subjectSelected = $('#deptTree').jstree().get_selected() + "";
                var arr = subjectSelected.split('_');
                /*小区类型*/
                if (arr[1] != 11) {
                    ckFramework.ModalHelper.Alert("请选择小区信息!");
                    return false;
                }
            }
            EntranceListService.ShowEntranceView(viewType, EntranceId);
        }

        $scope.DeleteEntrance = function (EntranceId) {
            EntranceListService.DeleteEntrance(EntranceId);
        }

    };

    ckFramework.EntranceListController.$inject = injectParams;
    app.register.controller('EntranceListController', ckFramework.EntranceListController);
});

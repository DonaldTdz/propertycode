'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.EntranceListService = function ($http, $q, $compile, $rootScope) {
        var EntranceListService = {};

        EntranceListService.ShowEntranceView = function (viewType, EntranceId) {
            var VillageId = 0;
            if (viewType == "Add") {
                var id = $("#SelectDeptTypeAndId").val() + "";
                var arr = id.split('_')
                if (arr.length == 2)/*选中的是小区*/ {
                    if (arr[1] == "11") {
                        VillageId = arr[0];
                    }
                }
            }
            var loadUrl;
            switch (viewType) {
                case "Add":
                    loadUrl = ckFramework.EntranceViewUrlAdd + '?villageId=' + VillageId;
                    break;
                case "Edit":
                    loadUrl = ckFramework.EntranceViewUrlEdit + '?entranceId=' + EntranceId + "&villageId=" + 0;
                    break;
                case "Look":
                    loadUrl = ckFramework.EntranceViewUrlLook + '?entranceId=' + EntranceId + "&villageId=" + 0;
                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/Entrance/EntranceViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 1100,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.EntranceTable.draw();
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divEntranceViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        EntranceListService.DeleteEntrance = function (EntranceId) {
            $http({
                url: "/PropertyMgr/Entrance/DeleteEntrance?entranceId=" + EntranceId,
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.CloseWait();
                ckFramework.ModalHelper.Alert(ckFramework.ClientMessage.GetMessage().DeleteSuccess);
                ckFramework.EntranceTable.draw();
            });
        };

        return EntranceListService;
    };

    ckFramework.EntranceListService.$inject = injectParams;
    app.register.factory('EntranceListService', ckFramework.EntranceListService);
});
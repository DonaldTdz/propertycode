'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.EntrancePowerListService = function ($http, $q, $compile, $rootScope) {
        var EntrancePowerListService = {};

        EntrancePowerListService.ShowEntrancePowerView = function (viewType, userIds) {
            /*取掉个人授权参数*/
            if (viewType != "AddPowperPersonal") {
                if (ckFramework.ClientCustomSearch.length >= 4) {
                    ckFramework.ClientCustomSearch.pop();/*移除上次加入的GUID*/
                }
            } 

            var loadUrl;
            switch (viewType) {
                case "Add":
                    loadUrl = ckFramework.EntrancePowerViewUrlAdd;
                    break;
                case "AddPowperPersonal":
                    loadUrl = ckFramework.EntrancePowerViewUrlEdit + '?userIds=' + userIds;
                    break;
                case "Look":
                    loadUrl = ckFramework.EntrancePowerViewUrlLook + '?userIds=' + userIds;

                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/EntrancePower/EntrancePowerViewController.js'], function (app) {
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
                            ckFramework.EntrancePowerViewTable.draw();
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divEntrancePowerViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return EntrancePowerListService;
    };

    ckFramework.EntrancePowerListService.$inject = injectParams;
    app.register.factory('EntrancePowerListService', ckFramework.EntrancePowerListService);
});
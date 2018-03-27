'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.PrepayAccountListService = function ($http, $q, $compile, $rootScope) {
        var PrepayAccountListService = {};

        PrepayAccountListService.ShowPrepayAccountView = function (viewType, HouseDeptId) {
            var loadUrl;
            switch (viewType) {
                case "Add":
                    loadUrl = ckFramework.PrepayAccountViewUrlAdd;
                    break;
                case "Edit":
                    loadUrl = ckFramework.PrepayAccountViewUrlEdit + '?HouseDeptId=' + HouseDeptId;
                    break;
                case "Look":
                    loadUrl = ckFramework.PrepayAccountViewUrlLook + '?HouseDeptId=' + HouseDeptId;
                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/PrepayAccount/PrepayAccountViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: false,
                        keyboard: false,
                        width: 1100,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.PrepayAccountTable.draw();
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divPrepayAccountViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }



        return PrepayAccountListService;
    };

    ckFramework.PrepayAccountListService.$inject = injectParams;
    app.register.factory('PrepayAccountListService', ckFramework.PrepayAccountListService);
});
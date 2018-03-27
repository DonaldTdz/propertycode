'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.VacationListService = function ($http, $q, $compile, $rootScope) {
        var vacationListService = {};

        vacationListService.ShowVacationView = function (viewType, VacationId) {
            var loadUrl;
            switch(viewType)
            {
                case "Add":
                    loadUrl = ckFramework.VacationViewUrlAdd;
                    break;
                case "Edit":
                    loadUrl = ckFramework.VacationViewUrlEdit + '?VacationId=' + VacationId;
                    break;
                case "View":
                    loadUrl = ckFramework.VacationViewUrlLook + '?VacationId=' + VacationId;
                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/Vacation/VacationViewController.js'], function (app) {
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
                            ckFramework.VacationTable.draw();
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divVacationViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        vacationListService.DeleteVacation = function (VacationId) {
            $http({
                url: "/PropertyMgr/Vacation/DeleteVacation?vacationId=" + VacationId,
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.CloseWait();
                ckFramework.ModalHelper.Alert(ckFramework.ClientMessage.GetMessage().DeleteSuccess);
                ckFramework.VacationTable.draw();
            });
        };

        return vacationListService;
    };

    ckFramework.VacationListService.$inject = injectParams;
    app.register.factory('VacationListService', ckFramework.VacationListService);
});
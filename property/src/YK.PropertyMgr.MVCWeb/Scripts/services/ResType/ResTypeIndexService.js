'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ResTypeIndexService = function ($http, $q, $compile, $rootScope) {
        var ResTypeIndexService = {};

        ResTypeIndexService.ShowSubjectBillView = function (data) {
            //alert(22)
            //alert(JSON.stringify(data))
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            ckFramework.ModalHelper.BillViewData = data;
            $modal.load("PropertyMgr/SubjectBill/SubjectBillIndex", { }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/SubjectBill/SubjectBillListController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 800,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;

                    $modal.off("hide.bs.modal").on("hide.bs.modal", function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ChargeSubjectTable.draw();
                        }
                    });
                    $compile($('#divSubjectBillController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        return ResTypeIndexService;
    };

    ckFramework.ResTypeIndexService.$inject = injectParams;
    app.register.factory('ResTypeIndexService', ckFramework.ResTypeIndexService);
});
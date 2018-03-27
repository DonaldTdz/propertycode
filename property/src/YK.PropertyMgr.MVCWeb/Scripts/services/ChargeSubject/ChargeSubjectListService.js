'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ChargeSubjectListService = function ($http, $q, $compile, $rootScope) {
        var chargeSubjectListService = {};

        chargeSubjectListService.ShowChargeSubjectView = function (viewType, SubjectId) {
            var loadUrl;
            switch (viewType) {
                case "Add":
                    loadUrl = ckFramework.ChargeSubjectViewUrlAdd;
                    break;
                case "Edit":
                    loadUrl = ckFramework.ChargeSubjectViewUrlEdit + '?subjectId=' + SubjectId;
                    break;
                case "Look":
                    loadUrl = ckFramework.ChargeSubjectViewUrlLook + '?subjectId=' + SubjectId;
                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargeSubject/ChargeSubjectViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 1100,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;

                    $modal.off('hide.bs.modal').on("hide.bs.modal", function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ChargeSubjectTable.draw();
                        }
                    });
                    $compile($('#divChargeSubjectViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        chargeSubjectListService.DeleteChargeSubject = function (SubjectId) {
            $http({
                url: "/PropertyMgr/ChargeSubject/DeleteChargeSubject?subjectId=" + SubjectId,
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.CloseWait();
                ckFramework.ModalHelper.Alert(ckFramework.ClientMessage.GetMessage().DeleteSuccess);
                ckFramework.ChargeSubjectTable.draw();
            });
        };

        return chargeSubjectListService;
    };

    ckFramework.ChargeSubjectListService.$inject = injectParams;
    app.register.factory('ChargeSubjectListService', ckFramework.ChargeSubjectListService);
});
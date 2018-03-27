'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ReceiptBookService = function ($http, $q, $compile, $rootScope) {
        var ReceiptBookService = {};
        ReceiptBookService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        ReceiptBookService.ShowReceiptBookView = function (viewType, ReceiptBookId) {
            var loadUrl;
            switch (viewType) {
                case "Add":
                    loadUrl = ckFramework.ReceiptBookViewUrlAdd;
                    break;
                case "Edit":
                    loadUrl = ckFramework.ReceiptBookViewUrlEdit + '?receiptBookId=' + ReceiptBookId;
                    break;
                case "Look":
                    loadUrl = ckFramework.ReceiptBookViewUrlLook + '?receiptBookId=' + ReceiptBookId;
                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ReceiptBook/ReceiptBookViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;

                    $modal.off('hide.bs.modal').on("hide.bs.modal", function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ReciptBookManagerTable.draw();
                        }
                    });
                    $compile($('#divReceiptBookViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        ReceiptBookService.ShowReceiptBookDetailView = function (ReceiptBookId) {
           var loadUrl;
           loadUrl = ckFramework.ReceiptBookViewUrlShow + '?Id=' + ReceiptBookId;
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ReceiptBook/ReceiptBookDetailShowViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 1000,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;

                    $modal.off('hide.bs.modal').on("hide.bs.modal", function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ReciptBookManagerTable.draw();
                        }
                    });
                    $compile($('#divReceiptBookDetailShowViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }



        ReceiptBookService.SaveData = function (formData, viewType, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: formData,
                    url: viewType == 'Add' ? "/PropertyMgr/ReceiptBook/AddReceiptBook" : "/PropertyMgr/ReceiptBook/EditReceiptBook",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        if (viewType == 'Add' && callback) {
                            callback(data);
                        }
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.ModalHelper.Alert(data.DataInfo);

                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.DataInfo);
                    }
                });
            }, 200);
        };

        ReceiptBookService.SaveModifyData = function (formData, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: formData,
                    url: "/PropertyMgr/ReceiptBook/ModifyReceiptBook",
                }).success(function (data, status, headers, config) {

                    if (data.IsSuccess) {
                        if (data.ActionInfo == "2") {
                            ckFramework.ModalHelper.Confirm(data.DataInfo, function () {
                                formData.IsModify = true;
                                $http({
                                    method: 'POST',
                                    data: formData,
                                    url: "/PropertyMgr/ReceiptBook/ModifyReceiptBook",
                                }).success(function (data, status, headers, config) {
                                    if (data.IsSuccess) {
                                        document.getElementById("ReceiptBookModifyForm").reset();
                                        ckFramework.ModalHelper.Alert(data.DataInfo);
                                    }
                                    else {
                                        ckFramework.ModalHelper.Alert(data.DataInfo);
                                    }
                                });






                            });
                        }
                        else if (data.ActionInfo == "1") {
                            document.getElementById("ReceiptBookModifyForm").reset();
                            ckFramework.ModalHelper.Alert(data.DataInfo);
                        }

                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.DataInfo);
                    }
                });
            }, 200);
        };

        return ReceiptBookService;
    };

    ckFramework.ReceiptBookService.$inject = injectParams;
    app.register.factory('ReceiptBookService', ckFramework.ReceiptBookService);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ChargeRecordListService = function ($http, $q, $compile, $rootScope) {
        var chargeRecordListService = {};

        chargeRecordListService.CompileChargeRecordView = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {
                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        chargeRecordListService.ShowRefundModal = function (info) {
            info.ResourcesNamesFormat = "";
            var loadUrl = ckFramework.RefundRecordUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            info.HouseDoorNoFormat = "";
            $modal.load(loadUrl, { chargeRecordInfo: info }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/RefundRecord/RefundRecordController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.off('hide.bs.modal').on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ChargeRecordTable.draw();
                        }
                    });
                    $compile($('#divRefundRecordController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        chargeRecordListService.ShowChargeRecordViewModal = function (info) {
            info.ResourcesNamesFormat = "";
            $http({
                method: 'POST',
                url: '/PropertyMgr/ChargeRecord/ChargeRecordValidation',
                data: {
                    ChargeRecord: info
                }
            }).success(function (data) {
                if (data.IsSuccess) {
                    var loadUrl = ckFramework.ChargeRecordViewUrl;
                    //alert(loadUrl);
                    var $modal = $('#divModal');
                    ckFramework.ModalHelper.OpenModal1 = $modal;
                    ckFramework.ModalHelper.OpenWait();
                    info.HouseDoorNoFormat = "";
                    $modal.load(loadUrl, { chargeRecordInfo: info }, function () {
                        require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargeRecord/ChargeRecordViewController.js'], function (app) {
                            $modal.modal({
                                backdrop: 'static',
                                scrollY: true,
                                keyboard: false,
                                width: 500,
                                maxHeight: 600
                            });
                            ckFramework.ModalHelper.isRefresh = false;
                            $modal.on('hide.bs.modal', function () {
                                if (ckFramework.ModalHelper.isRefresh) {
                                    ckFramework.ChargeRecordTable.draw();
                                }
                                $(this).off('hide.bs.modal');
                            });
                            $compile($('#divChargeRecordViewController'))($rootScope);
                            $rootScope.$apply();
                        });
                    });
                } else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
            
        }
        chargeRecordListService.ShowReceiptPrintViewModal = function (info) {
            info.ResourcesNamesFormat = "";
            var loadUrl = ckFramework.ReceiptPrintViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            info.HouseDoorNoFormat = "";
            $modal.load(loadUrl, { chargeRecordInfo: info }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargeRecord/ReceiptPrintViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ChargeRecordTable.draw();
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divReceiptPrintViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        chargeRecordListService.ShowBillChargeRecordViewModal = function (houseDeptId, id, rnum) {
            var loadUrl = ckFramework.BillChargeRecordViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { HouseDeptId: houseDeptId, RecordId: id, ReceiptNum: rnum }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargeRecord/BillChargeRecordViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: false,
                        keyboard: false,
                        width: 1000,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $compile($('#divBillChargeRecordViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        chargeRecordListService.Refund = function (refundRecord, callback) {
            var validate = $('#' + ckFramework.createRefundRecordFormData.FormId).data('formValidation').validate();
            if (validate.isValid()) {
                    setTimeout(function () {
                        $http({
                            method: 'POST',
                            url: '/PropertyMgr/ChargeRecord/Refund',
                            data: {
                                RefundRecord: refundRecord
                            }
                        }).success(function (data) {
                            callback(data);
                        });
                    }, 200);
                }
        }
        chargeRecordListService.SaveData = function (chargeRecord, callback) {
            var validate = $('#' + ckFramework.createChargeRecordViewFormData.FormId).data('formValidation').validate();
            if (validate.isValid()) {
                setTimeout(function () {
                    $http({
                        method: 'POST',
                        url: '/PropertyMgr/ChargeRecord/Update',
                        data: {
                            ChargeRecord: chargeRecord
                        }
                    }).success(function (data) {
                        callback(data);
                    });
                }, 200);
            }
        }
        
        chargeRecordListService.ReceiptPrint = function (chargeRecord, callback) {
            var validate = $('#' + ckFramework.createReceiptPrintViewFormData.FormId).data('formValidation').validate();
            if (validate.isValid()) {
                setTimeout(function () {
                    $http({
                        method: 'POST',
                        url: '/PropertyMgr/ChargeRecord/ReceiptPrint',
                        data: {
                            ChargeRecord: chargeRecord
                        }
                    }).success(function (data) {
                        callback(data);
                    });
                }, 200);
            }
        }


        chargeRecordListService.ShowForegiRefundModal = function (info) {
            info.ResourcesNamesFormat = "";
            var loadUrl = ckFramework.ForegiRefundRecordUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            info.HouseDoorNoFormat = "";
            $modal.load(loadUrl, { chargeRecordInfo: info }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/RefundRecord/RefundRecordController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ChargeRecordTable.draw();
                        }
                        //解绑 去除重复绑定
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divRefundRecordController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        chargeRecordListService.ShowForegiChargeRecordViewModal = function (info) {
            info.ResourcesNamesFormat = "";
            $http({
                method: 'POST',
                url: '/PropertyMgr/ChargeRecord/ChargeRecordValidation',
                data: {
                    ChargeRecord: info
                }
            }).success(function (data) {
                if (data.IsSuccess) {
                    var loadUrl = ckFramework.ForegiChargeRecordViewUrl;
                    //alert(loadUrl);
                    var $modal = $('#divModal');
                    ckFramework.ModalHelper.OpenModal1 = $modal;
                    ckFramework.ModalHelper.OpenWait();
                    info.HouseDoorNoFormat = "";
                    $modal.load(loadUrl, { chargeRecordInfo: info }, function () {
                        require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargeRecord/ChargeRecordViewController.js'], function (app) {
                            $modal.modal({
                                backdrop: 'static',
                                scrollY: true,
                                keyboard: false,
                                width: 500,
                                maxHeight: 600
                            });
                            ckFramework.ModalHelper.isRefresh = false;
                            $modal.on('hide.bs.modal', function () {
                                if (ckFramework.ModalHelper.isRefresh) {
                                    ckFramework.ChargeRecordTable.draw();
                                }
                                $(this).off('hide.bs.modal');
                            });
                            $compile($('#divChargeRecordViewController'))($rootScope);
                            $rootScope.$apply();
                        });
                    });
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        }
        chargeRecordListService.ShowForegiReceiptPrintViewModal = function (info) {
            info.ResourcesNamesFormat = "";
            var loadUrl = ckFramework.ForegiReceiptPrintViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            info.HouseDoorNoFormat = "";
            $modal.load(loadUrl, { chargeRecordInfo: info }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargeRecord/ReceiptPrintViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ChargeRecordTable.draw();
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divReceiptPrintViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }


        return chargeRecordListService;
    };

    ckFramework.ChargeRecordListService.$inject = injectParams;
    app.register.factory('ChargeRecordListService', ckFramework.ChargeRecordListService);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.DailyChargListService = function ($http, $q, $compile, $rootScope) {
        var DailyChargListService = {};

        DailyChargListService.ShowSplitBillViewModal = function (info, callback) {
            var loadUrl = ckFramework.SplitBillViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { bill: info }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargBill/SplitBillViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.off('hide.bs.modal').on('hide.bs.modal', function () {
                        //alert(ckFramework.ModalHelper.isRefresh)
                        if (ckFramework.ModalHelper.isRefresh) {
                            //ckFramework.ChargSubjectTable.draw();
                            //alert(JSON.stringify(ckFramework.ModalHelper.SplitBillData));
                            callback(ckFramework.ModalHelper.SplitBillData);
                        }
                    });
                    $compile($('#divSplitBillViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }




        DailyChargListService.ShowPrepareAmountViewModal = function (hid, doorno, callback) {
            var loadUrl = ckFramework.PrepareAmountViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { houseDeptId: hid, doorNo: doorno }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargBill/PrepareAmountViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 600,
                        maxHeight: 500
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            //ckFramework.ChargSubjectTable.draw();
                            callback(ckFramework.ModalHelper.PrepareBillData);
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divPrepareAmountViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        DailyChargListService.ShowDeleteBillViewModal = function (info, callback) {
            var loadUrl = ckFramework.DeleteBillViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { bill: info }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargBill/DeleteBillViewController.js'], function (app) {
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
                            callback(ckFramework.ModalHelper.DeleteBillData);
                        }
                    });
                    $compile($('#divDeleteBillViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        DailyChargListService.ShowManuallyGenerateBillViewModal = function (hid, callback) {
            var loadUrl = ckFramework.ManuallyGenerateBillViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { houseDeptId: hid }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargBill/ManuallyGenerateBillViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 800,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.off('hide.bs.modal').on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            callback(ckFramework.ModalHelper.ManuallyGenerateBillData);
                        }
                    });
                    $compile($('#divManuallyGenerateBillViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }


        DailyChargListService.PrepareAmount = function (billInfo, callback) {
            var validate = $('#' + ckFramework.createPrepareAmountViewFormData.FormId).data('formValidation').validate();
            if (validate.isValid()) {
                setTimeout(function () {
                    $http({
                        method: 'POST',
                        url: '/PropertyMgr/ChargBill/PrepareAmount',
                        data: {
                            bill: billInfo
                        }
                    }).success(function (data) {
                        callback(data);
                    });
                }, 200);
            }
        }

        DailyChargListService.SplitBill = function (billInfo, callback) {
            var validate = $('#' + ckFramework.createSplitBillViewFormData.FormId).data('formValidation').validate();
            if (validate.isValid()) {
                setTimeout(function () {
                    $http({
                        method: 'POST',
                        url: '/PropertyMgr/ChargBill/SplitBill',
                        data: {
                            bill: billInfo
                        }
                    }).success(function (data) {
                        //alert(JSON.stringify(data));
                        callback(data);
                    });
                }, 200);
            }
        }

        DailyChargListService.DeleteBill = function (billInfo,billList, callback) {

       
           

            var validate = $('#' + ckFramework.createDeleteBillViewFormData.FormId).data('formValidation').validate();
            if (validate.isValid()) {
                ckFramework.ModalHelper.Confirm("是否作废账单？", function () {
                    setTimeout(function () {
                        $http({
                            method: 'POST',
                            url: '/PropertyMgr/ChargBill/DeleteBill',
                            data: {
                                bill: billInfo,
                                billList: billList,
                                deleteModel: ckFramework.ModalHelper.DeleteModel
                            }
                        }).success(function (data) {
                            //alert(JSON.stringify(data));
                            callback(data);
                        });
                    }, 200);
                });
                }
          
         
        }

        DailyChargListService.RefreshBill = function (hid,callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/ChargBill/RefreshBill',
                    data: {
                        houseDeptID: hid
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        DailyChargListService.ManuallyGenerateBill = function (hid,DeptType, nodes, EndDate, callback) {
            //alert(nodes)
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/ChargBill/ManuallyGenerateBill',
                    data: {
                        houseDeptId: hid,
                        DeptType:DeptType,
                        IdStr: nodes,
                        EndDate: EndDate
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        DailyChargListService.GetDailyChargList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/ChargBill/GetDailyChargList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        //add v2.9 confirm model
        DailyChargListService.ShowConfirmViewModal = function (confirmData, callback) {
            var loadUrl = ckFramework.ConfirmViewUrl;
            ckFramework.ModalHelper.BillConfirmData = confirmData;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { isPrintReceipt: confirmData.IsPrintReceipt, deptId: $("#SelectDeptId").val(), deptType: $("#SelectDeptType").val() }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargBill/ConfirmViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 800,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.off('hide.bs.modal').on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            callback(ckFramework.ModalHelper.ConfirmResult);
                        }
                    });
                    $compile($('#divConfirmViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        //add v2.9 获取票据号
        DailyChargListService.GetReceiptBookNumber = function (callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/ChargBill/GetReceiptBookNumber',
                    data: { deptId: $("#SelectDeptId").val(), deptType: $("#SelectDeptType").val() }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        return DailyChargListService;
    };

    ckFramework.DailyChargListService.$inject = injectParams;
    app.register.factory('DailyChargListService', ckFramework.DailyChargListService);
});
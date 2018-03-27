
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ForeigBillChargeListService = function ($http, $q, $compile, $rootScope) {
        var ForeigBillChargeListService = {};

        ForeigBillChargeListService.GetForeigBillChargList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',               
                    url: '/PropertyMgr/ChargBill/GetForeigBillChargeList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        ForeigBillChargeListService.ShowForeigBillViewModal = function (hid, callback) {
            var loadUrl = ckFramework.ForeigBillChargeViewUrlAdd;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { HouseDeptId: hid }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/ChargBill/ForeigBillChargeViewController.js'], function (app) {
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
                            //ckFramework.ChargSubjectTable.draw();
                            callback(ckFramework.ModalHelper.ForeigBillData);
                        }
                    });
                    $compile($('#divForeigBillChargeViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        ForeigBillChargeListService.Save = function (data, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: data,
                    url: "/PropertyMgr/ChargBill/AddForeigChargeBill",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        ckFramework.ForeigBillCustomerName = data.Data.CustomerName;
                        callback(data)
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            }, 200);
        };

        return ForeigBillChargeListService;
    };

    ckFramework.ForeigBillChargeListService.$inject = injectParams;
    app.register.factory('ForeigBillChargeListService', ckFramework.ForeigBillChargeListService);
});

'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.TempChargeListService = function ($http, $q, $compile, $rootScope) {
        var TempChargeListService = {};

        TempChargeListService.GetTempChargList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/TemporaryCharge/GetTempChargList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        TempChargeListService.ShowTempBillViewModal = function (hid, RefType,callback) {
            var loadUrl = ckFramework.TempChargeViewUrlAdd;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { HouseDeptId: hid, RefType: RefType }, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/TempCharge/TempChargeViewController.js'], function (app) {
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
                            callback(ckFramework.ModalHelper.TempBillData);
                        }
                    });
                    $compile($('#divTempChargeViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        TempChargeListService.Save = function (data, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: data,
                    url: "/PropertyMgr/TemporaryCharge/AddTemporaryChargeBill",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        //ckFramework.ModalHelper.Alert(data.Msg);
                        //ckFramework.ModalHelper.CloseOpenModal1();
                        callback(data)
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                        //ckFramework.TempChargeTable.draw();
                    }
                });
            }, 200);
        };

        return TempChargeListService;
    };

    ckFramework.TempChargeListService.$inject = injectParams;
    app.register.factory('TempChargeListService', ckFramework.TempChargeListService);
});
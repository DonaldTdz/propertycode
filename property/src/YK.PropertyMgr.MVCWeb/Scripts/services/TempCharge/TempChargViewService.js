
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.TempChargViewService = function ($http, $q, $compile, $rootScope) {
        var TempChargViewService = {};
        TempChargViewService.Save = function (data) {

            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: data,
                    url: "/PropertyMgr/TemporaryCharge/AddTemporaryChargeBill",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.Alert(data.Msg);
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.TempChargeTable.draw();
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                        ckFramework.TempChargeTable.draw();
                    }
                    $("#tableallCheck").prop("checked", false)
                });
            }, 200);
        };
        return TempChargViewService;
    };

    ckFramework.TempChargViewService.$inject = injectParams;
    app.register.factory('TempChargViewService', ckFramework.TempChargViewService);
});
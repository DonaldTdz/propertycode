'use strict';

define(['apps/HomeApp'], function (app) {

    var injectParams = ['$http', '$q'];

    ckFramework.EntrancePowerViewService = function ($http, $q) {
        var EntrancePowerViewService = {};
        EntrancePowerViewService.Save = function (formData) {
            $http({
                method: 'POST',
                data: formData,
                url: "/PropertyMgr/EntrancePower/Save"
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.isRefresh = true;
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.CloseOpenModal1();
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        }
        return EntrancePowerViewService;
    };



    ckFramework.EntrancePowerViewService.$inject = injectParams;
    app.register.factory('EntrancePowerViewService', ckFramework.EntrancePowerViewService);
});
'use strict';

define(['apps/HomeApp'], function (app) {

    var injectParams = ['$http', '$q'];

    ckFramework.ChargeSubjectViewService = function ($http, $q) {
        var chargeSubjectViewService = {};

        chargeSubjectViewService.Validate = function (formId) {
            var validate = $('#' + formId).data('formValidation').validate();
            if (!validate.isValid()) {
                ckFramework.ModalHelper.Alert(ckFramework.ClientMessage.GetMessage().UserValidateError);
                return false;
            }

            return true;
        }

        chargeSubjectViewService.SaveData = function (formData, viewType, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: formData,
                    url: viewType == 'Add' ? "/PropertyMgr/ChargeSubject/AddChargeSubject" : "/PropertyMgr/ChargeSubject/EditChargeSubject",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        if (viewType == 'Add' && callback) {
                            callback(data.DataInfo);
                        }
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.ModalHelper.Alert(data.ActionInfo);

                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.ActionInfo);
                    }
                });
            }, 200);
        };

        return chargeSubjectViewService;
    };

    ckFramework.ChargeSubjectViewService.$inject = injectParams;
    app.register.factory('ChargeSubjectViewService', ckFramework.ChargeSubjectViewService);
});
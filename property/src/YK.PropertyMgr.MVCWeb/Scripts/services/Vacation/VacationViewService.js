'use strict';

define(['apps/HomeApp'], function (app) {

    var injectParams = ['$http', '$q'];
    ckFramework.VacationViewService = function ($http, $q) {
        var vacationViewService = {};

        vacationViewService.Validate = function(formId)
        {
            var validate = $('#' + formId).data('formValidation').validate();
            if (!validate.isValid()) {
                ckFramework.ModalHelper.Alert(ckFramework.ClientMessage.GetMessage().UserValidateError);
                return false;
            }

            return true;
        }

        vacationViewService.SaveData = function (formData, viewType, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: formData,
                    url: viewType == 'Add' ? "/PropertyMgr/Vacation/AddVacation" : "/OAProject/Vacation/EditVacation",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        if (viewType == 'Add' && callback)
                        {
                            callback(data.DataInfo);
                        }
                        ckFramework.ModalHelper.Alert(data.ActionInfo);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.ActionInfo);
                    }
                });
            }, 200);
        };

        return vacationViewService;
    };

    ckFramework.VacationViewService.$inject = injectParams;
    app.register.factory('VacationViewService', ckFramework.VacationViewService);
});
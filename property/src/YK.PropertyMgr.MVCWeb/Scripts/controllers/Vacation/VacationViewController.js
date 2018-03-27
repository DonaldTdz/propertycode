'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Vacation/VacationViewService.js'], function (app) {
    var injectParams = ['$scope', 'VacationViewService'];

    ckFramework.VacationViewController = function ($scope, VacationViewService) {
        $scope.ClientMessage = ckFramework.ClientMessage.GetMessage();
        $scope.VacationViewData = ckFramework.VacationViewData;
        $scope.FormData = ckFramework.VacationViewData.VacationInfo;

        $scope.Validate = function (formId) {
            return VacationViewService.Validate(formId);
        }

        $scope.SaveData = function (callback) {
            VacationViewService.SaveData($scope.FormData, $scope.VacationViewData.ViewType, callback);
        };
    };

    ckFramework.VacationViewController.$inject = injectParams;

    app.register.controller('VacationViewController', ckFramework.VacationViewController);
});
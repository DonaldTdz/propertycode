'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeRecord/ChargeRecordListService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ChargeRecordListService'];
    ckFramework.ChargeRecordViewController = function ($http, $scope, $rootScope, ChargeRecordListService) {
        $scope.FormData = ckFramework.ChargeRecordViewData.ChargeRecordInfo;
        $scope.SaveData = function () {
            ChargeRecordListService.SaveData($scope.FormData, function (data) {
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                ckFramework.ModalHelper.Alert(data.Msg);
            });
        };
    };

    ckFramework.ChargeRecordViewController.$inject = injectParams;
    app.register.controller('ChargeRecordViewController', ckFramework.ChargeRecordViewController);
});

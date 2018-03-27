'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PreAccountManage/PreAccountManageService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PreAccountManageService'];

    ckFramework.PreAccountCostTransferViewController = function ($compile, $rootScope, $scope, PreAccountManageService) {

        $scope.FormData = {};
        $scope.FormData.ChargeSubjectId = 0;
        $scope.PreAccountTransfer = ckFramework.PreAccountCostTransferData.PreAccountTransfer;
        $scope.HouseSubjectList = ckFramework.PreAccountCostTransferData.HouseSubjectList;

        $scope.PreAccountCostTransfer = function () {
            $scope.FormData.OutChargeSubjectName = $scope.PreAccountTransfer.ChargeSubjectName;
            $scope.FormData.InChargeSubjectName = $scope.GetSelectSubjectName();
            $scope.FormData.PrepayAccountId = $scope.PreAccountTransfer.Id;
            PreAccountManageService.PreAccountCostTransfer($scope.FormData, function (data) {
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.PreAccountCostTransferData = data;
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        }

        $scope.GetSelectSubjectName = function () {
            var subjectName = "";
            $scope.HouseSubjectList.forEach(function (item) {
                if (item.Id == $scope.FormData.ChargeSubjectId) {
                    subjectName = item.Name;
                }
            });
            return subjectName;
        }
     
    }

    ckFramework.PreAccountCostTransferViewController.$inject = injectParams;

    app.register.controller('PreAccountCostTransferViewController', ckFramework.PreAccountCostTransferViewController);
});
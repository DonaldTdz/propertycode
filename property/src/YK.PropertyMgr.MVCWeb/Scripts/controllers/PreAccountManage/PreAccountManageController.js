'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PreAccountManage/PreAccountManageService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope','PreAccountManageService'];

    ckFramework.PreAccountManageController = function ($compile, $rootScope, $scope, PreAccountManageService) {
        $scope.CostTransferList = [];
        $scope.IsShowSearch = true;
       
        $scope.GetPreAccountList = function () {
            PreAccountManageService.GetPreAccountList(function (data) {
                $scope.CostTransferList = data;
            });
        }
        //$scope.GetPreAccountList();

        $scope.ShowPreAccountCostTransferViewModal = function (info) {
            PreAccountManageService.ShowPreAccountCostTransferViewModal(info, function (data) {
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.Alert(data.Msg, 3000);//弹出提示信息,3秒后关闭
                    $scope.GetPreAccountList();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        }
    }

    ckFramework.PreAccountManageController.$inject = injectParams;

    app.register.controller('PreAccountManageController', ckFramework.PreAccountManageController);
});
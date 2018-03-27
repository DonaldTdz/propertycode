'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ImportHistoryCostIndexController = function ($http, $scope, $rootScope) {
        $scope.ImportArrearageList = [];
        $scope.showWarning = false;
        $scope.showSuccess = false;
        $scope.showInfo = true;
        $scope.Msg = "";

        $scope.FileSelected = function () {
            //$("#comDeptId").val($("#SelectDeptId").val());
            //alert($("#comDeptId").val())
            var fileParams = {
                FileElementId: 'historyCostFileToUpload',
                FileFormId: 'historyCostFormFileUpload',
                SubmitAction: 'PropertyMgr/DataInit/ImportHistoryCost',
            };
            ckFramework.FileUploadService.CallBack = function (result) {
                $scope.showInfo = false;
                $scope.Msg = result.Msg;
                if (result.IsSuccess) {
                    //ckFramework.ModalHelper.Alert(result.Msg);
                    //$scope.ImportArrearageList = result.Data;
                    $scope.showWarning = false;
                    $scope.showSuccess = true;
                }
                else {
                    //ckFramework.ModalHelper.Alert(result.Msg);
                    $scope.showWarning = true;
                    $scope.showSuccess = false;
                }
            }
            if (!$scope.$$phase) {
                $scope.$apply();
            }
            ckFramework.FileUploadService.FileSelected(fileParams);
        }

    };

    ckFramework.ImportHistoryCostIndexController.$inject = injectParams;
    app.register.controller('ImportHistoryCostIndexController', ckFramework.ImportHistoryCostIndexController);
});
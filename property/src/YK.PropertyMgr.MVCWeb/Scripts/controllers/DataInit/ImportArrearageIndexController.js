'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.ImportArrearageIndexController = function ($http, $scope, $rootScope) {
        $scope.ImportArrearageList = [];
        $scope.showWarning = false;
        $scope.showSuccess = false;
        $scope.showInfo = true;
        $scope.Msg = "";

        $scope.FileSelected = function () {
            //$("#comDeptId").val($("#SelectDeptId").val());
            var fileParams = {
                FileElementId: 'arrearageFileToUpload',
                FileFormId: 'arrearageFormFileUpload',
                SubmitAction: 'PropertyMgr/DataInit/ImportArrearage',
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

    ckFramework.ImportArrearageIndexController.$inject = injectParams;
    app.register.controller('ImportArrearageIndexController', ckFramework.ImportArrearageIndexController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ReceiptBook/ReceiptBookService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReceiptBookService'];
    ckFramework.ReceiptBookViewController = function ($http, $scope, $rootScope, ReceiptBookService) {
        $scope.FormData = ckFramework.ReceiptBookViewData;

        $scope.ReceiptBookTypeList = ckFramework.ReceiptBookViewData.ReceiptBookTypeList;
        $scope.ReceiptBookStatusList = ckFramework.ReceiptBookViewData.ReceiptBookStatusList;

        $scope.PageViewModel = ckFramework.ReceiptBookViewData.ReceiptBook;


        $scope.PageReadOnly = false;
        $scope.ReceiptExample = "";
        var UsedNumber = $scope.PageViewModel.UsedNumber;

        if (UsedNumber > 0) {
            $scope.PageReadOnly = true;
        }


        if ($scope.PageViewModel.BeginCode == null) {
            $scope.PageViewModel.BeginCode = "";
        }
        if ($scope.PageViewModel.Suffix == null) {
            $scope.PageViewModel.Suffix = 8;
        }
        if ($scope.PageViewModel.Prefix == null) {
            $scope.PageViewModel.Prefix = $('#SelectDeptId').val();
        }

        $scope.ReceiptExample = $scope.PageViewModel.Prefix + padLeft($scope.PageViewModel.BeginCode.toString(), parseInt($scope.PageViewModel.Suffix));


        $scope.CalculationNum = function () {
            var CodestrBegin_1 = padLeft($scope.PageViewModel.BeginCode.toString(), $scope.PageViewModel.Suffix)
            $scope.ReceiptExample = $scope.PageViewModel.Prefix + CodestrBegin_1;
            if ($scope.PageViewModel.Id == null || $scope.PageViewModel.Id == 0) {
                $scope.PageViewModel.ReceiptCurrentNumberView = $scope.PageViewModel.Prefix + CodestrBegin_1;
            }
            if (!$scope.$$phase) {
                $scope.$apply();
            }



        };
        $scope.StatusWarning = function () {

            if ($scope.PageViewModel.ReceiptBookTypeStr == null || $scope.PageViewModel.ReceiptBookTypeStr == "") {
                ckFramework.ModalHelper.Alert("请选择票据类型后再选择");
                return;
            }
            if ($scope.PageViewModel.StatusStr == "1") {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/ReceiptBook/IsReceiptBookStatusPrompt?RceciptType=" + $scope.PageViewModel.ReceiptBookTypeStr + "&ComDeptId=" + $('#SelectDeptId').val() + "&ReceiptBookId=" + $scope.PageViewModel.Id,
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                            ckFramework.ModalHelper.Alert("启用将会使该小区其他同类型票据变为停用");
                    }

                });
            }




        }



        $scope.SaveData = function () {
            var validate = $('#ReceiptBookForm').data('formValidation').validate();
            if (!validate.isValid()) {
                ckFramework.ModalHelper.Alert("新增票据数据验证失败");
                return false;
            }
            $scope.PageViewModel.DeptId = $('#SelectDeptId').val();
            ReceiptBookService.SaveData($scope.PageViewModel, $scope.FormData.ViewType, function (data) {

                if (data.IsSuccess) {

                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                ckFramework.ModalHelper.Alert(data.DataInfo);
            });

        };
    };

    ckFramework.ReceiptBookViewController.$inject = injectParams;
    app.register.controller('ReceiptBookViewController', ckFramework.ReceiptBookViewController);
});

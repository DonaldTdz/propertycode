'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ReceiptBook/ReceiptBookService.js'], function (app) {

    var injectParams = ['$scope', 'ReceiptBookService'];

    ckFramework.ReceiptBookModiflyController = function ($scope, ReceiptBookService) {
        $scope.ReceiptBookTypeList = ckFramework.ReceiptBookModiflyData.ReceiptBookTypeList;
        $scope.PageViewModel = ckFramework.ReceiptBookModiflyData.ReceiptBookModify;
        ckFramework.ReceiptBookModiflyData

        $scope.SaveModifyData = function () {
         
            //var validate = $('#ReceiptBookModifyForm').data('formValidation').validate();
            //if (!validate.isValid()) {
            //    ckFramework.ModalHelper.Alert("票据修改数据验证失败");
            //    return false;
            //}

            var selDept = $("#SelectDeptTypeAndId").val();
            selDept = selDept.split("_");
            if (selDept.length == 2) {
                if (selDept[1] != 11) {
                    ckFramework.ModalHelper.Alert("请选择小区!");
                    return false;
                }
            } else {
                ckFramework.ModalHelper.Alert("请选择小区!");
                return false;
            }

            $scope.PageViewModel.IsModify = false;
            $scope.PageViewModel.ComDeptId = $('#SelectDeptId').val();
            ReceiptBookService.SaveModifyData($scope.PageViewModel, function (data) {
                ckFramework.ModalHelper.Alert(data.DataInfo);
            });

        };

        

    }

    ckFramework.ReceiptBookModiflyController.$inject = injectParams;

    app.register.controller('ReceiptBookModiflyController', ckFramework.ReceiptBookModiflyController);
});
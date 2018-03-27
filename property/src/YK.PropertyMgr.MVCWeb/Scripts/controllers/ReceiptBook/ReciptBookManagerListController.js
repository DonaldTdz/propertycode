'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ReceiptBook/ReceiptBookService.js'], function (app) {

    var injectParams = ['$scope', 'ReceiptBookService'];

    ckFramework.ReciptBookManagerListController = function ($scope, ReceiptBookService) {
        $scope.IsShowSearch = true;

        $scope.ShowReceiptBookView = function (viewType, ReceiptBookId) {

            ReceiptBookService.ShowReceiptBookView(viewType, ReceiptBookId);
        }


        $scope.Save = function () {
            
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

            $scope.ShowReceiptBookView('Add', 0);
        }


    }

    ckFramework.ReciptBookManagerListController.$inject = injectParams;

    app.register.controller('ReciptBookManagerListController', ckFramework.ReciptBookManagerListController);
});
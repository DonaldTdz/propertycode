'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PaymentTasks/PaymentTasksListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PaymentTasksListService'];

    ckFramework.PaymentTasksListController = function ($compile, $rootScope, $scope, PaymentTasksListService) {
        $scope.PaymentTasksListData = ckFramework.PaymentTasksListData;
        $scope.IsShowSearch = true;
        $scope.ShowPaymentTasksView = function (viewType, PaymentTasksId) {
            if ($('#SelectDeptType').val() != DeptTypeInfos.XiaoQu) {
                ckFramework.ModalHelper.Alert("请选择小区");
                return;
            }
            PaymentTasksListService.ShowPaymentTasksView(viewType, PaymentTasksId);
        }

        $scope.ShowPaymentTasksDetailView = function (PaymentTasksId) {
            PaymentTasksListService.ShowPaymentTasksDetailView(PaymentTasksId);
        }
    }

    ckFramework.PaymentTasksListController.$inject = injectParams;

    app.register.controller('PaymentTasksListController', ckFramework.PaymentTasksListController);
});
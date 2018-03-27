'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PaymentTasks/PaymentTaskDetailListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PaymentTaskDetailListService', '$http'];

    ckFramework.PaymentTaskByPayMthodViewListController = function ($compile, $rootScope, $scope, PaymentTaskDetailListService, $http) {
        $scope.Search = {};
        $scope.PayTypeMthodData = "";
        $scope.GetPayTypeMthodDataList = function () {

            $scope.Search.PaymentTaskId = $('#PaymentTaskId').val();
            $scope.Search.PaymentDateMax = $('#PaymentDateMax').val();
            $scope.Search.DeptId = $('#SelectDeptId').val();
            ckFramework.ModalHelper.OpenWait();
            PaymentTaskDetailListService.GetPayTypeMthodDataList($scope.Search, function (data) {


                $scope.PayTypeMthodData = data;
                ckFramework.ModalHelper.CloseWait();
            });
        }

        $scope.GetPayTypeMthodDataList();

    }

    ckFramework.PaymentTaskByPayMthodViewListController.$inject = injectParams;

    app.register.controller('PaymentTaskByPayMthodViewListController', ckFramework.PaymentTaskByPayMthodViewListController);
}); 
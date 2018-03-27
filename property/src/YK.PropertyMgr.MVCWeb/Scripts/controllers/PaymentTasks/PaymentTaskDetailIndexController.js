'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.PaymentTaskDetailIndexController = function ($http, $scope, $rootScope) {
        var myDate = new Date();

        $scope.IsShowSearch = false;
        $scope.IsShowCheck = false;
        $scope.IsAbandonRviewed = false;//弃审
        $scope.IsRevokeRviewed = false;//撤销审核
        $scope.IsRviewed = false;//审核
        $scope.IsRevokePayment = false;//撤销交款
        $scope.IsContributions = false;//交款
        $scope.PayDate = myDate.toLocaleDateString();//时间
        $scope.CheckRemark = ckFramework.PaymentTaskDetailData.paymentTasksDTO.CheckRemark;//审核备注
        $scope.LastPaymenTaskDate = ckFramework.PaymentTaskDetailData.LastPaymentTaskDate;


        switch (ckFramework.PaymentTaskDetailData.paymentTasksDTO.Status) {
            case 0:   //初始
                $scope.IsContributions = true;
                $scope.IsShowSearch = true;
                break;
            case 1://已提交
                $scope.IsAbandonRviewed = true;//弃审
                $scope.IsRviewed = true;//审核
                $scope.IsShowCheck = true;
                break;
            case 2://已审
                $scope.IsRevokeRviewed = true;
                $scope.IsShowCheck = true;  
                break;
            case 3://已退回
                $scope.IsRevokePayment = true;
                break;
        };

        $scope.AbandonRviewed = function () {
            ckFramework.ModalHelper.Confirm("请确认是否弃审？", function () {
                $http({
                    method: 'POST',
                    data: ckFramework.PaymentTaskDetailData.paymentTasksDTO,
                    url: '/PropertyMgr/PaymentTasks/PaymentTasksAbandonRviewed',
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            });
        }

        $scope.PaymentDelete = function () {
            ckFramework.ModalHelper.Confirm("请确认是否撤销交款？", function () {
                $http({
                    method: 'POST',
                    data: ckFramework.PaymentTaskDetailData.paymentTasksDTO,
                    url: '/PropertyMgr/PaymentTasks/PaymentTasksDelete',
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            });
        }

        $scope.PaymentTasksRviewed = function () {
      

            ckFramework.PaymentTaskDetailData.paymentTasksDTO.CheckRemark = $scope.CheckRemark;
            ckFramework.ModalHelper.Confirm("请确认是否审核？", function () {
                $http({
                    method: 'POST',
                    data: ckFramework.PaymentTaskDetailData.paymentTasksDTO,
                    url: '/PropertyMgr/PaymentTasks/PaymentTasksRviewed',
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            });
        }

        $scope.PaymentTasksRevokeRviewed = function () {

            ckFramework.PaymentTaskDetailData.paymentTasksDTO.CheckRemark = $scope.CheckRemark;
            ckFramework.ModalHelper.Confirm("请确认是否撤销审核？", function () {
                $http({
                    method: 'POST',
                    data: ckFramework.PaymentTaskDetailData.paymentTasksDTO,
                    url: '/PropertyMgr/PaymentTasks/PaymentTasksRevokeRviewed',
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            });
        }
            
    };

    ckFramework.PaymentTaskDetailIndexController.$inject = injectParams;
    app.register.controller('PaymentTaskDetailIndexController', ckFramework.PaymentTaskDetailIndexController);
});
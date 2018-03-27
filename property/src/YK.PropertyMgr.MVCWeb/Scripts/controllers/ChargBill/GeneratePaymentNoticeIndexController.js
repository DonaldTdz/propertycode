'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope'];
    ckFramework.GeneratePaymentNoticeIndexController = function ($http, $window, $scope, $rootScope) {
        $scope.SavePaymentNotice = function (nodes, EndDate, Remarks, ComDeptId) {
            $http({
                method: 'POST',
                url: '/PropertyMgr/ChargBill/SavePaymentNotice',
                data: {
                    Nodes: nodes,
                    EndDate: EndDate,
                    Remarks: Remarks,
                    ComDeptId: ComDeptId
                }
            }).success(function (data) {
                if (data.IsSuccess) {
                    var href = "/PropertyMgr/ChargBill/PaymentNoticePrintPDF";
                    $window.open(href, "_blank");
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.CloseWait();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        };
    };

    ckFramework.GeneratePaymentNoticeIndexController.$inject = injectParams;
    app.register.controller('GeneratePaymentNoticeIndexController', ckFramework.GeneratePaymentNoticeIndexController);
});
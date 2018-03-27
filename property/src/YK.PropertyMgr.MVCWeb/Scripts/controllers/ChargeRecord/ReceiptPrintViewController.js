'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeRecord/ChargeRecordListService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'ChargeRecordListService'];
    ckFramework.ReceiptPrintViewController = function ($http,$window, $scope, $rootScope, ChargeRecordListService) {
        $scope.FormData = ckFramework.ReceiptPrintViewData.ChargeRecordInfo;
        $scope.ReceiptPrint = function () {
            ChargeRecordListService.ReceiptPrint($scope.FormData, function (data) {
                if (data.IsSuccess) {
                    //ckFramework.fnPrintClick(data.Data.ChargeRecordId);
                    //$("#btnReceiptPrintPDF").click();
                    var href = ckFramework.ReceiptPrintPDFUrl + "?chargeRecordId=" + data.Data.ChargeRecordId;
                    $window.open(href, "_blank");
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        };
    };

    ckFramework.ReceiptPrintViewController.$inject = injectParams;
    app.register.controller('ReceiptPrintViewController', ckFramework.ReceiptPrintViewController);
});

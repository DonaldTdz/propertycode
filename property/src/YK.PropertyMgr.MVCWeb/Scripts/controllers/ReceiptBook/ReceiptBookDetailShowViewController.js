'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ReceiptBook/ReceiptBookService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReceiptBookService'];
    ckFramework.ReceiptBookDetailShowViewController = function ($http, $scope, $rootScope, ReceiptBookService) {
        $scope.IsShowSearch = true;
        $scope.ReceiptBookId = ckFramework.ReceiptBookDetailShowViewData.ReceiptBookId;

        $scope.ReceiptBookDetailShowExportData = function () {

            var parameters = "?ReceiptBookId=" + $(" #SearchReceiptBookDetailShowContainer input[name='ReceiptBookId']").val()
                + "&ReceiptNum=" + $(" #SearchReceiptBookDetailShowContainer input[name='ReceiptNum']").val()
                + "&ResourcesName=" + $(" #SearchReceiptBookDetailShowContainer input[name='ResourcesName']").val()
                + "&ChargeStartDate=" + $("#SearchReceiptBookDetailShowContainer input[name='ChargeStartDate']").val()
                + "&ChargeEndDate=" + $("#SearchReceiptBookDetailShowContainer input[name='ChargeEndDate']").val();

            var iframe = document.createElement("iframe");


            iframe.src = "PropertyMgr/ReceiptBook/GetReciptBookDetailShowExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }

    };

    ckFramework.ReceiptBookDetailShowViewController.$inject = injectParams;
    app.register.controller('ReceiptBookDetailShowViewController', ckFramework.ReceiptBookDetailShowViewController);
});

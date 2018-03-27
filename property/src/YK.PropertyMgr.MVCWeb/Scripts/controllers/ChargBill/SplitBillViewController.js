'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/DailyChargListService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'DailyChargListService'];
    ckFramework.SplitBillViewController = function ($http, $window, $scope, $rootScope, DailyChargListService) {
        $scope.FormData = ckFramework.SplitBillViewData.ChargBillInfo;
        //$scope.FormData.BeginDate = dateFormatter($scope.FormData.BeginDate);
        //$scope.FormData.EndDate = dateFormatter($scope.FormData.EndDate);
        //alert(JSON.stringify($scope.FormData))
        $scope.SplitBill = function () {
            //$scope.FormData.BeginDate = $("#SplitBillViewForm").find("input[name='BeginDate']").val();
            //$scope.FormData.EndDate = $("#SplitBillViewForm").find("input[name='EndDate']").val();
            //alert(JSON.stringify($scope.FormData))
            DailyChargListService.SplitBill($scope.FormData, function (data) {
                if (data.IsSuccess) {
                    //ckFramework.ModalHelper.Alert(data.Msg);
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.SplitBillData = data.Data;
                    //alert(JSON.stringify(data.Data));
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        };
    };

    ckFramework.SplitBillViewController.$inject = injectParams;
    app.register.controller('SplitBillViewController', ckFramework.SplitBillViewController);
});

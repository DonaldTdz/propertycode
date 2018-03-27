'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {

    var injectParams = ['$scope', 'AlipayPropertyService'];

    ckFramework.AlipayChargeBillViewController = function ($scope, AlipayPropertyService) {
        $scope.IsShowSearch = true;
        $scope.ChargeSubjectList = ckFramework.AlipayChargeBillViewData.ChargeSubjectlist;
        $scope.ChargeSubjectId = 0;

        $scope.SaveUpload = function () {
            var ids = "";
            $("#table_ckFramework_AlipayChargeBillView").find("input[type='checkbox']:checked").each(function () {


             ids = ids + $(this).attr("data") + ",";
            });
            if (ids.length > 0) {
                ids = ids.substr(0, ids.length - 1);
            }
            if (ids.length == 0) {
                ckFramework.ModalHelper.Alert("请选择账单");
                return;
            }
            AlipayPropertyService.SaveUploadChargeBill(ids, ckFramework.AlipayChargeBillViewData.DefaultComDeptId, function (data) {

            });

        }


       
    }

    ckFramework.AlipayChargeBillViewController.$inject = injectParams;

    app.register.controller('AlipayChargeBillViewController', ckFramework.AlipayChargeBillViewController);
});
'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.DeleteChargBillListController = function ($http, $scope, $rootScope) {
        $scope.IsShowSearch = true;
        $scope.ChargeSubjectList = [];
        $scope.ChargeSubjectListId = "0";


        $scope.ChangeSubjectList = function (id) {

            $("select[name=ChargeSubjectId]").find("option").remove();

            $http({
                method: 'POST',
                data: { ComDeptId: id },
                url: '/PropertyMgr/ChargBill/GetComChargeSubjectList',
            }).success(function (data, status, headers, config) {

                if (data.IsSuccess) {

                    $scope.ChargeSubjectList = data.Data;
                    $scope.ChargeSubjectListId = $scope.ChargeSubjectList[0].Id;

                }
                else {

                }
            });

        }

        $scope.ChangeSubjectList($("#SelectDeptId").val());

    };

    ckFramework.DeleteChargBillListController.$inject = injectParams;
    app.register.controller('DeleteChargBillListController', ckFramework.DeleteChargBillListController);
});
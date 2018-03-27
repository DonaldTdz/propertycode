'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope'];
    ckFramework.HouseChargeSubjectListController = function ($http, $scope) {
        $scope.ChargeSubjectList = ckFramework.HouseChargeSubjectData.ChargeSubjectList;
        $scope.ChargeBillInformation = ckFramework.HouseChargeSubjectData.chargeBillInformationDTO;
        $scope.BillPeriodList = ckFramework.HouseChargeSubjectData.DictionaryModels;
        $scope.IsShowHouse = ckFramework.HouseChargeSubjectData.chargeBillInformationDTO.IsShowHouse;
        $scope.IsShowCarPark = ckFramework.HouseChargeSubjectData.chargeBillInformationDTO.IsShowCarPark;
        $scope.IsShowMeter = ckFramework.HouseChargeSubjectData.chargeBillInformationDTO.IsShowMeter;



        //获取字典描述
        $scope.GetBillPeriodName = function (code) {
            for (var j = 0; j < $scope.BillPeriodList.length; j++) {
                if ($scope.BillPeriodList[j].Code == code) {
                    return $scope.BillPeriodList[j].CnName;
                }
            }
            return "";
        }
        //刷新数据
        $scope.RefreshData = function () {
            $http.get("/PropertyMgr/HouseChargeSubject/GetChargeSubjectList?DeptId=" + $('#SelectDeptId').val() + "&DeptType=" + $('#SelectDeptType').val()).success(function (data) {
                //alert(JSON.stringify($scope.ChargeSubjectList))
                if (data != null && data != undefined) {
                    $scope.ChargeSubjectList = data.ChargeSubjectList;
                    $scope.ChargeBillInformation = data.chargeBillInformationDTO;
                    $scope.IsShowHouse = data.chargeBillInformationDTO.IsShowHouse;
                    $scope.IsShowCarPark = data.chargeBillInformationDTO.IsShowCarPark;
                    $scope.IsShowMeter = data.chargeBillInformationDTO.IsShowMeter;

                }
            });
        }
    };

    ckFramework.HouseChargeSubjectListController.$inject = injectParams;
    app.register.controller('HouseChargeSubjectListController', ckFramework.HouseChargeSubjectListController);
});
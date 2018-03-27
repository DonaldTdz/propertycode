'use strict';

//define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Entrance/EntranceViewService.js'], function (app) {
define(['apps/HomeApp'], function (app) {
    var injectParams = ['$scope', '$http'];
    var typeLock = 0;
    ckFramework.EntranceViewController = function ($scope, $http) {

       
        $scope.EntranceViewData = ckFramework.EntranceViewData;
        $scope.Entrance = ckFramework.EntranceViewData.Entrance;
        $scope.DeptVillages = ckFramework.EntranceViewData.DeptVillages;
        $scope.Builds = ckFramework.EntranceViewData.Builds;
        $scope.Units = ckFramework.EntranceViewData.Units;

        $scope.Provinces = ckFramework.EntranceListData.Provinces;/*List页面中首次加载时给定的值*/
        $scope.Cities = ckFramework.EntranceViewData.Cities;
        $scope.Countries = ckFramework.EntranceViewData.Countries;
        $scope.Disabled = "";
        if (!($scope.Entrance.Id > 0)) {
            $scope.Entrance.State = 1;
            var data = ckFramework.ClientCustomSearch;
            if (data[2]["name"] == "DeptType" && data[2]["value"] == "11") {/*小区*/
                $scope.Entrance.VillageID = parseInt(data[0]["value"])
            }
        }


        /*加载市*/
        $scope.ChangeLoadCity = function (ProvinceID) {
            $http({
                url: "/PropertyMgr/Entrance/LoadCity?provinceId=" + ProvinceID,
            }).success(function (data, status, headers, config) {
                $("#drpCountry").empty();
                $scope.Cities = data;
            }).error(function (data, header, config, status) {
                ckFramework.ModalHelper.Alert("市加载信息有误!");
            });
        }
        /*加载区县*/
        $scope.ChangeLoadCountry = function (CityId) {
            $http({
                url: "/PropertyMgr/Entrance/LoadCountry?cityId=" + CityId,
            }).success(function (data, status, headers, config) {
                $scope.Countries = data;
            }).error(function (data, header, config, status) {
                ckFramework.ModalHelper.Alert("区/县加载信息有误!");
            });
        }
        /*加载楼宇*/
        $scope.ChangeLoadBuild = function (villageId) {
            $http({
                url: "/PropertyMgr/Entrance/LoadBuild?villageId=" + villageId,
            }).success(function (data, status, headers, config) {
                $scope.Builds = data;
            }).error(function (data, header, config, status) {
                ckFramework.ModalHelper.Alert("楼宇加载信息有误!");
            });
        }
        /*单元*/
        $scope.ChangeLoadUnit = function (buildId) {

            $http({
                url: "/PropertyMgr/Entrance/LoadUnit?buildId=" + buildId,
            }).success(function (data, status, headers, config) {
                $scope.Units = data;
            }).error(function (data, header, config, status) {
                ckFramework.ModalHelper.Alert("单元加载信息有误!");
            });
        }
        /*控件可用控制s*/
        $scope.EnableControl = function (type) {
            $("#drpUnit option").remove();
            $("#drpBuild option:not(:first)").remove();
            $("#drpVillage").removeAttr("disabled")

            /*未选则*/
            if (type == 0) {
                $("#drpBuild").attr("disabled", "disabled");
                $("#drpUnit").attr("disabled", "disabled");
                $("#drpVillage").attr("disabled", "disabled");
            }
            /*小区锁*/
            if (type == 1) {
                $("#drpBuild").attr("disabled", "disabled");
                $("#drpUnit").attr("disabled", "disabled");

            }     /*楼宇锁*/
            if (type == 2) {
                $("#drpUnit").attr("disabled")
            }
        }



        $scope.Save = function (viewType) {
            if (!($scope.Entrance.VillageID > 0)) {
                ckFramework.ModalHelper.Alert("请选择小区信息!");
                return false;
            }
            if (!($scope.Entrance.ProvinceID > 0)) {
                ckFramework.ModalHelper.Alert("请选择省份信息!");
                return false;
            }
            if (!($scope.Entrance.CityID > 0)) {
                ckFramework.ModalHelper.Alert("请选择市信息!");
                return false;
            }
            if (!($scope.Entrance.CountyID > 0)) {
                ckFramework.ModalHelper.Alert("请选择区、县信息!");
                return false;
            }
            if ($scope.Entrance.Name == "" || $scope.Entrance.Name == null) {
                ckFramework.ModalHelper.Alert("请填写名称!");
                return false;
            }

            if ($scope.Entrance.Address == "" || $scope.Entrance.Address == null) {
                ckFramework.ModalHelper.Alert("请填写地址信息!");
                return false;
            }

        

            $scope.Entrance.State = $("#radState").is(":checked");
            $scope.Disabled = "Disabled";
            $http({
                method: "POST",
                url: viewType == 'Add' ? "/PropertyMgr/Entrance/AddEntrance" : "/PropertyMgr/Entrance/EditEntrance",
                data: $scope.Entrance
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.isRefresh = true;
                if (data.IsSuccess) {
                    $scope.Disabled = "";
                    ckFramework.ModalHelper.Alert(data.ActionInfo);
                    ckFramework.ModalHelper.CloseOpenModal1();
                    ckFramework.EntranceTable.draw();
                }
                else {
                    $scope.Disabled = "";
                    ckFramework.ModalHelper.Alert(data.ActionInfo);
                }

            }).error(function (data, header, config, status) {
                $scope.Disabled = "";
                ckFramework.ModalHelper.Alert(data.ActionInfo);
            });
        }
    };

    ckFramework.EntranceViewController.$inject = injectParams;

    app.register.controller('EntranceViewController', ckFramework.EntranceViewController);
});
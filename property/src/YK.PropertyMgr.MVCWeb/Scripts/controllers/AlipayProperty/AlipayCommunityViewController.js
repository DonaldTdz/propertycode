'use strict';
define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'AlipayPropertyService'];
    ckFramework.AlipayCommunityViewController = function ($http, $window, $scope, $rootScope, AlipayPropertyService) {

        $scope.ProvinceList = ckFramework.AlipayCommunityViewData.ProvinceList;
        $scope.CityList = ckFramework.AlipayCommunityViewData.CityList;
        $scope.DistrictList = ckFramework.AlipayCommunityViewData.DistrictList;
        $scope.PageViewModel = ckFramework.AlipayCommunityViewData.CommunityModel;
        $scope.PageReadOnly = ckFramework.AlipayCommunityViewData.IsShow;
        $scope.ShowQrCode = ckFramework.AlipayCommunityViewData.IsShowQrCode;
        $scope.QrCodeImageUrl = ckFramework.AlipayCommunityViewData.QrCodeUrl;


        $scope.ChangeLoadCity = function (ProvinceID) {
            $http({
                method: 'POST',
                url: "/PropertyMgr/AlipayProperty/LoadCity?provinceId=" + ProvinceID,
            }).success(function (data, status, headers, config) {
                $("#seledistrict_code").empty();
                $scope.CityList = data;

            }).error(function (data, header, config, status) {
                ckFramework.ModalHelper.Alert("市加载信息有误!");
            });
        }

        $scope.ChangeLoadDistrict = function (CityID) {

            $http({
                method: 'POST',
                url: "/PropertyMgr/AlipayProperty/LoadDistrict?CityID=" + CityID,
            }).success(function (data, status, headers, config) {

                $scope.DistrictList = data;

            }).error(function (data, header, config, status) {
                ckFramework.ModalHelper.Alert("县/区加载信息有误!");
            });
        }
        $scope.SaveData = function () {

            $('#AlipayCommunityViewForm').formValidation('revalidateField', 'selecity_code');
            $('#AlipayCommunityViewForm').formValidation('revalidateField', 'seledistrict_code');
            var validate = $('#AlipayCommunityViewForm').data('formValidation').validate();
            if (!validate.isValid()) {
                ckFramework.ModalHelper.Alert("数据验证失败");
                return false;
            }
            AlipayPropertyService.SaveAlipayCommunity($scope.PageViewModel, function (data) {

                if (data.IsSuccess) {
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                ckFramework.ModalHelper.Alert(data.DataInfo);
            });
        }



    };

    ckFramework.AlipayCommunityViewController.$inject = injectParams;
    app.register.controller('AlipayCommunityViewController', ckFramework.AlipayCommunityViewController);
});

'use strict';
define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'AlipayPropertyService'];
    ckFramework.AlipayBasicerviceViewController = function ($http, $window, $scope, $rootScope, AlipayPropertyService) {
        $scope.PageViewModel = ckFramework.AlipayBasicerviceViewData.BaseserviceModel;
        $scope.PageReadOnly = ckFramework.AlipayBasicerviceViewData.IsReadOnly;
        $scope.BasicServiceStatusList = ckFramework.AlipayBasicerviceViewData.BasicServiceStatusList;
        $scope.BasicServiceTypeList = ckFramework.AlipayBasicerviceViewData.BasicServiceTypeList;
        $scope.PageAdd = ckFramework.AlipayBasicerviceViewData.IsAdd;
        $scope.SaveData = function () {
            var validate = $('#AlipayBasicerviceViewForm').data('formValidation').validate();
            if (!validate.isValid()) {
                ckFramework.ModalHelper.Alert("数据验证失败");
                return false;
            }
            AlipayPropertyService.SaveAlipayBasicService($scope.PageViewModel, function (data) {

                if (data.IsSuccess) {
                    ckFramework.ModalHelper.CloseOpenModal1();
                    var divAlipayBasicerviceListController = angular.element(document.getElementById('divAlipayBasicerviceListController')).scope();
                    divAlipayBasicerviceListController.GetCommunityList();
                   // $scope.GetCommunityList
                }
                ckFramework.ModalHelper.Alert(data.DataInfo);
            });
        }



    };

    ckFramework.AlipayBasicerviceViewController.$inject = injectParams;
    app.register.controller('AlipayBasicerviceViewController', ckFramework.AlipayBasicerviceViewController);
});

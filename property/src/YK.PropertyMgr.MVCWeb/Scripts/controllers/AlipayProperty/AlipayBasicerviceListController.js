'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'AlipayPropertyService'];
    ckFramework.AlipayBasicerviceListController = function ($http, $window, $scope, $rootScope, AlipayPropertyService) {


        $scope.AlipayBasicerviceList = {};

        $scope.GetCommunityBasicserviceList = function () {
            var selDeptId = $("#SelectDeptId").val();
            AlipayPropertyService.GetCommunityBasicserviceList(selDeptId, function (data) {

                $scope.AlipayBasicerviceList = data;

            });
        }
        $scope.AlipayCreateBasicservice = function (viewType, ComDeptId) {

            AlipayPropertyService.AlipayCreateBasicservice(viewType, ComDeptId, $scope);
        }




        $scope.GetCommunityBasicserviceList();
    };

    ckFramework.AlipayBasicerviceListController.$inject = injectParams;
    app.register.controller('AlipayBasicerviceListController', ckFramework.AlipayBasicerviceListController);
});

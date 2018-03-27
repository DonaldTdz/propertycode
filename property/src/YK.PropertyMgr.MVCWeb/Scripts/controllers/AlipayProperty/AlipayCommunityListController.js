'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'AlipayPropertyService'];
    ckFramework.AlipayCommunityListController = function ($http, $window, $scope, $rootScope, AlipayPropertyService) {


        $scope.AlipayCommunityList = {};

        $scope.GetCommunityList = function () {
            var selDeptId = $("#SelectDeptId").val();
            AlipayPropertyService.GetCommunityList(selDeptId, function (data) {
               
                $scope.AlipayCommunityList = data;

            });
        }
        $scope.AlipayCreateCommunity = function (viewType, ComDeptId) {

            AlipayPropertyService.ShowAlipayCommunityView(viewType, ComDeptId);
        }

      


        $scope.GetCommunityList();
    };

    ckFramework.AlipayCommunityListController.$inject = injectParams;
    app.register.controller('AlipayCommunityListController', ckFramework.AlipayCommunityListController);
});

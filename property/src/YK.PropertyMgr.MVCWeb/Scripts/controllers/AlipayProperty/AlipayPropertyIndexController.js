'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'AlipayPropertyService'];
    ckFramework.AlipayPropertyIndexController = function ($http, $window, $scope, $rootScope, AlipayPropertyService) {
        $scope.IsOAuth = false;
        $scope.IsShowPage = false;

        $scope.GetOAuthUrl = function () {
            var selDeptId = $("#SelectDeptId").val();
            AlipayPropertyService.GetAlipayOAuthUrl(selDeptId, function (data) {
                if (data.IsSuccess) {


                    var href = data.DataInfo;
                    $window.open(href, "_blank");
                }
            });
        }


        $scope.CheckIsOAuth = function () {
            var selDeptId = $("#SelectDeptId").val();
            AlipayPropertyService.CheckIsOAuth(selDeptId, function (data) {
                $scope.IsOAuth = data;

                $scope.IsShowPage = true;

            });
        }
        $scope.CheckIsOAuth();



    };

    ckFramework.AlipayPropertyIndexController.$inject = injectParams;
    app.register.controller('AlipayPropertyIndexController', ckFramework.AlipayPropertyIndexController);
});

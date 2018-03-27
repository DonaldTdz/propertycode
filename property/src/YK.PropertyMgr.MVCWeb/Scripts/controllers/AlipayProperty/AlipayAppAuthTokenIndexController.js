'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'AlipayPropertyService'];
    ckFramework.AlipayAppAuthTokenIndexController = function ($http, $window, $scope, $rootScope, AlipayPropertyService) {
        $scope.PageModel = {};


        $scope.GetAppAuthTokenQuery = function () {

            var selDeptId = $("#SelectDeptId").val();
            AlipayPropertyService.GetAppAuthTokenQuery(selDeptId, function (data) {
                $scope.PageModel = JSON.parse(data);
                if (!$scope.$$phase) {
                    $scope.$apply();
                }

            });

            $scope.ReGetOAuthUrl = function () {
                ckFramework.ModalHelper.Confirm("重新授权后,请刷新页面", function () {
                    var selDeptId = $("#SelectDeptId").val();
                    AlipayPropertyService.GetAlipayOAuthUrl(selDeptId, function (data) {
                        if (data.IsSuccess) {
                            var href = data.DataInfo;
                            $window.open(href, "_blank");
                        }
                    });
                });
            }

            $scope.RefreshAppAuthToken = function () {
                ckFramework.ModalHelper.Confirm("刷新令牌？", function () {
                    var selDeptId = $("#SelectDeptId").val();
                    AlipayPropertyService.RefreshAppAuthToken(selDeptId, function (data) {

                        $scope.GetAppAuthTokenQuery();

                    });
                });
            }
        }


        $scope.GetAppAuthTokenQuery();


    };

    ckFramework.AlipayAppAuthTokenIndexController.$inject = injectParams;
    app.register.controller('AlipayAppAuthTokenIndexController', ckFramework.AlipayAppAuthTokenIndexController);
});

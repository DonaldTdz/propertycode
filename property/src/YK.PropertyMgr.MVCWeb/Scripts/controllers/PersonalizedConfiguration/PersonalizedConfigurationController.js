'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PersonalizedConfiguration/PersonalizedConfigurationService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'PersonalizedConfigurationService'];

    ckFramework.PersonalizedConfigurationController = function ($http, $scope, $rootScope, PersonalizedConfigurationService) {
        $scope.CommunityConfig = {};
        $scope.EnableList = [{ Value: true, Name: '是' }, { Value: false, Name: '否' }];
        $scope.IsShowData = true;

        $scope.GetCommunityConfig = function () {
            var type = $('#SelectDeptType').val();
            //选择小区
            if (type == 11) {
                $scope.IsShowData = true;
                var deptId = $('#SelectDeptId').val();
                ckFramework.ModalHelper.OpenWait();
                PersonalizedConfigurationService.GetCommunityConfig(deptId, function (data) {
                    $scope.CommunityConfig = data;
                    ckFramework.ModalHelper.CloseWait();
                })
            } else {
                $scope.IsShowData = false;
            }
        };

        $scope.GetCommunityConfig();

        $scope.SaveData = function () {
            ckFramework.ModalHelper.OpenWait();
            PersonalizedConfigurationService.SaveCommunityConfig($scope.CommunityConfig, function (data) {
                $scope.GetCommunityConfig();
                ckFramework.ModalHelper.Alert(data.Msg, 3000);
                ckFramework.ModalHelper.CloseWait();
            });
        };
    }

    ckFramework.PersonalizedConfigurationController.$inject = injectParams;
    app.register.controller('PersonalizedConfigurationController', ckFramework.PersonalizedConfigurationController);
});
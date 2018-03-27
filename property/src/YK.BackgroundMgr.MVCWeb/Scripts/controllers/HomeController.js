'use strict';

define(['apps/HomeApp', 'services/HomeService'], function (app) {
    var injectParams = ['$scope', '$rootScope', '$location', 'HomeService'];

    ckFramework.HomeController = function ($scope,$rootScope, $location, HomeService) {
        $scope.homeData = ckFramework.HomeData; // 设置后台传递过来的Model对象
        $scope.ClientMessage = ckFramework.ClientMessage.GetMessage();
        $scope.OldPassword = "";
        $scope.NewPassword = "";
        $scope.CurrentSecondModalId = -1;

        //// 设置当前项目Modules
        //var SetCurrentModules = function () {
        //    $scope.homeData.RootModules = HomeService.GetRootModules(ckFramework.HomeData.ModuleInfos);
        //    if ($scope.homeData.RootModules.length == 0) {
        //        $location.path("/Error404");
        //        return false;
        //    }
        //    $scope.homeData.CurrentModule = $scope.homeData.CurrentModule || {};
        //    $scope.homeData.CurrentModule.Id = $scope.homeData.RootModules[0].Id;
        //    $scope.homeData.CurrentModule.Name = ckFramework.HomeData.Language == "zh-CN" ? $scope.homeData.RootModules[0].CnName : $scope.homeData.RootModules[0].EnName;
        //    return true;
        //};

        var SetCurrentLeftModules = function () {
            $scope.homeData.LeftModules = HomeService.GetChildrnModules(ckFramework.HomeData.ModuleInfos, $scope.homeData.CurrentModule.Id);
            if ($scope.homeData.LeftModules.length == 0) {
                $location.path("/Error404");
                return;
            }
            SetCurrentLeftModule($scope.homeData.LeftModules[0]);
        };

        var SetCurrentLeftModule = function (leftModule, isFromNav) {
            $scope.homeData.CurrentLeftModule = $scope.homeData.CurrentLeftModule || {};
            $scope.homeData.CurrentLeftModule.Id = leftModule.Id;
            $scope.homeData.CurrentLeftModule.Name = ckFramework.HomeData.Language == "zh-CN" ? leftModule.CnName : leftModule.EnName;
            $scope.homeData.CurrentLeftModule.Description = leftModule.Description;
            $scope.homeData.CurrentLeftModule.Url = leftModule.Url;
            $scope.homeData.CurrentLeftModule.PId = leftModule.PId;

            if (isFromNav) {
                $('#' + leftModule.PId).parent().parent().children().removeClass('active').children('ul').removeClass('menu-open').hide();
                $('#' + leftModule.PId).next().addClass('menu-open').show().parent().addClass('active');
            }

            if (leftModule.Url) {
                $location.path(leftModule.Url);
            }
        }

        SetCurrentLeftModules();

        $scope.InitCurrentLeftModule = function () {
            if (!$scope.homeData.CurrentLeftModule.Url)
            {
                var leftModules = HomeService.GetChildrnModules(ckFramework.HomeData.ModuleInfos, $scope.homeData.CurrentLeftModule.Id);
                if ($scope.homeData.LeftModules.length == 0) {
                    $location.path("/Error404");
                    return;
                }
                SetCurrentLeftModule(leftModules[0],true);
            }            
        };

        // 选择Module
        $scope.ChangeModule = function (moduleId, moduleName) {
            ckFramework.IsFirstAdminLTETree = true;
            if ($scope.homeData.CurrentModule.Id != moduleId) {
                $scope.homeData.CurrentModule.Id = moduleId;
                $scope.homeData.CurrentModule.Name = moduleName;
                SetCurrentLeftModules();
            }
        };

        // 选择Module
        $scope.ChangeLeftModule = function (moduleId) {
            var selectedLeftModule = HomeService.FindLeftModule($scope.homeData.LeftModules, moduleId);
            if (selectedLeftModule.Url) {
                SetCurrentLeftModule(selectedLeftModule);
            }
        };

        $scope.ChangeLeftModuleFromNav = function (moduleId) {
            var selectedLeftModule = HomeService.FindLeftModule($scope.homeData.LeftModules, moduleId);
            if (selectedLeftModule.Url) {
                SetCurrentLeftModule(selectedLeftModule, true);
            }
        };

        // 退出登录
        $scope.Logout = function () {
            window.location.href = ckFramework.UrlLogout;
        };
        
        $rootScope.CheckPermission = function (action) {
            //return true;
            //alert(action)
            //alert(ckFramework.HomeData.OperateCodeAndRoleInfos.length)
            var userOperates = action.split(';');
            for (var i = 0; i < userOperates.length; i++) {
                var tempUserOperate = userOperates[i];
                for (var j = 0; j < ckFramework.HomeData.OperateCodeAndRoleInfos.length; j++) {
                    var tempCodeAndRole = ckFramework.HomeData.OperateCodeAndRoleInfos[j];
                    if (tempCodeAndRole.Code == tempUserOperate) {
                        return true;
                    }
                }
            }
            return false;
        };
        //$scope.CheckPermission("Refund");
        //$.AdminLTE.tree('.sidebar');
    }
    ckFramework.HomeController.$inject = injectParams;
    app.controller('HomeController', ckFramework.HomeController);
});
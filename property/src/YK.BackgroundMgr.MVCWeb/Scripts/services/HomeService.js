'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http'];
    ckFramework.HomeService = function ($http) {
        var homeService = {};

        var SetThirdLevelModule = function (ModuleInfos, currentModule) {
            currentModule.ChildrenModule = [];
            for (var i = 0; i < ModuleInfos.length; i++) {
                if (ModuleInfos[i].PId == currentModule.Id && ModuleInfos[i].IsUsed && ModuleInfos[i].IsShow) {
                    var addModule = {
                        Id: ModuleInfos[i].Id,
                        CnName: ModuleInfos[i].CnName,
                        EnName: ModuleInfos[i].EnName,
                        Iconic: ModuleInfos[i].Iconic,
                        Url: ModuleInfos[i].Url,
                        NavImage: ModuleInfos[i].NavImage,
                        Description: ModuleInfos[i].Description,
                        PId: ModuleInfos[i].PId
                    };

                    currentModule.ChildrenModule.push(addModule);
                }
            }
        }

        var CheckHasChild = function (ModuleInfos, moduleId) {
            for (var i = 0; i < ModuleInfos.length; i++) {
                if (ModuleInfos[i].PId == moduleId && ModuleInfos[i].IsUsed && ModuleInfos[i].IsShow) {
                    return true;
                }
            }
            return false;
        }

        homeService.GetRootModules = function (ModuleInfos) {
            var rootModules = [];
            for (var i = 0; i < ModuleInfos.length; i++) {
                if (ModuleInfos[i].PId == 0 && ModuleInfos[i].IsUsed && ModuleInfos[i].IsShow) {
                    if (CheckHasChild(ModuleInfos, ModuleInfos[i].Id)) {
                        rootModules.push(ModuleInfos[i]);
                        return rootModules;
                    }
                }
            }

            return rootModules;
        };

        // 获取左侧导航树信息
        homeService.GetChildrnModules = function (ModuleInfos, moduleId) {
            var navModules = [];
            for (var i = 0; i < ModuleInfos.length; i++) {
                if (ModuleInfos[i].PId == moduleId && ModuleInfos[i].IsUsed && ModuleInfos[i].IsShow) {
                    var addModule = {
                        Id: ModuleInfos[i].Id,
                        CnName: ModuleInfos[i].CnName,
                        EnName: ModuleInfos[i].EnName,
                        Iconic: ModuleInfos[i].Iconic,
                        Url: ModuleInfos[i].Url,
                        NavImage: ModuleInfos[i].NavImage,
                        Description: ModuleInfos[i].Description,
                        PId: ModuleInfos[i].PId
                    };
                    SetThirdLevelModule(ModuleInfos, addModule)
                    navModules.push(addModule);
                }
            }

            return navModules;
        }

        // 获取左侧导航树信息
        homeService.FindLeftModule = function (LeftModules, moduleId) {
            for (var i = 0; i < LeftModules.length; i++) {
                for (var j = 0; j < LeftModules[i].ChildrenModule.length; j++) {
                    if (LeftModules[i].ChildrenModule[j].Id == moduleId) {
                        return LeftModules[i].ChildrenModule[j];
                    }
                }
                if (LeftModules[i].Id == moduleId) {
                    return LeftModules[i];
                }
            }

            return null;
        }

        homeService.UpdatePassword = function (strOldPassword, strNewPassword) {
            if (!strOldPassword) {
                ckFramework.ModalHelper.Alert("新输入旧密码！");
                return;
            }
            if (!strNewPassword) {
                ckFramework.ModalHelper.Alert("新输入新密码！");
                return;
            }

            $http({
                method: 'GET',
                url: "/AdminUser/UpdatePassword?strOldPassword=" + strOldPassword + "&strNewPassword=" + strNewPassword,
            }).success(function (data, status, headers, config) {
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.Alert(data.ActionInfo, function () { window.location.href = ckFramework.UrlLogout; });
                }
                else {
                    ckFramework.ModalHelper.Alert(data.ActionInfo);
                }
            });
        }

        return homeService;
    };

    ckFramework.HomeService.$inject = injectParams;

    app.factory('HomeService', ckFramework.HomeService);
});

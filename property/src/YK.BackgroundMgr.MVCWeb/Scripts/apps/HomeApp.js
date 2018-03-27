'use strict';

define(['directives/ModalDirective', 'directives/KeydownDirective', 'directives/ConvertToNumberDirective', 'directives/DatetimePickerDirective', 'directives/OperateAuthorityDirective', 'directives/LogYearMonthDirective', 'directives/DictionaryDirective', 'configs/routeResolver'], function () {
    ckFramework.HomeApp = angular.module('HomeApp', ['ngRoute', 'routeResolverServices', 'ui.bootstrap', 'ui.utils']);

    ckFramework.HomeApp.config(['$routeProvider', 'routeResolverProvider', '$controllerProvider',
                '$compileProvider', '$filterProvider', '$provide', '$httpProvider',
        function ($routeProvider, routeResolverProvider, $controllerProvider,
                  $compileProvider, $filterProvider, $provide, $httpProvider) {

            ckFramework.HomeApp.register =
            {
                controller: $controllerProvider.register,
                directive: $compileProvider.directive,
                filter: $filterProvider.register,
                factory: $provide.factory,
                service: $provide.service,
            };

            var route = routeResolverProvider.route;

            ckFramework.HomeData.ModuleInfos.forEach(function (modelItem) {
                if (modelItem.Url) {

                    $routeProvider.when("/" + modelItem.Url, route.resolve(modelItem.Url, modelItem.JSController));
                }

                $routeProvider.when("/", {});
            });

            $routeProvider.when('/Error404', {
                templateUrl: 'Error/PageNotFind'
            })
            .when('/Error501', {
                templateUrl: 'Error/PageError'
            });
        }]);

    // 注册指令
    ckFramework.HomeApp.directive('modalDirective', ckFramework.ModalDirective);
    ckFramework.HomeApp.directive('modalConfirmDirective', ckFramework.ModalConfirmDirective);
    ckFramework.HomeApp.directive('keydownDirective', ckFramework.KeydownDirective);
    ckFramework.HomeApp.directive('convertToNumber', ckFramework.ConvertToNumberDirective);
    ckFramework.HomeApp.directive('dateTimePicker', ckFramework.DatetimePickerDirective); 
    ckFramework.HomeApp.directive('operateAuthority', ckFramework.OperateAuthorityDirective); 
    ckFramework.HomeApp.directive('logYearMonth', ckFramework.LogYearMonthDirective);
    ckFramework.HomeApp.directive('dictionary', ckFramework.DictionaryDirective);
    
    ckFramework.IsLoadAdminLTE = false;

    ckFramework.HomeApp.run(['$rootScope', '$location', function ($rootScope, $location) {
        $rootScope.$on('$routeChangeStart', function (evt, next, current) {
            if (!ckFramework.IsLoadAdminLTE) {
                $.AdminLTE.tree('.sidebar');
                ckFramework.IsLoadAdminLTE = true;
                var homeController = angular.element(document.getElementById('htmlHomeController')).scope();
                homeController.InitCurrentLeftModule();
            }
            if (next.$$route && !next.$$route.originalPath) {
                evt.preventDefault();
            }
            else {
                ckFramework.ModalHelper.OpenWait();
            }
        });

        $rootScope.$on('$routeChangeSuccess', function (evt, next, current) {
            setTimeout(function () { ckFramework.ModalHelper.CloseWait(); }, 200);
        });

        $rootScope.$on('$routeChangeError', function (evt, next, current) {
            setTimeout(function () { ckFramework.ModalHelper.CloseWait(); }, 200);
        });
        $rootScope.$on('$routeUpdate', function (evt, next, current) {
            setTimeout(function () { ckFramework.ModalHelper.CloseWait(); }, 200);
        });
    }]);

    return ckFramework.HomeApp;
});

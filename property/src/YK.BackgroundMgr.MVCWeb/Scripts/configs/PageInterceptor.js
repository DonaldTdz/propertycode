'use strict';

define(['apps/HomeApp'], function (app) {
    app.config(['$httpProvider', function ($httpProvider) {
        var injectParams = ['$q', '$location', '$rootScope'];
        ckFramework.PageInterceptor = function ($q, $location, $rootScope) {
            var errorMessage = ckFramework.ClientMessage.GetMessage().SesstionTimeoutError;
            return {
                request: function (request) {
                    return request || $q.when(request);
                },
                requestError: function (rejection) {
                    return $q.reject(rejection);
                },
                response: function (response) {
                    if (response.status === 401) {
                        ckFramework.ModalHelper.CloseWait();
                        ckFramework.ModalHelper.CloseOpenModal();
                        ckFramework.ModalHelper.Alert(errorMessage, function () { window.location.href = ckFramework.UrlLogout; });

                    }
                    return response || $q.when(response);
                },
                responseError: function (rejection) {
                    ckFramework.ModalHelper.CloseWait();
                    ckFramework.ModalHelper.CloseOpenModal();
                    if (rejection.status === 401) {
                        ckFramework.ModalHelper.Alert(errorMessage, function () { window.location.href = ckFramework.UrlLogout; });
                    }
                    if (rejection.status === 404) { // 如果返回404错误，显示404错误页面
                        $location.path('/Error404');
                        $location.replace();
                    }
                    if (rejection.status === 501 || rejection.status === 500) { // 如果返回404错误，显示404错误页面
                        $location.path('/Error501');
                        $location.replace();
                    }
                    return $q.reject(rejection);
                }
            }
        };
        ckFramework.PageInterceptor.$inject = injectParams;

        $httpProvider.interceptors.push(ckFramework.PageInterceptor);

    }]);

});
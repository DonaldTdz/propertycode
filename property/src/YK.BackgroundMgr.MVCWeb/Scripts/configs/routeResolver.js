'use strict';

define([], function () {
    var routeResolver = function () {
        this.$get = function () {
            return this;
        };

        this.route = function () {

            var resolve = function (templateUrl, dependencies) {
                var routeDef = {};
                routeDef.templateUrl = templateUrl;
                routeDef.secure = false;
                routeDef.resolve = {
                    load: ['$q', '$rootScope', function ($q, $rootScope) {
                        var tempdependencies;
                        if (dependencies)
                        {
                            tempdependencies = [dependencies + '.js'];
                        }
                        return resolveDependencies($q, $rootScope, tempdependencies || []);
                    }]
                };

                return routeDef;
            },

            resolveDependencies = function ($q, $rootScope, dependencies) {
                var defer = $q.defer();
                require(dependencies, function () {
                    $rootScope.$apply()
                    defer.resolve();
                });

                return defer.promise;
            };

            return {
                resolve: resolve
            }
        }();
    };

    var servicesApp = angular.module('routeResolverServices', []);
    servicesApp.provider('routeResolver', routeResolver);
});

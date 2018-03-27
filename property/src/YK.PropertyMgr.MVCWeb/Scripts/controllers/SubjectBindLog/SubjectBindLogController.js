'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.BindLogListController = function ($http, $scope, $rootScope) {
        $scope.Subject;
        $scope.Subjects = ckFramework.BindLogListData.Subjects;
    };
    ckFramework.BindLogListController.$inject = injectParams;
    app.register.controller('BindLogListController', ckFramework.BindLogListController);
});

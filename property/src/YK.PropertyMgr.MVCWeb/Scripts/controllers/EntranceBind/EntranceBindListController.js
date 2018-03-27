'use strict';
define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope'];
    ckFramework.EntranceBindListController = function ($http, $scope, $rootScope) {
        $scope.IsShowSearch = true;
        $scope.EntranceBindListData = ckFramework.EntranceBindListData;
        //$scope.ShowEntranceView = function (viewType, EntranceId) {

        //    EntranceListService.ShowEntranceView(viewType, EntranceId);
        //}
        //$scope.DeleteEntrance = function (EntranceId) {
        //    EntranceListService.DeleteEntrance(EntranceId);
        //}
    };
    ckFramework.EntranceBindListController.$inject = injectParams;
    app.register.controller('EntranceBindListController', ckFramework.EntranceBindListController);
});

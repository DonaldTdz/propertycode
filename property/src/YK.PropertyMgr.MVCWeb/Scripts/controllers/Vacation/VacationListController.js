'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Vacation/VacationListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope','VacationListService'];

    ckFramework.VacationListController = function ($compile, $rootScope, $scope, VacationListService) {
        $scope.VacationListData = ckFramework.VacationListData;
        $scope.IsShowSearch = true;

        $scope.ShowVacationView = function (viewType,VacationId)
        {
            VacationListService.ShowVacationView(viewType, VacationId);
        }

        $scope.DeleteVacation = function (VacationId)
        {
            VacationListService.DeleteVacation(VacationId);
        }
    }

    ckFramework.VacationListController.$inject = injectParams;

    app.register.controller('VacationListController', ckFramework.VacationListController);
});
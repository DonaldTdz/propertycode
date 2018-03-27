'use strict';
define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/SubjectHouseRef/SubjectHouseRefListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'SubjectHouseRefListService'];

    ckFramework.SubjectHouseRefListController = function ($compile, $rootScope, $scope, SubjectHouseRefListService) {
        //$scope.ChargeSubjectListData = ckFramework.ChargeSubjectListData;
        //$scope.IsShowSearch = true;

        //$scope.ShowChargeSubjectView = function (viewType, SubjectId) {

        //    ChargeSubjectListService.ShowChargeSubjectView(viewType, SubjectId);
        //}

        //$scope.DeleteChargeSubject = function (SubjectId) {
        //    ChargeSubjectListService.DeleteChargeSubject(SubjectId);
        //}
    }

    ckFramework.SubjectHouseRefListController.$inject = injectParams;

    app.register.controller('SubjectHouseRefListController', ckFramework.SubjectHouseRefListController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/BindSubjectBySingleRes/BindSubjectBySingleResService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'BindSubjectBySingleResService'];
    ckFramework.ResTypeIndexController = function ($compile, $rootScope, $scope, BindSubjectBySingleResService) {
        $scope.Save = function () {
            var isPostBillSave = true;
            /*根据科目绑定资源*/
            BindSubjectBySingleResService.SaveSubjectBySingleSubject(isPostBillSave);
        }
    }

    ckFramework.ResTypeIndexController.$inject = injectParams;

    app.register.controller('ResTypeIndexController', ckFramework.ResTypeIndexController);
});
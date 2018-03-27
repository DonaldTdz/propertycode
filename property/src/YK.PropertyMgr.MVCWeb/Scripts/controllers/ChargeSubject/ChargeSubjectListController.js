'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargeSubject/ChargeSubjectListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'ChargeSubjectListService'];

    ckFramework.ChargeSubjectListController = function ($compile, $rootScope, $scope, ChargeSubjectListService) {
        $scope.ChargeSubjectListData = ckFramework.ChargeSubjectListData;
        $scope.IsShowSearch = true;
       
        $scope.ShowChargeSubjectView = function (viewType, SubjectId) {
         
            ChargeSubjectListService.ShowChargeSubjectView(viewType, SubjectId);
        }

        $scope.DeleteChargeSubject = function (SubjectId) {
            ChargeSubjectListService.DeleteChargeSubject(SubjectId);
        }
        $scope.Save = function () {
            //alert($("#SelectDeptTypeAndId").val())
            var selDept = $("#SelectDeptTypeAndId").val();
            selDept = selDept.split("_");
            if (selDept.length == 2) {
                if (selDept[1] != 11) {
                    ckFramework.ModalHelper.Alert("请选择小区!");
                    return false;
                }
            } else {
                ckFramework.ModalHelper.Alert("请选择小区!");
                return false;
            }
            
            $scope.ShowChargeSubjectView('Add', 0);
        }
    }
    
    ckFramework.ChargeSubjectListController.$inject = injectParams;

    app.register.controller('ChargeSubjectListController', ckFramework.ChargeSubjectListController);
});
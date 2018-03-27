'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PaymentTasks/PaymentTasksAddListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PaymentTasksAddListService', '$http'];

    ckFramework.PaymentTasksAddListController = function ($compile, $rootScope, $scope, PaymentTasksAddListService, $http) {
        $scope.ChargeRecordListData = ckFramework.ChargeRecordListData;
        $scope.IsShowSearch = true;
        $scope.ComDeptId = ckFramework.ChargeRecordListData.ComDeptId;
    
        $scope.SaveData = function () {




            var ids = ckFramework.fnGetSelectdChargeRecordIds();


            if (ids.length <= 0) {
                ckFramework.ModalHelper.Alert("请选择缴费记录后再提交");
                return;
            }
             

            var PageModel = new Object();
            PageModel.IdStr = ids

            $http({
                method: 'POST',
                data: PageModel,
                url: '/PropertyMgr/PaymentTasks/PaymentTasksAdd',
            }).success(function (data, status, headers, config) {
                ckFramework.ModalHelper.isRefresh = true;
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.CloseOpenModal1();
                    ckFramework.ModalHelper.Alert(data.Msg);
                 

                 
                    if (!$scope.$$phase) {
                        $scope.$apply();
                    }

                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                  
                }
            });





        }



    }

    ckFramework.PaymentTasksAddListController.$inject = injectParams;

    app.register.controller('PaymentTasksAddListController', ckFramework.PaymentTasksAddListController);
});
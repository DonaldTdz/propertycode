'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.PaymentTasksListService = function ($http, $q, $compile, $rootScope) {
        var PaymentTasksListService = {};

        PaymentTasksListService.ShowPaymentTasksView = function (viewType, PaymentTasksId) {
           
            var loadUrl;
            switch (viewType) {
                case "Add":
                 
                    loadUrl = ckFramework.PaymentTasksViewUrlLook + '?ComDeptId=' + $('#SelectDeptId').val() + '&PaymentTaskId=0';
                    break;
                case "Edit":

                    loadUrl = ckFramework.PaymentTasksViewUrlLook + '?PaymentTaskId=' + PaymentTasksId + '&ComDeptId=0';
                    break;
                case "Look":
                    loadUrl = ckFramework.PaymentTasksViewUrlLook + '?PaymentTaskId=' + PaymentTasksId + '&ComDeptId=0';
                 
                    break;
            }
            var $modal = $('#divPaymentTasksModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/PaymentTasks/PaymentTaskDetailIndexController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: false,
                        keyboard: false,
                        width: 1100
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.PaymentTasksTable.draw();
                        }
                        $(this).off('hide.bs.modal');
                    });
                    $compile($('#divPaymentTaskDetailIndexViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

    
        PaymentTasksListService.ShowPaymentTasksDetailView = function (PaymentTasksId) {
            var loadUrl = ckFramework.PaymentTasksViewUrlLook + '?PaymentTaskId=' + PaymentTasksId + '&ComDeptId=' + $('#SelectDeptId').val();
           
            var $modal = $('#divPaymentTasksModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/PaymentTasks/PaymentTaskDetailIndexController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: false,
                        keyboard: false,
                        width: 1100
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.on('hide', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.PaymentTasksTable.draw();
                        }
                    });
                    $compile($('#divPaymentTaskDetailIndexViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }
        return PaymentTasksListService;
    };

    ckFramework.PaymentTasksListService.$inject = injectParams;
    app.register.factory('PaymentTasksListService', ckFramework.PaymentTasksListService);
});
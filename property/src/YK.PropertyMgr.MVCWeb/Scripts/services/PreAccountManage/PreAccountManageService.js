'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.PreAccountManageService = function ($http, $q, $compile, $rootScope) {
        var PreAccountManageService = {};

        PreAccountManageService.GetPreAccountList = function (callback) {
            //alert($('#SelectDeptId').val())
            //alert($('#SelectDeptType').val())
            setTimeout(function () {
                $http({
                    method: 'GET',
                    url: '/PropertyMgr/PreAccountManage/GetPreAccountList?deptId=' + $('#SelectDeptId').val() + '&deptType=' + $('#SelectDeptType').val()//,
                    //data: {
                    //    deptId: $('#SelectDeptId').val(),
                    //    deptType: $('#SelectDeptType').val()
                    //}
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        PreAccountManageService.ShowPreAccountCostTransferViewModal = function (pinfo, callback) {
            var loadUrl = ckFramework.PreAccountCostTransferViewUrl;
            //alert(loadUrl);
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, { deptId: $('#SelectDeptId').val(), deptType: $('#SelectDeptType').val(), info: pinfo },
                function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/PreAccountManage/PreAccountCostTransferViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;
                    $modal.off('hide.bs.modal').on('hide.bs.modal', function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            callback(ckFramework.ModalHelper.PreAccountCostTransferData);
                        }
                    });
                    $compile($('#divPreAccountCostTransferViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        PreAccountManageService.PreAccountCostTransfer = function (info, callback) {               
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/PreAccountManage/PreAccountCostTransfer',
                    data: {
                        transfer: info
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        PreAccountManageService.PreCostBatchDeduction = function (hsids, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/PreAccountManage/PreCostBatchDeduction',
                    data: {
                        houseDeptSubjectIds: hsids
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        return PreAccountManageService;
    };

    ckFramework.PreAccountManageService.$inject = injectParams;
    app.register.factory('PreAccountManageService', ckFramework.PreAccountManageService);
});
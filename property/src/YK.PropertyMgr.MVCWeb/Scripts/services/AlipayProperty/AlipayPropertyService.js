'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.AlipayPropertyService = function ($http, $q, $compile, $rootScope) {
        var AlipayPropertyService = {};
        AlipayPropertyService.GetAlipayOAuthUrl = function (DeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/GetAlipayOAuth?DeptId=" + DeptId,
                }).success(function (data, status, headers, config) {
                    callback(data);

                });
            }, 200);
        };


        AlipayPropertyService.CheckIsOAuth = function (DeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/CheckIsOAuth?DeptId=" + DeptId,
                }).success(function (data, status, headers, config) {

                    callback(data);
                });
            }, 200);
        };

        AlipayPropertyService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        };

        AlipayPropertyService.GetAppAuthTokenQuery = function (DeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/GetAppAuthTokenQuery?DeptId=" + DeptId,
                }).success(function (data, status, headers, config) {

                    callback(data);
                });
            }, 200);
        };

        AlipayPropertyService.RefreshAppAuthToken = function (DeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/RefreshAppAuthToken?DeptId=" + DeptId,
                }).success(function (data, status, headers, config) {

                    callback(data);
                });
            }, 200);
        };

        AlipayPropertyService.GetCommunityList = function (DeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/GetAlipayCommunityList?DeptId=" + DeptId,
                }).success(function (data, status, headers, config) {

                    callback(data);
                });
            }, 200);
        };

        AlipayPropertyService.ShowAlipayCommunityView = function (viewType, ComDeptId) {
            var selDeptId = $("#SelectDeptId").val();
            var loadUrl;
            switch (viewType) {
                case "Add":
                    loadUrl = "/PropertyMgr/AlipayProperty/AlipaiCommunityCreateViewAdd?ComDeptId=" + ComDeptId;
                    break;
                case "Edit":
                    loadUrl = "/PropertyMgr/AlipayProperty/AlipaiCommunityCreateViewEdit?AlipayCommunityId=" + ComDeptId;
                    break;
                case "Show":
                    loadUrl = "/PropertyMgr/AlipayProperty/AlipaiCommunityCreateViewShow?AlipayCommunityId=" + ComDeptId;
                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/AlipayProperty/AlipayCommunityViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });


                    //$modal.off('hide.bs.modal').on("hide.bs.modal", function () {
                    //    if (ckFramework.ModalHelper.isRefresh) {
                    //        ckFramework.ReciptBookManagerTable.draw();
                    //    }
                    //});
                    $compile($('#divAlipayCommunityViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        AlipayPropertyService.SaveAlipayCommunity = function (formData, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: formData,
                    url: "/PropertyMgr/AlipayProperty/SaveAlipayCommunity",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        callback(data);
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            }, 200);
        };

        AlipayPropertyService.GetCommunityBasicserviceList = function (DeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/GetAlipayCommunityBaseServerList?DeptId=" + DeptId,
                }).success(function (data, status, headers, config) {

                    callback(data);
                });
            }, 200);
        };

        AlipayPropertyService.AlipayCreateBasicservice = function (viewType, ComDeptId, pageScope) {
            var selDeptId = $("#SelectDeptId").val();
            var loadUrl;
            switch (viewType) {
                case "Add":
                    loadUrl = "/PropertyMgr/AlipayProperty/AlipaiCommunityBasicserviceAddView?AlipayCommunityId=" + ComDeptId;
                    break;
                case "Edit":
                    loadUrl = "/PropertyMgr/AlipayProperty/AlipaiCommunityBasicserviceEditView?AlipayCommunityId=" + ComDeptId;
                    break;
                case "Show":
                    loadUrl = "/PropertyMgr/AlipayProperty/AlipaiCommunityBasicserviceShowView?AlipayCommunityId=" + ComDeptId;
                    break;
            }
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/AlipayProperty/AlipayBasicerviceViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });


                    $modal.off('hide.bs.modal').on("hide.bs.modal", function () {
                        pageScope.GetCommunityBasicserviceList();
                    });
                    $compile($('#divAlipayBasicerviceViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        AlipayPropertyService.SaveAlipayBasicService = function (formData, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: formData,
                    url: "/PropertyMgr/AlipayProperty/SaveAlipayBasicService",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        callback(data);
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            }, 200);
        };

        AlipayPropertyService.GetCommunityListByPropertyDeptId = function (DeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/GetComDeptListByPropertyDeptId?ProDeptId=" + DeptId,
                }).success(function (data, status, headers, config) {

                    callback(data);
                });
            }, 200);
        };

        AlipayPropertyService.ShowUploadAlipayRoomView = function (ComDeptId) {

            var loadUrl = "/PropertyMgr/AlipayProperty/AlipayRoomView?ComDeptId=" + ComDeptId;

            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/AlipayProperty/AlipayRoomViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 500,
                        maxHeight: 600
                    });
                    $modal.off('hide.bs.modal').on("hide.bs.modal", function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.AlipayRoomTable.draw();
                        }
                    });
                    $compile($('#divAlipayRoomViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        AlipayPropertyService.SaveAlipayRoomUpload = function (formData, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    data: formData,
                    url: "/PropertyMgr/AlipayProperty/SaveAlipayRoomUpload",
                }).success(function (data, status, headers, config) {
                    ckFramework.ModalHelper.isRefresh = true;
                    if (data.IsSuccess) {
                        callback(data);
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                });
            }, 200);
        };


        AlipayPropertyService.BatchDelete = function (Ids, ComDeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/DeleteAlipayRoom?Ids=" + Ids + "&ComDeptId=" + ComDeptId,
                }).success(function (data, status, headers, config) {
                    if (data.IsSuccess) {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.createAlipayRoomListData.draw();
                        }
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }

                });
            }, 200);
        };


        AlipayPropertyService.SynchronizationRoomInfo = function (ComDeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/SynchronizationRoomInfo?ComDeptId=" + ComDeptId,
                }).success(function (data, status, headers, config) {
                    callback(data);
                });
            }, 200);
        };


        AlipayPropertyService.ShowUploadAlipayChargeBillView = function (ComDeptId) {

            var loadUrl = "/PropertyMgr/AlipayProperty/AlipayChargeBillView?ComDeptId=" + ComDeptId;

            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            $modal.load(loadUrl, '', function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/AlipayProperty/AlipayChargeBillViewController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 900,
                        maxHeight: 600
                    });
                    $modal.off('hide.bs.modal').on("hide.bs.modal", function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.AlipayChargeBillTable.draw();
                        }
                    });
                    $compile($('#divAlipayChargeBillViewController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }


        AlipayPropertyService.SaveUploadChargeBill = function (Ids, ComDeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/SaveAlipayChargeBillUpload?Ids=" + Ids + "&ComDeptId=" + ComDeptId,
                }).success(function (data, status, headers, config) {
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.isRefresh = true
                        ckFramework.AlipayChargeBillViewTable.draw();
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }

                });
            }, 200);
        };

        AlipayPropertyService.AlipayChargeBillBatchDelete = function (Ids, ComDeptId, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: "/PropertyMgr/AlipayProperty/DeleteAlipayChargeBill?Ids=" + Ids + "&ComDeptId=" + ComDeptId,
                }).success(function (data, status, headers, config) {
                    if (data.IsSuccess) {
                        ckFramework.ModalHelper.isRefresh = true;
                        ckFramework.AlipayChargeBillTable.draw();
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                    else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }

                });
            }, 200);
        };




        return AlipayPropertyService;
    };

    ckFramework.AlipayPropertyService.$inject = injectParams;
    app.register.factory('AlipayPropertyService', ckFramework.AlipayPropertyService);
});
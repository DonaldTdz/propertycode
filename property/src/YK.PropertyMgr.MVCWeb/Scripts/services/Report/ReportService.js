'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.ReportService = function ($http, $q, $compile, $rootScope) {
        var ReportService = {};

        ReportService.GetArrearsReportDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/Report/GetArrearsReportList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        ReportService.GetArrearsReportDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/Report/GetArrearsReportList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                   

                    callback(data);
                });
            }, 200);
        }

        ReportService.GetDayReportDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/Report/GetDayReportList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }

        ReportService.GetMonthReportDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/Report/GetMonthReportList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }


        ReportService.GetPrePaymentDetailChargeSubjectList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/Report/GetReportComChargeSubjectList?ComDeptId=' + searchInfo,
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }


        ReportService.ShowContainerContent = function (divPageId, loadUrl, controllerUrl, controllerContext) {
            require(controllerUrl, function (app) {

                $("#" + divPageId).load(loadUrl, function () {
                    $compile($('#' + controllerContext))($rootScope);
                    $rootScope.$apply();
                });
            });
        }


        ReportService.GetMeterReportDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/Report/GetMeterReportList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }


        ReportService.GetIntegratedReportByChargeSubjectDataList = function (searchInfo, callback) {
            setTimeout(function () {
                $http({
                    method: 'POST',
                    url: '/PropertyMgr/Report/GetIntegratedReportByChargeSubjectList',
                    data: {
                        search: searchInfo
                    }
                }).success(function (data) {
                    callback(data);
                });
            }, 200);
        }





        return ReportService;
    
    };








    ckFramework.ReportService.$inject = injectParams;
    app.register.factory('ReportService', ckFramework.ReportService);
});
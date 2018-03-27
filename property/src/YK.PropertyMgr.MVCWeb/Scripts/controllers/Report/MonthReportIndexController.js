'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReportService'];
    ckFramework.MonthReportIndexController = function ($http, $scope, $rootScope, ReportService) {

    };

    ckFramework.MonthReportIndexController.$inject = injectParams;
    app.register.controller('MonthReportIndexController', ckFramework.MonthReportIndexController);
});
'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', 'ReportService'];
    ckFramework.DayReportIndexController = function ($http, $scope, $rootScope, ReportService) {
        
    };

    ckFramework.DayReportIndexController.$inject = injectParams;
    app.register.controller('DayReportIndexController', ckFramework.DayReportIndexController);
});
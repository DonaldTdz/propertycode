'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ReceiptBook/ReceiptBookService.js'], function (app) {

    var injectParams = ['$scope', 'ReceiptBookService'];

    ckFramework.ReciptBookHistoryListController = function ($scope, ReceiptBookService) {
        $scope.IsShowSearch = true;
    }

    ckFramework.ReciptBookHistoryListController.$inject = injectParams;

    app.register.controller('ReciptBookHistoryListController', ckFramework.ReciptBookHistoryListController);
});
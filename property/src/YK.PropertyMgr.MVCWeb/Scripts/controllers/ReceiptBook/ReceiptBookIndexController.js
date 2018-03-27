'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ReceiptBook/ReceiptBookService.js'], function (app) {

    var injectParams = ['$scope', 'ReceiptBookService'];

    ckFramework.ReceiptBookIndexController = function ($scope, ReceiptBookService) {
      
   
    }

    ckFramework.ReceiptBookIndexController.$inject = injectParams;

    app.register.controller('ReceiptBookIndexController', ckFramework.ReceiptBookIndexController);
});
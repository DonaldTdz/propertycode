'use strict';
define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/AlipayProperty/AlipayPropertyService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'AlipayPropertyService'];
    ckFramework.AlipayRoomViewController = function ($http, $window, $scope, $rootScope, AlipayPropertyService) {

        $scope.SaveData = function ()
        {
            var nodes = $("#treeAlipayRoom").jstree("get_checked");
            var formdata = {};
            formdata.ComDeptId = ckFramework.AlipayRoomViewData.ComDeptId;
            formdata.TreeNodeList = nodes;


            AlipayPropertyService.SaveAlipayRoomUpload(formdata, function (data) {

                if (data.IsSuccess) {
                    ckFramework.ModalHelper.CloseOpenModal1();
                }
                ckFramework.ModalHelper.Alert(data.DataInfo);
            });

        }

        

    };

    ckFramework.AlipayRoomViewController.$inject = injectParams;
    app.register.controller('AlipayRoomViewController', ckFramework.AlipayRoomViewController);
});

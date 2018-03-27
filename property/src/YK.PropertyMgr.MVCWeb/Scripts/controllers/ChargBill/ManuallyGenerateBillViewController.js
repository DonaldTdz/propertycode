'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/ChargBill/DailyChargListService.js'], function (app) {
    var injectParams = ['$http', '$window', '$scope', '$rootScope', 'DailyChargListService'];
    ckFramework.ManuallyGenerateBillViewController = function ($http, $window, $scope, $rootScope, DailyChargListService) { 
        $scope.ManuallyGenerateBill = function () {

            var nodes = $("#ManuallyTree").jstree().get_checked(); /*选中的节点*/
            var EndDate = $("#divManuallyGenerateBillViewController input[name='Enddate']").val();
            //alert(nodes)
            if (nodes == "" || nodes == null) {
                ckFramework.ModalHelper.Alert("请选择收费项目");
                return;
            }
            if (EndDate == null || EndDate == "") {
                ckFramework.ModalHelper.Alert("请填写结束时间");
                return;
            }
            ckFramework.ModalHelper.OpenWait();
            DailyChargListService.ManuallyGenerateBill($('#SelectDeptId').val(), $('#SelectDeptType').val(), nodes, EndDate, function (data) {
                ckFramework.ModalHelper.CloseWait();
                if (data.IsSuccess) {
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.ManuallyGenerateBillData = data;
                    ckFramework.ModalHelper.CloseOpenModal1();
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
                else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                }
            });
        };
    };

    ckFramework.ManuallyGenerateBillViewController.$inject = injectParams;
    app.register.controller('ManuallyGenerateBillViewController', ckFramework.ManuallyGenerateBillViewController);
});

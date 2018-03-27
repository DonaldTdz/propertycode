'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/BindSubjectBySingleRes/BindSubjectBySingleResService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'BindSubjectBySingleResService'];
    ckFramework.BindSubjectBySingleResController = function ($compile, $rootScope, $scope, BindSubjectBySingleResService) {
        $scope.BindSubjectBySingleResListData = ckFramework.BindSubjectBySingleResListData;
        $scope.ChargeSubjects = ckFramework.BindSubjectBySingleResListData.ChargesubjectList;

        $scope.CllearTime = function (id) {
            $("#" + id).val("");
        }

        $scope.ChargeSubjectsIndex = function (id, billPeriod) {
            /*每月计费按月收费、按读数*/
            if (billPeriod == 2 || billPeriod == 4) {
                $("#setTimeBegin" + id).datetimepicker({
                    format: 'yyyy-mm',
                    minView: 3,
                    startView: 3,
                    autoclose: true
                });
                $("#setTimeEnd" + id).datetimepicker({
                    format: 'yyyy-mm',
                    minView: 3,
                    startView: 3,
                    autoclose: true
                });
            }
            /*每日计费按月收费*/
            if (billPeriod == 1) {
                $("#setTimeBegin" + id).datetimepicker({
                    format: 'yyyy-mm-dd',
                    minView: 2,
                    startView: 2,
                    autoclose: true
                });
                $("#setTimeEnd" + id).datetimepicker({
                    format: 'yyyy-mm-dd',
                    minView: 2,
                    startView: 2,
                    autoclose: true
                });
            }

            if (billPeriod == 2 || billPeriod == 4 || billPeriod == 1) {
                $("#setCreateTime" + id).datetimepicker({
                    format: 'yyyy-mm-dd',
                    minView: 2,
                    startView: 2,
                    autoclose: true
                });
            }

        }
        //$('.#setCreateTime').datetimepicker({
        //    format: 'yyyy-mm-dd',
        //    minView: 2,
        //    startView: 2,
        //    autoclose: true
        //    }).on('changeDate', function(ev){

        //        if (ev.date.valueOf() != undefined || ev.date.valueOf() != "") {
        //            var sdate = ev.date;
        //            //alert(sdate);
        //        }
        //var type;
        //$scope.GetEasyDailyChargList = function (id) {
        //    $.post("PropertyMgr/BindSubjectBySingleRes/CheckBillBeginDate", { HouseDeptId: ckFramework.ResourcBindHouseDeptId, ChargeSubjecId: id, IsBill: ckFramework.IsBill, type: (type == null ? 0 : type), resType: ckFramework.resType, CarHouseDeptId: ckFramework.CarHouseDeptId, TextTime: $("#txtCreateTime" + id).val() }, function (data) {

        //        if (data.ErrorCode = "701") {
        //            $("#setCreateTime" + id).datetimepicker('hide');
        //            ckFramework.ModalHelper.Alert(data.Msg);
        //        }
        //        if (data.ErrorCode = "702") {
        //            var time = data.Data.EndDateFormat;
        //            $("#txtCreateTime" + id).val(data.Data.EndDateFormat);
        //            $("#setCreateTime" + id).datetimepicker('setStartDate', time);
        //        } else {
        //            $("#setCreateTime" + id).datetimepicker('setDate', data.Data.EndDateFormat);
        //        }


        //        if (data.ErrorCode = "700") {
        //            type = id;
        //            var time = data.Data.EndDateFormat;
        //            $("#txtCreateTime" + id).val(data.Data.EndDateFormat);
        //            $("#setCreateTime" + id).datetimepicker('setStartDate', time);
        //            $("#setCreateTime" + id).datetimepicker('hide');
        //            ckFramework.ModalHelper.Alert(data.Msg);
        //            ckFramework.IsBill = 0;
        //        }


        //    })
        //}

        $scope.Save = function () {
            var isPostBillSave = true;
            BindSubjectBySingleResService.SaveSubjectBySingleRes(isPostBillSave);
        }

    }

    ckFramework.BindSubjectBySingleResController.$inject = injectParams;

    app.register.controller('BindSubjectBySingleResController', ckFramework.BindSubjectBySingleResController);
});
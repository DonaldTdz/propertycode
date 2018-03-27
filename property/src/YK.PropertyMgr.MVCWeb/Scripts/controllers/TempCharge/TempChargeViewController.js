'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/TempCharge/TempChargListService.js'], function (app) {
    var injectParams = ['$compile', '$rootScope', '$scope', '$http', 'TempChargeListService'];
    ckFramework.TempChargeViewController = function ($compile, $rootScope, $scope, $http, TempChargeListService) {
        $scope.TempChargeViewData = ckFramework.TempChargeViewData;
        $scope.PageViewModel = ckFramework.TempChargeViewData.PageViewModel;
        $scope.SubjectList = ckFramework.TempChargeViewData.SubjectList;
       
        $scope.Save = function () {
            var selectedId = $('#deptTree').jstree().get_selected() + "";
            var buildId = $("#deptTree").jstree().get_parent(selectedId) + "";
            var villageId = $("#deptTree").jstree().get_parent(buildId) + "";
            if (selectedId.split('_')[1] != 20 && selectedId.split('_')[1]!=21) {
                ckFramework.ModalHelper.Alert("请选择房屋或者车位信息!");
                return false;
            }
            var msg = $scope.CheckValue()
            if (msg.length == 0) {
                $scope.PageViewModel.BeginDate = $("input[name='BeginDate']").val();
                $scope.PageViewModel.EndDate = $("input[name='EndDate']").val();
                $scope.PageViewModel.HouseDeptId = selectedId.split('_')[0]
                $scope.PageViewModel.ComVillageDeptId = villageId.split('_')[0]
                TempChargeListService.Save($scope.PageViewModel, function (data) {
                    ckFramework.ModalHelper.isRefresh = true;
                    ckFramework.ModalHelper.TempBillData = data.Data;
                    ckFramework.ModalHelper.CloseOpenModal1();
                });
            } else {
                ckFramework.ModalHelper.Alert(msg);
                return;
            }
        }
        $scope.LoadPric = function () {
            //if ($scope.PageViewModel.SubjectId > 0) {
            //    $.each(ckFramework.TempChargeViewData.SubjectList, function (i) {
            //        if (ckFramework.TempChargeViewData.SubjectList[i].Id == $scope.PageViewModel.SubjectId) {
            //            $scope.PageViewModel.Money = ckFramework.TempChargeViewData.SubjectList[i].Price;
            //        } 
            //    });
            //} else {
            //    $scope.PageViewModel.Money = 0;
            if ($scope.PageViewModel.SubjectId > 0) {
                var selectedId = $('#SelectDeptId').val();
                var postData = new Object();
                
                postData.ChargeSubjectId = $scope.PageViewModel.SubjectId;
                postData.HouseDeptId = selectedId;
                postData.RefType =  $scope.PageViewModel.RefType;
                postData.ResourcesId =selectedId;
                $http({     
                    method: 'POST',
                    data: postData,
                    url: '/PropertyMgr/TemporaryCharge/ComputeChargeSubjectAmount',
                }).success(function (data, status, headers, config) {
                    if (data.IsSuccess) {

                        $scope.PageViewModel.Money = data.Data;
                    }
                    else {

                        ckFramework.ModalHelper.Alert(data.Msg);

                    }
                });
            }
            else {
                $scope.PageViewModel.Money = 0;
            }




            
        }
        $scope.CheckValue = function () {
            var msg = "";
            if (!($("#drpSubject option:selected").val().length > 0)) {

                msg = "请选择科目、若没有请添加一次性计费科目";;
                return msg;
            }
            var beginDate = $("input[name='BeginDate']").val();
            var endDate = $("input[name='EndDate']").val();
            //if (beginDate.length == 0 || beginDate == "" || beginDate == null) {
            //    msg = "初始时间不能为空";
            //    return msg;
            //}
            //if (endDate.length == 0 || endDate == "" || endDate == null) {
            //    msg = "结束时间不能为空";
            //    return msg;
            //}
            if (beginDate != null && beginDate != "" && beginDate.length != 0) {
                if (endDate == null || endDate.length == 0 || endDate == "" ) {
                    msg = "请填写结束时间";
                    return msg;
                }
                var beginCompare = new Date(beginDate)
                var endCompare = new Date(endDate)
                if (endCompare < beginCompare) {
                    msg = "结束时间不能小于初始时间";
                    return msg;
                }
            }
            if (!($("input[name='Money']").val() > 0)) {
                msg = "请输入大于0的金额";
                return msg;
            }
            return msg;
        }
        //限制金额输入：保留小数点后两位
        $scope.toDecimal = function (event) {
            var txt = $(event.target);
            var val = ckFramework.toDecimal(txt.val());
            if (val)
                txt.val(val);
            else
                txt.val('');
        }
        $scope.isNumber = function (event) {
            var txt = $(event.target);
            if (isNaN(txt.val()))
                txt.val('');
        }
    };

    ckFramework.TempChargeViewController.$inject = injectParams;

    app.register.controller('TempChargeViewController', ckFramework.TempChargeViewController);
});
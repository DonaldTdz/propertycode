'use strict';

define(['apps/HomeApp'], function (app) {
    var injectParams = ['$http', '$q', '$compile', '$rootScope'];
    ckFramework.BindSubjectBySingleResService = function ($http, $q, $compile, $rootScope) {
        var BindSubjectBySingleResService = {};

        /*弹出框*/
        BindSubjectBySingleResService.ShowSubjectBillView = function (data) {
            //alert(11)
            //alert(JSON.stringify(data))
            var $modal = $('#divModal');
            ckFramework.ModalHelper.OpenModal1 = $modal;
            ckFramework.ModalHelper.OpenWait();
            ckFramework.ModalHelper.BillViewData = data;
            $modal.load("PropertyMgr/SubjectBill/SubjectBillIndex", {}, function () {
                require(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/controllers/SubjectBill/SubjectBillListController.js'], function (app) {
                    $modal.modal({
                        backdrop: 'static',
                        scrollY: true,
                        keyboard: false,
                        width: 800,
                        maxHeight: 600
                    });
                    ckFramework.ModalHelper.isRefresh = false;

                    $modal.off("hide.bs.modal").on("hide.bs.modal", function () {
                        if (ckFramework.ModalHelper.isRefresh) {
                            ckFramework.ChargeSubjectTable.draw();
                        }
                    });
                    $compile($('#divSubjectBillController'))($rootScope);
                    $rootScope.$apply();
                });
            });
        }

        /*按单个资源绑定收费项目*/
        BindSubjectBySingleResService.SaveSubjectBySingleRes = function (bindSubjecctBill) {
            ckFramework.ModalHelper.BindType = 1;
            var selDept = $("#SelectDeptTypeAndId").val();
            selDept = selDept.split("_");
            if (selDept.length == 2 || selDept.length == 3) {
                if (selDept[1] != 11)/*选中小区*/ {
                    ckFramework.ModalHelper.Alert("请选择小区!");
                    return false;
                }
            } else {
                ckFramework.ModalHelper.Alert("请选择小区!");
                return false;
            }

            var model;
            var arr = new Array();
            var postModel = new Object();
            var sourceType = $("#subjectHouseRefTab li[class='active']").attr("litype");
            var chkNode = $("#table_subject :checkbox[name='chkSetTime']");
            var checkedNode = $("#table_subject :checkbox[name='chkSetTime']:checked");
            var selectNode = $("#treeBindSubjectBySingleRes").jstree().get_selected();
            if (selectNode.length == 0) {
                ckFramework.ModalHelper.Alert("请选择资源");
                return false;
            }
            var idAndType = selectNode[0].split('_');
            var resId = 0;
            var houseId = 0;
            var subjectType = 0;
            /*选定的资源为三表【水、电、气】*/
            if (sourceType == 1 || sourceType == 2 || sourceType == 3) {
                if (idAndType[1] != 1000 && idAndType.length != 3) {
                    ckFramework.ModalHelper.Alert("您选择的房屋没有三表");
                    return false;
                }
                if (checkedNode.length >= 2) {
                    ckFramework.ModalHelper.Alert("三表资源 只能绑定一个三表收费项目");
                    ckFramework.SetSubjectResultData();
                    return false;
                }
                resId = idAndType[2];
                houseId = idAndType[0];
                subjectType = SubjectTypeEnum.Meter;
            } else if (sourceType == 13) {/*车库*/
                if (idAndType[1].toString().length != 36) {
                    ckFramework.ModalHelper.Alert("请选择的房屋没有车位");
                    return false;
                }
                resId = idAndType[0];
                houseId = idAndType[2];
                subjectType = SubjectTypeEnum.ParkingSpace;
            } else if (sourceType == 12) {/*房屋*/
                if (idAndType[1] != 20) {
                    ckFramework.ModalHelper.Alert("请选择房屋");
                    return false;
                }
                resId = idAndType[0]
                houseId = idAndType[0];
                subjectType = SubjectTypeEnum.House;
            }
            /*选定的具体资源*/
            var isSuccess = true;
            chkNode.each(function () {
                model = new Object();
                var id = $(this).attr("id");
                var txtBeginDate = $("#txtBeginDate" + id).val();
                var txtEndDate = $("#txtEndDate" + id).val();
                var txtCreateTime = $("#txtCreateTime" + id).val();
                var msg = "请为科目'";
                var setTimeMsg = " '设置代缴时间";
                var setTimeMsgT = " '设置计费日期";
                if ($(this).is(":checked")) {
                    if ((typeof (txtBeginDate) == "undefined" || txtBeginDate == null || txtBeginDate == "") && txtEndDate.length > 0) {
                        ckFramework.ModalHelper.Alert(msg + $(this).attr("subjectName") + setTimeMsg);
                        isSuccess = false;
                        return false;
                    }
                    if ((typeof (txtEndDate) == "undefined" || txtEndDate == null || txtEndDate == "") && txtBeginDate.length > 0) {
                        ckFramework.ModalHelper.Alert(msg + $(this).attr("subjectName") + setTimeMsg);
                        isSuccess = false;
                        return false;
                    }
                    if ((typeof (txtCreateTime) == "undefined" || txtCreateTime == null || txtCreateTime == "") && txtCreateTime.length > 0) {
                        ckFramework.ModalHelper.Alert(msg + $(this).attr("subjectName") + setTimeMsgT);
                        isSuccess = false;
                        return false;
                    }
                    if (txtEndDate.split('-').length == 2) {
                        var timeArr = txtBeginDate.split('-');
                        txtBeginDate = timeArr[0] + "-" + timeArr[1];
                        if (txtBeginDate > txtEndDate) {
                            ckFramework.ModalHelper.Alert("开始时间必须小于结束时间");
                            isSuccess = false;
                            return false;
                        }
                    } else {
                        if (txtBeginDate > txtEndDate) {
                            ckFramework.ModalHelper.Alert("开始时间必须小于结束时间");
                            isSuccess = false;
                            return false;
                        }
                    }
                    model.BindSubject = true;
                    model.BeginDate = txtBeginDate;
                    model.EndDate = txtEndDate;
                    model.BeginDateBill = txtCreateTime;
                   
                } else {
                    model.BindSubject = false;
                }
                model.SubjectId = id;
                arr.push(model);
            })
            if (isSuccess == true) {
                postModel.ResId = resId;
                postModel.DeveloperSetTimelist = arr;
                postModel.HouseDeptId = houseId;
                postModel.SubjectType = subjectType;                                                        /*保存是检验账单*/
                $.post("PropertyMgr/BindSubjectBySingleRes/Save", { jsonString: JSON.stringify(postModel), isPostBillSave: bindSubjecctBill, sourceType: sourceType }, function (data) {
                    if (data.IsSuccess) {
                        if (data.ErrorCode == "100") {
                            //alert(JSON.stringify(data))
                            /*生成账单的时候*/
                            BindSubjectBySingleResService.ShowSubjectBillView(data.Data);
                        } else {
                            /*保存解绑或绑定的的时候*/
                            ckFramework.ModalHelper.CloseOpenModal1();
                            ckFramework.ModalHelper.Alert(data.Msg);
                        }
                    } else {
                        ckFramework.ModalHelper.Alert(data.Msg);
                    }
                })
            }
        }

        /*按单个科目绑定资源*/
        BindSubjectBySingleResService.SaveSubjectBySingleSubject = function (bindSubjecctBill) {
            ckFramework.ModalHelper.BindType = 2;
            $("#btnResSave").attr("disabled", "disabled");
            var bindAllHouseByBuild = [];/*绑定或解绑整栋楼的、没有子节点切被选中*/
            var unBindAllHouseByBuild = "";/*解绑整栋楼的、没有子节点切被选中*/
            var checkNodes = $("#deptResTypeTree li[aria-level='1']");/*确定全选的资源*/
            var checkNodesUnBind = $("#deptResTypeTree li[aria-level='2'][aria-selected='false']");/*确定要解绑的资源*/
            $.each(checkNodesUnBind, function (i) {
                unBindAllHouseByBuild = unBindAllHouseByBuild + $(this).attr("id") + ",";
            });
            var model;
            $.each(checkNodes, function (i) {
                model = new Object();
                if (!($(this).find("ul").length > 0) && $(this).attr("aria-selected") == "true" && !$(this).hasClass("jstree-leaf")) {
                    model.IsBind = true;
                    model.State = 1;
                    model.ResId = $(this).attr("Id");
                    bindAllHouseByBuild.push(model)
                }
                if (!($(this).find("ul").length > 0) && $(this).attr("aria-selected") == "false" && !$(this).hasClass("jstree-leaf")) {
                    model.IsBind = false;
                    model.State = 2;
                    model.ResId = $(this).attr("Id");
                    bindAllHouseByBuild.push(model)
                }
                /*半选状态*/
                if (!($(this).find("ul").length > 0) && $(this).attr("aria-selected") == "false" && $(this).find("a:first").find("i:first").hasClass("jstree-undetermined") && !$(this).hasClass("jstree-leaf")) {
                    model.ResId = $(this).attr("Id");
                    model.State = 3;
                    model.IsBind = true;
                    bindAllHouseByBuild.push(model)
                }
            })
            BindSubjectBySingleResService.PostSingleSubject(bindAllHouseByBuild, unBindAllHouseByBuild, $("#hidSubjectId").val(), $("#hidResType").val(), bindSubjecctBill);
        }

        /*绑定_解绑_开发商代缴*/
        BindSubjectBySingleResService.PostSingleSubject = function (bindBatchOrNot, unBindAllHouseByBuild, subjectId, resType, bindSubjecctBill) {
            var ids = GetCheckedNodes();/*选择框勾选开始点中的元素*/
            //alert(ids)
            if (!(subjectId > 0)) {
                $("#btnResSave").removeAttr("disabled"); ckFramework.ModalHelper.Alert("请选择绑定的科目,若没有科目请添加"); return false;
            }
            ckFramework.ModalHelper.OpenWait();
            $.post("PropertyMgr/ResType/SaveData", { 'ids': ids, 'subjectId': subjectId, resType: resType, bindBatchOrNot: JSON.stringify(bindBatchOrNot), unBindAllHouseByBuild: unBindAllHouseByBuild, isPostBillSave: bindSubjecctBill }, function (data) {
                if (data.IsSuccess) {
                    if (data.ErrorCode == "100") {
                        //alert(JSON.stringify(data))
                        /*生成账单的时候*/
                        BindSubjectBySingleResService.ShowSubjectBillView(data.Data)
                    } else {
                        ckFramework.ModalHelper.CloseOpenModal1();
                        ckFramework.ModalHelper.Alert(data.Msg);
                        $("#subjectHouseRefTab li[class='active'] a").click();
                    }
                } else {
                    ckFramework.ModalHelper.Alert(data.Msg);
                    $("#subjectHouseRefTab li[class='active'] a").click();
                }
                ckFramework.ModalHelper.CloseWait();
                $("#btnResSave").removeAttr("disabled");
            })
            $("#btnResSave").removeAttr("disabled");
        }


        return BindSubjectBySingleResService;
    };

    ckFramework.BindSubjectBySingleResService.$inject = injectParams;
    app.register.factory('BindSubjectBySingleResService', ckFramework.BindSubjectBySingleResService);
});
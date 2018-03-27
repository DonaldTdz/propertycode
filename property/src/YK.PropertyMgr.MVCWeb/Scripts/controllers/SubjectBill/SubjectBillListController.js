'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/BindSubjectBySingleRes/BindSubjectBySingleResService.js'],
    function (app) {
        var injectParams = ['$compile', '$rootScope', '$scope', '$http', 'BindSubjectBySingleResService'];
        ckFramework.SubjectBillController = function ($compile, $rootScope, $scope, $http, BindSubjectBySingleResService) {
            $scope.IsCheckedAll = false;
            $scope.ChkALLNum = 0;
            //$scope.PageViewSubjectBills = ckFramework.ModalHelper.BillViewData;//ckFramework.PageViewData.PageViewSubjectBills;
            $scope.PageViewSubjectBillsSearch = [];
            $scope.PageViewSubjectBills = [];
            var jsonData = [];
            $scope.ResourceName = "";
            $scope.DeleteResourceSubject = function (subjectId, resourceId) {
                for (var i = 0; i < $scope.PageViewSubjectBills.length; i++) {
                    if ($scope.PageViewSubjectBills[i].SubjectId == subjectId && $scope.PageViewSubjectBills[i].ResourceId == resourceId) {
                        $scope.PageViewSubjectBills.splice(i, 1);
                    }
                }
                //alert(JSON.stringify(jsonData));
            }
            //alert(JSON.stringify(ckFramework.ModalHelper.BillViewData))
           
            if (ckFramework.ModalHelper.BillViewData != undefined
                && ckFramework.ModalHelper.BillViewData != null
                && ckFramework.ModalHelper.BillViewData.length > 0) {
                jsonData = ckFramework.ModalHelper.BillViewData;
            }
            $scope.PageViewSubjectBills = jsonData;
            //alert(JSON.stringify(jsonData))

            /*03.全选*/
            $scope.ChkAll = function () {
                //if ($scope.IsCheckedAll == false) {
                //    $scope.ChkALLNum = 0;
                //}
                //$scope.IsCheckedAll = !$scope.IsCheckedAll;
                //$scope.PageViewSubjectBills.forEach(function (item) {
                //    item.IsChecked = $scope.IsCheckedAll;
                //});

                $("#simpleTable").find("input[type='checkbox']").prop("checked", $scope.IsCheckedAll);
            }

            //datatable 配置
            $scope.simpleTableOptions = {
                //sAjaxSource: 'Scripts/widgets/datatable/data.json',
                drawCallback: function (backSettings) {
                    //alert(8080)
                    $scope.IsCheckedAll = false;
                    $("#divSimpleTable").find("input[name='chkAll']").prop("checked", $scope.IsCheckedAll);
                    $scope.ChkAll();
                    //$compile($("#divSimpleTable"))($scope);
                    //$('#simpleTable_filter').append("<div class='btn-group' style='font-size:12px; position:absolute;right:70px;top:0px;'><a class='btn btn-primary' href='#'><i class='fa fa-plus'></i> 添加</a></div>");
                },
                aoColumns: [
                     {
                         data: null,
                         "mRender": function (data) {
                             var action = "<input type='checkbox' />"
                             + "<input type='hidden' value='" + data.SubjectId + "," + data.HouseDeptId + "," + data.ResourceId + "' />";
                             return action;
                         },
                         "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                             $compile(nTd)($scope);
                         }
                     },
                    { data: 'ResourceName' },
                    { data: 'SubjctName' },
                    { data: 'BillEndTimeFormat' },
                    {
                        data: null,
                        "mRender": function (data) {
                            var action = "<input type='button' value='生成' class='btn btn-primary btn-small' ng-click='GenerationBillUnbundling("+data.SubjectId+","+data.ResourceId+","+data.HouseDeptId+")' />";
                            return action;
                        },
                        "fnCreatedCell": function (nTd, sData, oData, iRow, iCol) {
                            $compile(nTd)($scope);
                        }
                    }
                ],
                "aaData": jsonData,
                "sDom": "Tflt<'row DTTTFooter'<'col-sm-6'i><'col-sm-6'p>>",
                "iDisplayLength": 100,
                "aLengthMenu" : [50, 100],
                "sScrollY": 300,
                "sScrollX": 778,
                "bAutoWidth": false,//是否自适应宽度 
                "bSort": false,
                //"bScrollCollapse": true, //是否开启DataTables的高度自适应，当数据条数不够分页数据条数的时候，插件高度是否随数据条数而改变  
                //"bLengthChange": false,
                "bFilter": true,
                "table-layout": "fixed",
                "serverSide" : false,
                "oTableTools": {
                    "aButtons": [
                        //"copy", "csv", "xls", "pdf", "print"
                    ]//,
                    //"sSwfPath": "assets/swf/copy_csv_xls_pdf.swf"
                },
                "language": {
                    "search": "",
                    "sLengthMenu": "_MENU_",
                    "sLengthMenu": "显示 _MENU_",
                    "sProcessing": "正在获取数据，请稍后...",
                    "sZeroRecords": "没有您要搜索的内容",
                    "sInfo": "从 _START_ 到  _END_ 条记录 共 _TOTAL_ 条",
                    "sInfoEmpty": "记录数为0",
                    "sInfoFiltered": "(共显示 _MAX_ 条数据)",
                    "sInfoPostFix": "",
                    "oPaginate": {
                        "sPrevious": "上一页",
                        "sNext": "下一页"
                    }
                },
                "aaSorting": []
            };
            //$("#simpleTable").dataTable().fnDraw(false);
            $scope.RedrawTable = function () {
                //alert(jsonData.length)
                $("#simpleTable").dataTable().api().destroy();
                $scope.simpleTableOptions.aaData = jsonData;
                $("#simpleTable").dataTable($scope.simpleTableOptions);
                $("#simpleTable").find("input[type='checkbox']").prop("checked", $scope.IsCheckedAll);
            }
            /*01.账单数据提交*/
            $scope.PostBill = function (subjectIds, arr, subjectId, resourceId) {
                ckFramework.ModalHelper.OpenWait();
                setTimeout(function () {
                    $http({
                        method: 'POST',
                        url: '/PropertyMgr/SubjectBill/GenerationBillUnbundling',
                        data: {
                            subjectIdJson: subjectIds,
                            unbundlingDtoJson: JSON.stringify(arr)
                        }
                    }).success(function (data) {
                        //alert(JSON.stringify(data))
                        var errorCount = 0;
                        data.forEach(function (item) {
                            //alert(JSON.stringify(item.Data.ResultIds))
                            if (item.IsSuccess) {
                                for (var i = 0; i < item.Data.ResultIds.length; i++) {
                                    $scope.DeleteResourceSubject(item.Data.SubjectId, item.Data.ResultIds[i]);
                                }
                                $scope.ChkALLNum = 0;
                                $scope.IsCheckedAll = false;
                                $scope.PageViewSubjectBills.forEach(function (item) {
                                    item.IsChecked = false;
                                });
                            }
                            else {
                                errorCount++;
                            }
                        });
                        if (errorCount == 0) {
                            ckFramework.ModalHelper.Alert("生成账单成功!");
                            $scope.RedrawTable();
                        } else if (errorCount == data.length) {
                            ckFramework.ModalHelper.Alert("生成账单失败!");
                        } else {
                            ckFramework.ModalHelper.Alert("生成账单部分成功，详细请查看页面结果!");
                            $scope.RedrawTable();
                        }
                        ckFramework.ModalHelper.CloseWait();
                    });
                }, 200);

            }
            /*02.生成账单*/
            $scope.GenerationBillUnbundling = function (subjectId, resourceId, houseDeptId) {
                var arr = new Array();
                var subjectArr = new Array();
                /*单个账单*/
                if (subjectId > 0 && resourceId > 0) {
                    var model = new Object();
                    model.HouseDeptId = houseDeptId;
                    model.ResultId = resourceId;
                    arr.push(model);
                    subjectArr.push(subjectId);
                    $scope.PostBill(JSON.stringify(subjectArr), arr, subjectId, resourceId);
                } else {
                    /*批量账单*/
                    //$scope.PageViewSubjectBills.forEach(function (item) {
                    //    if (item.IsChecked) {
                    //        if (!(subjectId > 0)) {
                    //            subjectArr.push(item.SubjectId);
                    //        }
                    //        var model = new Object();
                    //        model.HouseDeptId = item.HouseDeptId;
                    //        model.ResultId = item.ResourceId;
                    //        arr.push(model);

                    //    }
                    //});
                    $("#simpleTable").find("input[type='checkbox']:checked").each(function () {
                       // if ($(this).prop("checked")) {
                        var dataItem = $(this).parent().find("input[type='hidden']").val();
                        if (dataItem != undefined && dataItem != null) {
                            var iarr = dataItem.split(',');
                            // alert(iarr)
                            if (!(subjectId > 0)) {
                                subjectArr.push(iarr[0]);
                            }
                            var model = new Object();
                            model.HouseDeptId = iarr[1];
                            model.ResultId = iarr[2];
                            arr.push(model);
                        }
                          
                       // }
                    });
                    if (arr.length == 0) {
                        ckFramework.ModalHelper.Alert("请选择要批量生成账单的资源!");
                        return;
                    } else {
                        $scope.PostBill(JSON.stringify(subjectArr), arr, null, null);
                    }
                }
            }
           
            //$scope.Change = function () {
            //    alert($("#bodyContent td[name='res']").html())
            //    $scope.IsCheckedAll = false;
            //    $scope.PageViewSubjectBills.forEach(function (item) {
            //        item.IsChecked = false;
            //    });
            //}
            /*04.单选*/
            $scope.SingleChecked = function (resourceId, subjectId) {
                //$scope.PageViewSubjectBills.forEach(function (item) {
                //    if (item.ResourceId == resourceId && item.SubjectId == subjectId) {
                //        item.IsChecked = !item.IsChecked;
                //        if (item.IsChecked) {
                //            $scope.ChkALLNum = $scope.ChkALLNum + 1;
                //        } else {
                //            if ($scope.ChkALLNum > 0) {
                //                $scope.ChkALLNum = $scope.ChkALLNum - 1;
                //            }
                //        }
                //    }
                //});
                //if ($scope.ChkALLNum == $scope.PageViewSubjectBills.length) {
                //    $scope.IsCheckedAll = true;
                //} else {
                //    $scope.IsCheckedAll = false;
                //}
            }
            /*05.保存父页面的绑定数据或解绑数据*/
            $scope.BindSave = function () {
                /*根据资源绑定科目*/
                if (ckFramework.ModalHelper.BindType == 1) {
                    BindSubjectBySingleResService.SaveSubjectBySingleRes(false)
                }
                if (ckFramework.ModalHelper.BindType == 2) {
                    /*根据科目绑定资源*/
                    BindSubjectBySingleResService.SaveSubjectBySingleSubject(false);
                }
            }
        }
        ckFramework.SubjectBillController.$inject = injectParams;
        app.register.controller('SubjectBillController', ckFramework.SubjectBillController);
    });
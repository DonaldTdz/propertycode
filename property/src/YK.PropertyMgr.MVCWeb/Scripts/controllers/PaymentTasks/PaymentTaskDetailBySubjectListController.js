'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/PaymentTasks/PaymentTaskDetailBySubjectListService.js'], function (app) {

    var injectParams = ['$compile', '$rootScope', '$scope', 'PaymentTaskDetailBySubjectListService', '$http'];

    ckFramework.PaymentTaskDetailBySubjectListController = function ($compile, $rootScope, $scope, PaymentTaskDetailBySubjectListService, $http) {
        $scope.ChargeRecordListData = ckFramework.ChargeRecordListData;
        $scope.IsShowSearch = true;
        $scope.Search = {};
        $scope.PaymentTaskSubjectDataList =[];
        $scope.TotalStyle = {
            "background-color": "#ddd",
            "font-weight": "bold"
        };
        $scope.searchClick = function () {
            $("#testCustomer").dataTable().fnDraw();
        }

        $scope.testTableOptions = {
            //aoColumns: [
            //    { "data": "name" },
            //    { "data": "title" },
            //    { "data": "desc" }
            //],
            /* 
             * 向服务器传递的参数 
            */
            "fnServerParams": function (aoData) {
            },
            "sDom": "Tflt<'row DTTTFooter'<'col-sm-6'i><'col-sm-6'p>>",
            "iDisplayLength": 10,
            "aLengthMenu": [5, 10, 20, 50],
            "oTableTools": {
                //"sRowSelect": "multi",
                "aButtons": [
                ]//,
                // "sSwfPath": "assets/swf/copy_csv_xls_pdf.swf"
            },
            "paging":false,
            "bProcessing": true,
            "bServerSide": true, //是否启动服务器端数据导入 
            "bSort": false, //是否启动各个字段的排序功能  
            "language": {
                "search": "",
                "sLengthMenu": "_MENU_",
                "sProcessing": "正在获取数据，请稍后...",
                //"sLengthMenu": "显示 _MENU_ 条",
                "sZeroRecords": "没有数据",
                "sInfo": "从 _START_ 到  _END_ 条记录 共 _TOTAL_ 条",
                "sInfoEmpty": "记录数为0",
                "sInfoFiltered": "(共显示 _MAX_ 条数据)",
                "sInfoPostFix": "",
                "oPaginate": {
                    "sPrevious": "上一页",
                    "sNext": "下一页"
                }
            },
            "aaSorting": [],
            //请求url  
            "sAjaxSource": "PropertyMgr/PaymentTasks/GetPaymentTasksBySubjectList",
            drawCallback: function (backSettings) {
                var headhtml = '';
                for (var i = 0; i < $scope.headList.length; i++) {
                    headhtml += '<th>' + $scope.headList[i].name + '</th>';
                }
                //alert(headhtml);
                $("#trTestThead").empty();
                $("#trTestThead").html(headhtml);
                var bodyhtml = '';
                for (var j = 0; j < $scope.bodyData.length; j++) {
                    bodyhtml += '<tr>';
                    for (var k = 0; k < $scope.bodyData[j].length; k++) {
                        bodyhtml += '<td>' + $scope.bodyData[j][k] + '</td>';
                    }
                    bodyhtml += '</tr>';
                }
                $("#tbodyTest").empty();
                $("#tbodyTest").html(bodyhtml);
            },
            //服务器端，数据回调处理  
            "fnServerData": function (sSource, aDataSet, fnCallback, oSettings) {
                //alert(JSON.stringify(oSettings));
                var sd = {};
                for (var i = 0; i < aDataSet.length; i++) {
                    if (aDataSet[i].name == "iDisplayStart") {
                        sd.PageStart = aDataSet[i].value;
                    }
                    if (aDataSet[i].name == "iDisplayLength") {
                        sd.PageSize = aDataSet[i].value;
                    }
                    if (aDataSet[i].name == "sEcho") {
                        sd.Draw = aDataSet[i].value;
                    }
                }

                sd.PaymentTaskId = $('#PaymentTaskId').val();
                sd.PaymentDateMax = $('#PaymentDateMax').val();
                sd.DeptId = $('#SelectDeptId').val();

                $.ajax({
                    "dataType": 'json',
                    "type": "post",
                    "url": sSource,
                    "data": sd,
                    "success": function (resp) {
                        $scope.headList = resp.thead;
                        $scope.bodyData = resp.data.aaData;
                        fnCallback(resp.data);
                    }
                });
            }
        };


      
        $scope.GetPaymentTaskSubjectDataList = function () {

            $scope.Search.PaymentTaskId = $('#PaymentTaskId').val();
            $scope.Search.PaymentDateMax = $('#PaymentDateMax').val();
            $scope.Search.DeptId = $('#SelectDeptId').val();
            ckFramework.ModalHelper.OpenWait();
            PaymentTaskDetailBySubjectListService.GetPaymentTaskSubjectDataList($scope.Search, function (data) {
                $scope.PaymentTaskSubjectDataList = data;
                //ckFramework.ResetWindowLayout();
                ckFramework.ModalHelper.CloseWait();
            });
        }

        $scope.GetPaymentTaskSubjectDataList();




    }

    ckFramework.PaymentTaskDetailBySubjectListController.$inject = injectParams;

    app.register.controller('PaymentTaskDetailBySubjectListController', ckFramework.PaymentTaskDetailBySubjectListController);
});
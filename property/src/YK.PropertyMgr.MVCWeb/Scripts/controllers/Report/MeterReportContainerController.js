'use strict';

define(['apps/HomeApp', '/Plugins/PropertyMgr/Scripts/services/Report/ReportService.js'], function (app) {
    var injectParams = ['$http', '$scope', '$rootScope', '$compile', 'ReportService'];
    ckFramework.MeterReportContainerController = function ($http, $scope, $rootScope, $compile, ReportService) {
        $scope.IsShowSearch = true;
        $scope.PageModel = ckFramework.MeterReportData;
        $scope.ComDeptList = ckFramework.MeterReportData.ReportDeptinfo;
        $scope.ComDeptId = ckFramework.MeterReportData.DefaultDeptId;
        $scope.NMonth = (new Date()).getMonth() + 1;
        $scope.MeterReportDataList = [];
        $scope.Search = {};
        $scope.ComDeptId;
        $scope.TotalStyle = {
            "background-color": "#ddd",
            "font-weight": "bold"
        };

        $scope.MeterReportExportData = function () {
            var LouyuIdStr = $("#SearchContainerMeterReport input:hidden[name='LouyuIdStr']").val();
            var parameters = "?ComDeptIdStr=" + $('#ComDeptIdStr').val() + "&BeginDate=" + $('#BeginDate').val() + "&EndDate=" + $('#EndDate').val() + "&DefaultComDeptId=" + $('#DefaultComDeptId').val() + "&ResourceName=" + $('#ResourceName').val() + "&OwnerName=" + $('#OwnerName').val() + "&LouyuIdStr=" + LouyuIdStr;
            var iframe = document.createElement("iframe");
            iframe.src = "PropertyMgr/Report/MeterReportExportData" + parameters;

            iframe.style.display = "none";
            parent.document.body.appendChild(iframe);
        }
        $scope.headList = [{ name: "姓名" }, { name: "标签" }, { name: "描述" }];
        $scope.bodyData = [];

        $scope.searchClick = function () {
            $("#testCustomer").dataTable().fnDraw();
        }

        $scope.MeterTableOptions = {
            
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
            "bProcessing": true,
            "bServerSide": true, //是否启动服务器端数据导入 
            "bSort": false, //是否启动各个字段的排序功能  
            "language": {
                "search": "",
                "sLengthMenu": "显示 _MENU_ 项结果",//修改4575 2017/7/5 zzh
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
            "sAjaxSource": "PropertyMgr/Report/GetMeterReportListNew",
            drawCallback: function (backSettings) {
                //$('#tableCustomer_filter').append("<div class='btn-group' style='font-size:12px; position:absolute;right:70px;top:0px;'><a class='btn btn-primary' href='#/app/customerCreate/0'><i class='glyphicon glyphicon-plus'></i> 添加</a></div>");
                //var txtSearch = $('#divCustomer').find("input[type='search']");
                //$(txtSearch).css({ width: "350px" });
                //$(txtSearch).attr("placeholder", "姓名/客户名称、账号/客户编码、单位");
                //$(".DTTT").html("<a class='btn btn-primary' href='#/app/createnews/:0'><i class='glyphicon glyphicon-plus'></i> 添加</a>");
                //var html = '';
                //html +='显示'
                //html += '<lable>';
                //html += '<select name="testCustomer_length" aria-controls="testCustomer" class="form-control inputsm"><option value="5">5</option><option value="10">10</option><option value="20">20</option><option value="50">50</option></select>'
                //html += '</lable>';
                //html +='项结果'
                //$("div#testCustomer_length").html(html);

                var headhtml = '';
                for (var i = 0; i < $scope.headList.length; i++) {
                    headhtml += '<th>' + $scope.headList[i].name + '</th>';
                }
                //alert(headhtml);
                //alert($("div#testCustomer_length").html());
                $("#trTestThead").empty();
                $("#trTestThead").html(headhtml);
                var bodyhtml = '';
                if ($scope.bodyData.length < 2)
                {
                    bodyhtml += '<tr class="odd">';

                    bodyhtml += '<td valign="top" colspan="8" class="dataTables_empty">没有数据</td>';

                    bodyhtml += '</tr>';
                }
                else
                {
                    for (var j = 0; j < $scope.bodyData.length; j++) {
                        bodyhtml += '<tr>';
                        for (var k = 0; k < $scope.bodyData[j].length; k++) {
                            bodyhtml += '<td>' + $scope.bodyData[j][k] + '</td>';
                        }
                        bodyhtml += '</tr>';
                    }
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

                sd.ComDeptIdStr = $('#ComDeptIdStr').val();
                sd.BeginDate = $('#BeginDate').val();
                sd.EndDate = $('#EndDate').val();
                sd.DefaultComDeptId = $('#DefaultComDeptId').val();
                sd.ResourceName = $('#ResourceName').val();
                sd.OwnerName = $('#OwnerName').val();
                sd.LouyuIdStr = $("#SearchContainerMeterReport input:hidden[name='LouyuIdStr']").val();
                $.ajax({
                    "dataType": 'json',
                    "type": "post",
                    "url": sSource,
                    "data": sd,
                    "success": function (resp) {
                        //alert(JSON.stringify(resp));
                        $scope.headList = resp.thead;
                        $scope.bodyData = resp.data.aaData;
                        //$scope.testTableOptions.aoColumns = resp.columnBind;
                        //alert(JSON.stringify($scope.headList));
                        fnCallback(resp.data);
                    }
                });
            }
        };
    };

    ckFramework.MeterReportContainerController.$inject = injectParams;
    app.register.controller('MeterReportContainerController', ckFramework.MeterReportContainerController);
});
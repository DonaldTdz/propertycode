// 注册全局设置

$.ajaxSetup({ cache: false }); // 禁止Ajax缓存

$.extend($.fn.DataTable.defaults, {
    searching: false, //  是否显示jquery.datatable的默认查询
});

//验证信息中文设置
$.extend($.validator.messages, {
    required: "这是必填字段",
    remote: "请修正此字段",
    email: "请输入有效的电子邮件地址",
    url: "请输入有效的网址",
    date: "请输入有效的日期",
    dateISO: "请输入有效的日期 (YYYY-MM-DD)",
    number: "请输入有效的数字",
    digits: "只能输入数字",
    creditcard: "请输入有效的信用卡号码",
    equalTo: "你的输入不相同",
    extension: "请输入有效的后缀",
    maxlength: $.validator.format("最多可以输入 {0} 个字符"),
    minlength: $.validator.format("最少要输入 {0} 个字符"),
    rangelength: $.validator.format("请输入长度在 {0} 到 {1} 之间的字符串"),
    range: $.validator.format("请输入范围在 {0} 到 {1} 之间的数值"),
    max: $.validator.format("请输入不大于 {0} 的数值"),
    min: $.validator.format("请输入不小于 {0} 的数值")
});

$('#divModal').on('show.bs.modal', function (e) {
    setTimeout(function () {
        ckFramework.ModalHelper.CloseWait();
    }, 100);
});

var Close401Modal = function () {
    setTimeout(function () {
        ckFramework.ModalHelper.CloseWait();
        ckFramework.ModalHelper.CloseOpenModal();
        Close401Modal();
    }, 200);
}

$(document).ajaxError(function (event, jqxhr, settings, thrownError) {
    if (jqxhr.status == 401) {
        setTimeout(function () {
            ckFramework.ModalHelper.CloseWait();
            ckFramework.ModalHelper.CloseOpenModal();
            var errorMessage = ckFramework.ClientMessage.GetMessage().SesstionTimeoutError;
            ckFramework.ModalHelper.Alert(errorMessage, function () { window.location.href = ckFramework.UrlLogout; });
            Close401Modal();
        }, 200);
    }
});

(function ($) {
    jQuery.fn.dataTableWithFilter = function (settings, isInTab, isWorkflow) {
        ckFramework.ModalHelper.IsAddRow = false;
        // alias the original jQuery object passed in since there is a possibility of multiple dataTables and search containers on a single page.
        // If we don't do this then we run the risk of having the wrong jQuery object before forcing a dataTable.draw() call
        ckFramework.SearchCriterias = [];
        var $dataTable = this,
            searchCriteria = [],
            filterOptions = settings.filterOptions,
            // retrieves all inputs that we want to filter by in the searchContainer
            $searchContainerInputs = $('#' + filterOptions.searchContainer).find('input[type="text"],input[type="radio"],input[type="checkbox"],select,textarea');
        // remove the filterOptions object from the object literal (json) that will be passed to dataTables
        delete settings.filterOptions;
        if (filterOptions === undefined) {
            throw {
                name: 'filterOptionsUndefinedError',
                message: 'Please define a filterOptions property in the object literal'
            };
        }
        if (filterOptions.searchButton === undefined) {
            throw {
                name: 'searchButtonUndefinedError',
                message: 'Please define a searchButton in the filterOptions'
            };
        }
        //if (filterOptions.clearSearchButton === undefined) {
        //    throw {
        //        name: 'clearSearchButtonUndefinedError',
        //        message: 'Please define a clearSearchButton in the filterOptions'
        //    };
        //}
        if (filterOptions.searchContainer === undefined) {
            throw {
                name: 'searchContainerUndefinedError',
                message: 'Please define a searchContainer in the filterOptions'
            };
        }
        $searchContainerInputs.keypress(function (e) {
            if (e.keyCode === 13) {
                $("#" + filterOptions.searchButton).click();
            }
        });
        var SearchQuery = function () {
            var searchContainer = $("#" + filterOptions.searchContainer);
            searchContainer.find('input[type="text"][value!=""],input[type="hidden"],input[type="radio"]:checked,input[type="checkbox"]:checked,textarea[value!=""],select[value!=""]').each(function () {
                // all textboxes, radio buttons, checkboxes, textareas, and selects that actually have a value associated with them
                var element = $(this), value = element.val();
                //alert(element.attr("name"))
                //alert($(this).attr("type"))
                if (element.attr("type") == "checkbox") {
                    //alert(8888)
                    //alert(element.attr("type"))
                    ckFramework.SearchCriterias.push({
                        "name": element.attr("name"), "value": "true"
                    });
                    searchCriteria.push({ "name": element.attr("name"), "value": "true" });
                } else {
                    if (typeof value === "string") {
                        ckFramework.SearchCriterias.push({ "name": element.attr("name"), "value": value });
                        searchCriteria.push({ "name": element.attr("name"), "value": value });
                    }
                    else if (Object.prototype.toString.apply(value) === '[object Array]') {
                        // multi select since it has an array of selected values
                        var i;
                        for (i = 0; i < value.length; i++) {
                            ckFramework.SearchCriterias.push({ "name": element.attr("name"), "value": value[i] });
                            searchCriteria.push({ "name": element.attr("name"), "value": value[i] });
                        }
                    }
                    else {
                        ckFramework.SearchCriterias.push({ "name": element.attr("name"), "value": value });
                    }
                }
            });
        }
        $("#" + filterOptions.searchButton).click(function () {
            ckFramework.SearchCriterias = [];
            searchCriteria = [];
            SearchQuery();
            if (ckFramework.ClientCustomSearch) {
                for (var i = 0; i < ckFramework.ClientCustomSearch.length; i++) {
                    searchCriteria.push(ckFramework.ClientCustomSearch[i]);
                }
            };
            $dataTable.data = searchCriteria;
            // force dataTables to make a server-side request
            //$dataTable.draw();
            filterOptions.DrawTable();
        });
        if (ckFramework.ClientCustomSearch) {
            SearchQuery();
            for (var i = 0; i < ckFramework.ClientCustomSearch.length; i++) {
                searchCriteria.push(ckFramework.ClientCustomSearch[i]);
            }
        };
        settings.processing = true;
        settings.serverSide = true;
        settings.deferRender = true;
        settings.scrollCollapse = true;
        settings.drawCallback = function (backSettings) {
            if (!isInTab) {
                var $table = $('#' + backSettings.sTableId).dataTable();
                try {
                    $table.fnAdjustColumnSizing(false);
                } catch (ex) { }
                var rowLength = $('#' + backSettings.sTableId + ' tbody tr').length;
                if (rowLength == 0) {
                    rowLength = 1;
                }
                var rowHeight = rowLength * 46 + 5;

                //
                var $dataTableWrapper = $('#' + backSettings.sTableId).closest(".dataTables_wrapper");
                //var panelHeight = $dataTableWrapper.parent().height() + (ckFramework.ModalHelper.IsAddRow ? 45 : 0);

                //$dataTableWrapper.find(".fg-toolbar").each(function (i, obj) {
                //    toolbarHeights = toolbarHeights + $(obj).height();
                //});

                var toolbarHeights = $("#divToolbar").height();
                var pageHeight = $(".paging_simple_numbers").height();
                var searchHeight = $("#SearchContainer").height();
                var scrollHeadHeight = $dataTableWrapper.find(".dataTables_scrollHead").height();

                var windowsHeight = $(window).height();
                //200为上下一些控件的固定高度
                $dataTableWrapper.find(".dataTables_scrollBody").height(windowsHeight - (toolbarHeights + pageHeight + searchHeight + scrollHeadHeight + 200) + "px")

                //if (!settings.OtherHeight) {
                //    settings.OtherHeight = 400;
                //}
                //var height = 0;
                //var windowHeight = $(window).height();
                //if (rowHeight + toolbarHeights + scrollHeadHeight + settings.OtherHeight > windowHeight)
                //{
                //    height = windowHeight - scrollHeadHeight - toolbarHeights - settings.OtherHeight;
                //}
                //else
                //{
                //    height = rowHeight;
                //}

                //if (height < 0)
                //{
                //    height = 70;
                //}

                //height = panelHeight - toolbarHeights - scrollHeadHeight;
                //if (($(window).height() - settings.OtherHeight) < (height - (isWorkflow ? 68 : 70))) {
                //    height = $(window).height() - settings.OtherHeight + (isWorkflow ? 68 : 70);
                //}
                //$dataTableWrapper.find(".dataTables_scrollBody").height( );
                try {
                    $table._fnScrollDraw();
                } catch (ex) { }
            }
        }
        settings.ajax = {
            url: filterOptions.Url,
            type: 'POST',
            data: function (d) {
                return $.extend({}, d, {
                    "CustomSearch": JSON.stringify(searchCriteria)
                });
            },
            "error": function () {
            }
        };
        return $dataTable.DataTable(settings);
    };
}(jQuery));

//add by donald 2016-9-7
(function ($) {
    jQuery.fn.dataSearchTableWithFilter = function (settings, isInTab, isWorkflow) {
        ckFramework.ModalHelper.IsAddRow = false;
        // alias the original jQuery object passed in since there is a possibility of multiple dataTables and search containers on a single page.
        // If we don't do this then we run the risk of having the wrong jQuery object before forcing a dataTable.draw() call
        ckFramework.SearchCriterias = [];
        var $dataTable = this,
            searchCriteria = [],
            filterOptions = settings.filterOptions,
            // retrieves all inputs that we want to filter by in the searchContainer
            $searchContainerInputs = $('#' + filterOptions.searchContainer).find('input[type="text"],input[type="hidden"],input[type="radio"],input[type="checkbox"],select,textarea');
        // remove the filterOptions object from the object literal (json) that will be passed to dataTables
        delete settings.filterOptions;
        if (filterOptions === undefined) {
            throw {
                name: 'filterOptionsUndefinedError',
                message: 'Please define a filterOptions property in the object literal'
            };
        }
        if (filterOptions.searchButton === undefined) {
            throw {
                name: 'searchButtonUndefinedError',
                message: 'Please define a searchButton in the filterOptions'
            };
        }

        if (filterOptions.searchContainer === undefined) {
            throw {
                name: 'searchContainerUndefinedError',
                message: 'Please define a searchContainer in the filterOptions'
            };
        }
        $searchContainerInputs.keypress(function (e) {
            if (e.keyCode === 13) {
                $("#" + filterOptions.searchButton).click();
            }
        });
        var SearchQuery = function () {
            var searchContainer = $("#" + filterOptions.searchContainer);
            searchContainer.find('input[type="text"][value!=""],input[type="hidden"],input[type="radio"]:checked,input[type="checkbox"]:checked,textarea[value!=""],select[value!=""]').each(function () {
                // all textboxes, radio buttons, checkboxes, textareas, and selects that actually have a value associated with them
                var element = $(this), value = element.val();
                //alert(element.attr("name"))
                //alert($(this).attr("type"))
                if (element.attr("type") == "checkbox") {
                    //alert(8888)
                    //alert(element.attr("type"))
                    ckFramework.SearchCriterias.push({
                        "name": element.attr("name"), "value": "true"
                    });
                    searchCriteria.push({ "name": element.attr("name"), "value": "true" });
                } else {
                    if (typeof value === "string") {
                        ckFramework.SearchCriterias.push({ "name": element.attr("name"), "value": value });
                        searchCriteria.push({ "name": element.attr("name"), "value": value });
                    }
                    else if (Object.prototype.toString.apply(value) === '[object Array]') {
                        // multi select since it has an array of selected values
                        var i;
                        for (i = 0; i < value.length; i++) {
                            ckFramework.SearchCriterias.push({ "name": element.attr("name"), "value": value[i] });
                            searchCriteria.push({ "name": element.attr("name"), "value": value[i] });
                        }
                    }
                    else {
                        ckFramework.SearchCriterias.push({ "name": element.attr("name"), "value": value });
                    }
                }
            });
        }
        $("#" + filterOptions.searchButton).click(function () {
            ckFramework.SearchCriterias = [];
            searchCriteria = [];
            SearchQuery();
            if (ckFramework.ClientCustomSearch) {
                for (var i = 0; i < ckFramework.ClientCustomSearch.length; i++) {
                    searchCriteria.push(ckFramework.ClientCustomSearch[i]);
                }
            };
            $dataTable.data = searchCriteria;
            // force dataTables to make a server-side request
            //$dataTable.draw();
            filterOptions.DrawTable();
        });
        if (ckFramework.ClientCustomSearch) {
            SearchQuery();
            for (var i = 0; i < ckFramework.ClientCustomSearch.length; i++) {
                searchCriteria.push(ckFramework.ClientCustomSearch[i]);
            }
        };
        settings.processing = true;
        settings.serverSide = true;
        settings.deferRender = true;
        settings.scrollCollapse = true;
        settings.drawCallback = function (backSettings) {
            if (!isInTab) {
                var $table = $('#' + backSettings.sTableId).dataTable();
                try {
                    $table.fnAdjustColumnSizing(false);
                } catch (ex) { }
                var rowLength = $('#' + backSettings.sTableId + ' tbody tr').length;
                if (rowLength == 0) {
                    rowLength = 1;
                }
                var rowHeight = rowLength * 46 + 5;

                //
                var $dataTableWrapper = $('#' + backSettings.sTableId).closest(".dataTables_wrapper");
                //var panelHeight = $dataTableWrapper.parent().height() + (ckFramework.ModalHelper.IsAddRow ? 45 : 0);

                //$dataTableWrapper.find(".fg-toolbar").each(function (i, obj) {
                //    toolbarHeights = toolbarHeights + $(obj).height();
                //});

                var toolbarHeights = $("#divToolbar").height();
                var pageHeight = $(".paging_simple_numbers").height();
                var searchHeight = $("#SearchContainer").height();
                var scrollHeadHeight = $dataTableWrapper.find(".dataTables_scrollHead").height();

                var windowsHeight = $(window).height();
                if (settings.tableHeight) {
                    //200为上下一些控件的固定高度
                    $dataTableWrapper.find(".dataTables_scrollBody").height(settings.tableHeight + "px")
                }
                else {
                    //200为上下一些控件的固定高度
                    $dataTableWrapper.find(".dataTables_scrollBody").height(windowsHeight - (toolbarHeights + pageHeight + searchHeight + scrollHeadHeight + 200) + "px")
                }
                try{
                    $table._fnScrollDraw();
                } catch (ex) { }
            }
        }
        settings.ajax = {
            url: filterOptions.Url,
            type: 'POST',
            data: function (d) {
                //alert(JSON.stringify(d));
                //alert(JSON.stringify(searchCriteria))
                var sd = {};
                sd.PageStart = d.start;
                sd.PageSize = d.length;
                //sd.Draw = d.draw;
                for (var i = 0; i < searchCriteria.length; i++) {
                    sd[searchCriteria[i].name] = searchCriteria[i].value;
                    //sd = $.extend({}, sd, { "dd" : searchCriteria[i].value} );
                }

                var sdata = { search: sd };
                //alert(JSON.stringify(sdata));
                return sdata;
            },
            "dataSrc": function (json) {
                // 获取除了表格内容的其他数据
                ckFramework.otherData = json.otherData;
                return json.data;
            },
            "error": function () {
            }
        };
        return $dataTable.DataTable(settings);
    };
}(jQuery));

ckFramework.ModalHelper.IsAddRow = false;

ckFramework.GetAngularService = function (serverName) {
    var eleminjector = angular.element(document.querySelector('[ng-controller]')).injector();
    var angularService = eleminjector.get(serverName);

    return angularService;
}

var DeptTypeInfos = {
    RootNode: 1,
    WuYE: 10,
    XiaoQu: 11,
    LouYu: 12,
    CheKu: 13,
    FangWu: 20,
    CheWei: 21,
    CheLiang: 22,
    GongGongZiYuan: 23,
    SheBei: 24,
    KaiFaShang: 30,
    UserOwner: 31,
    Others: 100
};

var RootDeptId = "3";
var RootDeptName = "逸社区";
var RootDeptType = 1;

var RootDeptIdAndType = "#3_1";

var DeptContainerType = {
    Xiaoqu: 1,
    FangWu: 2,
    CheWei: 3,
    CheKu: 4,
    KaiFaShang: 5,
    WangGuan: 6,
    WifiAuthHistory: 7,
    WifiAuthCurrent: 8,
};

ckFramework.IsFirstAdminLTETree = true; // 控制左侧菜单Active样式

//保留两位小数  
//功能：将浮点数取小数点后2位 
//@x:要处理的浮点数
//by:yangbo
ckFramework.toDecimal = function (x) {
    var new_x;
    var f = parseFloat(x);
    if (isNaN(f)) {
        return new_x;
    }
    if (x.toString().indexOf(".") > -1) {
        var strs = x.toString().split('.');
        new_x = strs[0] + ".";
        if (strs[1].length < 3) {
            new_x += strs[1];
        } else {
            new_x += strs[1].substr(0, 2);
        }
    } else
        new_x = x;
    return new_x;
}
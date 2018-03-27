var ckFramework = ckFramework || {};
/*
    createFormData{
        FormId, // 表单Id
        DivContainer, // container容器
        TemplateItemDatas, // 模版数据
        ColumnCount, // 每一行显示的列数
        Language, // 语言

        extends:
            ColumnClass, // 行样式
    }
*/
ckFramework.FormService = (function (formService) {
    var formData;
    var clientMessage;
    var InitParams = function (createFormData) {
        formData = createFormData;
        switch (formData.ColumnCount) {
            case 1:
                formData.ColumnClass = 'col-md-12';
                break;
            case 2:
                formData.ColumnClass = 'col-md-6';
                break;
            case 3:
                formData.ColumnClass = 'col-md-4';
                break;
            default:
                formData.ColumnClass = 'col-md-4';
                break;
        };
    };

    var CheckReadonly = function (fieldId) {
        if (formData.ReadonlyForm) {
            return true;
        }
        if (formData.ReadonlyDatas) {
            for (var i = 0; i < formData.ReadonlyDatas.length; i++) {
                if (fieldId == formData.ReadonlyDatas[i]) {
                    return true;
                }
            }
        }
        return false;
    }

    var CreateForm = function () {
        var form = $('<div />', {
            'id': formData.FormId,
            'class': 'form-horizontal'
        });

        for (var i = 0; i < formData.TemplateItemDatas.length; i++) {
            var divRow = $('<div />', {
                'class': 'row'
            });
            for (var j = 0; j < formData.ColumnCount; j++) {
                var isJump = true;
                while (isJump && (i < formData.TemplateItemDatas.length)) {
                    isJump = false;
                    for (var k = 0; k < formData.jumpFields.length; k++) {
                        if (formData.TemplateItemDatas[i].Field == formData.jumpFields[k]) {
                            isJump = true;
                            i++;
                            break;
                        }
                    };
                };

                CreateContent(divRow, formData.TemplateItemDatas[i]);
                i++;
                if (i >= formData.TemplateItemDatas.length) {
                    break;
                };
            };
            i--;
            divRow.appendTo(form);
        };

        form.appendTo(formData.DivContainer);
    };

    // 创建ColumnContent
    var CreateContent = function (divRow, templateItemData) {
        if (templateItemData) {
            var divColumn = $('<div />', {
                'class': 'form-group ModalFormMagin ' + formData.ColumnClass
            });

            CreateColumnLable(divColumn, templateItemData);
            CreateColumnContent(divColumn, templateItemData);
            divColumn.appendTo(divRow);
        }
    };

    // 创建ColumnTitle
    var CreateColumnLable = function (divColumn, templateItemData) {
        var lblColumn = $('<lable />', {
            'class': 'control-label ckFormControlLable'
        });
        var strColumnTitle;
        if (templateItemData.IsRequred) {
            strColumnTitle = '*' + (ckFramework.HomeData.Language == 'zh-CN' ? templateItemData.CnName : templateItemData.EnName);
        }
        else {
            strColumnTitle = ckFramework.HomeData.Language == 'zh-CN' ? templateItemData.CnName : templateItemData.EnName;
        };
        $('<span>' + strColumnTitle + '</span>').appendTo(lblColumn);

        lblColumn.appendTo(divColumn);
    };

    var CreateColumnContent = function (divColumn, templateItemData) {
        switch (templateItemData.Type) {
            case 'string':
                CreateStringContent(divColumn, templateItemData);
                break;
            case 'datetime':
                CreateDateTimeContent(divColumn, templateItemData, false);
                break;
            case 'date':
                CreateDateTimeContent(divColumn, templateItemData, true);
                break;
            case 'double':
                CreateStringContent(divColumn, templateItemData);
                break;
            case 'int':
                CreateStringContent(divColumn, templateItemData);
                break;
            case 'dict':
                CreateDictContent(divColumn, templateItemData);
                break;
            case 'bool':
                CreateBoolContent(divColumn, templateItemData);
                break;
            case 'decimal':
                CreateStringContent(divColumn, templateItemData);
        }
    };

    var CreateStringContent = function (divColumn, templateItemData) {
        var columnContent;
        switch (templateItemData.ElementType) {
            case 'Lable':
                columnContent = $('<lable class="form-control ckFormControl" for="' + templateItemData.Field + '" style="border:0px">{{FormData.' + templateItemData.Field + '}}</lable>');
                break;
            case 'TextBox':
                columnContent = $('<input />', {
                    'type': 'text',
                    'class': 'form-control ckFormControl',
                    'name': templateItemData.Field,
                    'ng-model': 'FormData.' + templateItemData.Field,
                    'ng-readonly': CheckReadonly(templateItemData.Field)
                });
                break;
            case 'TextArea':
                columnContent = $('<textarea />', {
                    'rows': '3',
                    'class': 'form-control ckFormControl',
                    'name': templateItemData.Field,
                    'ng-model': 'FormData.' + templateItemData.Field,
                    'ng-readonly': CheckReadonly(templateItemData.Field)
                });
                break;
            default:
                columnContent = $('<input />', {
                    'type': 'text',
                    'class': 'form-control ckFormControl',
                    'name': templateItemData.Field,
                    'ng-model': 'FormData.' + templateItemData.Field,
                });
                break;
        }
        //alert(templateItemData.IsAttr)
        if (templateItemData.IsAttr && templateItemData.IsAttr == true) {
            //alert(templateItemData.Attrs)
            for (var i = 0; i < templateItemData.Attrs.length; i++) {
                $(columnContent).attr(templateItemData.Attrs[i].Name, templateItemData.Attrs[i].Val);
            }
        }

        columnContent.appendTo(divColumn);
    };
    /*
    <input type="checkbox" name="favoriteColors" ng-model="formData.favoriteColors.red"> Red
    */
    var CreateBoolContent = function (divColumn, templateItemData) {
        var columnContent = $('<input />', {
            'type': 'checkbox',
            'name': templateItemData.Field,
            'ng-model': 'FormData.' + templateItemData.Field,
            'ng-disabled': CheckReadonly(templateItemData.Field)
        });

        $('<br />').appendTo(divColumn);
        columnContent.appendTo(divColumn);
        $('<span>' + GetFieldName(templateItemData) + '</span>').appendTo(divColumn);
    };

    var CreateDateTimeContent = function (divColumn, templateItemData, isDateTime) {
        var tempField = templateItemData.Field;
        var divDateTime = $('<div />', {
            'class': 'input-append date input-group ckFormControl',
        });
        var readonly = CheckReadonly(templateItemData.Field);
        $('<input />', {
            'type': 'text',
            'name': templateItemData.Field,
            'class': 'form-control',
            'date-time-picker': '',
            'timeformat': (isDateTime ? 'yyyy-MM-dd' : 'yyyy-MM-dd hh:mm'),
            'recipient': 'FormData.' + templateItemData.Field,
            'ng-model': 'FormData.' + templateItemData.Field,
            'ng-readonly': readonly
        }).appendTo(divDateTime);
        if (!readonly) {
            $('<span class="input-group-addon"><span class="glyphicon glyphicon-calendar"></span></span>').appendTo(divDateTime);

            divDateTime.datetimepicker({
                format: isDateTime ? 'yyyy-mm-dd' : 'yyyy-mm-dd hh:ii'//,
                //pickerPosition: "bottom-left",
                //language: "zh-CN"
            });
            divDateTime.on("dp.change", function (e) {
                $('#' + formData.FormId).bootstrapValidator('revalidateField', tempField);
                var formScope = angular.element(document.getElementById(formData.DivContainer.parents('div[ng-controller]').first().attr('id'))).scope();
                eval("formScope.FormData." + templateItemData.Field + "=e.date.toDate()");
                formScope.$apply();
            });
        }
        divDateTime.appendTo(divColumn);
    };

    var CreateDictContent = function (divColumn, templateItemData) {
        var columnContent = $('<select convert-to-number ng-disabled=' + CheckReadonly(templateItemData.Field) + ' ng-model="FormData.' + templateItemData.Field + '" class="form-control ckFormControl" name="' + templateItemData.Field + '"/>');

        for (var i = 0; i < templateItemData.DictionaryModels.length; i++) {
            $('<option value="' + templateItemData.DictionaryModels[i].Code + '">' + GetFieldName(templateItemData.DictionaryModels[i]) + '</option>').appendTo(columnContent);
        }

        if (templateItemData.IsAttr && templateItemData.IsAttr == true) {
            //alert(templateItemData.Attrs)
            for (var i = 0; i < templateItemData.Attrs.length; i++) {
                $(columnContent).attr(templateItemData.Attrs[i].Name, templateItemData.Attrs[i].Val);
            }
        }

        columnContent.appendTo(divColumn);
    };

    formService.CreateBusinessForm = function (createFormData) {
        InitParams(createFormData);
        CreateForm();
    };

    var GetFieldName = function (itemData) {
        return (ckFramework.HomeData.Language == 'zh-CN' ? itemData.CnName : itemData.EnName);
    };

    var AddNotEmptyValidate = function (templateItemData, formValidate) {
        if (templateItemData.IsRequred) {
            formValidate[templateItemData.Field].validators.notEmpty = { message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateNotEmpty };
        };
    };

    // 验证正则表达式
    var AddRegexpValidate = function (templateItemData, formValidate) {
        if (templateItemData.Regular) {
            formValidate[templateItemData.Field].validators.regexp = {
                message: '"' + GetFieldName(templateItemData) + '"' + templateItemData.ExportComments,
                regexp: templateItemData.Regular
            };
        };
    };

    // 输入长度验证
    var ValidateStringLength = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        AddNotEmptyValidate(templateItemData, formValidate);
        formValidate[templateItemData.Field].validators.stringLength = {
            max: templateItemData.MaxLength,
            message: clientMessage.ValidateErrorMessage + '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateLength + templateItemData.MaxLength
        };
        ValidateSpecialVal(templateItemData, formValidate);
    };

    // 验证Email
    var ValidateEmailAddress = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        AddNotEmptyValidate(templateItemData, formValidate);
        formValidate[templateItemData.Field].validators.emailAddress = {
            message: clientMessage.ValidateErrorMessage + '"' + GetFieldName(templateItemData) + '"'
        };
    };

    // 验证Phone
    var ValidatePhone = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        AddNotEmptyValidate(templateItemData, formValidate);
        formValidate[templateItemData.Field].validators.phone = {
            country: 'CN',
            message: clientMessage.ValidateErrorMessage + '"' + GetFieldName(templateItemData) + '"'
        };
    };

    // 验证Double
    var ValidateDouble = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        AddNotEmptyValidate(templateItemData, formValidate);
        formValidate[templateItemData.Field].validators.callback = {
            message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateDouble,
            callback: function (value, validator, $field) {
                if (value.length != 0) {
                    //var reg = /^[0-9]\d+(\.\d+)?$/;
                    var reg = /^([1-9]\d*|\d+\.\d{1,2})$/;
                    return reg.test(value);
                }
                return true;
            }
        }
    };

    // 验证Int
    var ValidateInt = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        AddNotEmptyValidate(templateItemData, formValidate);
        formValidate[templateItemData.Field].validators.callback = {
            message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateInt,
            callback: function (value, validator, $field) {
                if (value.length != 0) {
                    //var reg = /^[-+]?\d*$/;
                    var reg = /^[1-9]+\d{0,}$/;
                    return reg.test(value);
                }
                return true;
            }
        }
    };

    // 验证正则表达式
    var ValidateSpecialVal = function (templateItemData, formValidate) {
        if (templateItemData.SkipSpecialCheck) {
            return;
        }
        formValidate[templateItemData.Field].validators.regexp = {
            message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateNotSpecial,
            //regexp: /^[^~#^$@%&!*]?[\u4e00-\u9fa5_a-zA-Z0-9.-]+$/
            regexp: /^[^~#^$@%&!*]+$/
        };
    };

    // 验证身份证表达式
    var ValidateIdcardVal = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        formValidate[templateItemData.Field].validators.callback = {
            message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.IdCard,
            callback: function (value, validator, $field) {
                if (value.length != 0) {
                    var reg = /^[1-9]\d{7}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}$|^[1-9]\d{5}[1-9]\d{3}((0\d)|(1[0-2]))(([0|1|2]\d)|3[0-1])\d{3}([0-9]|X)$/;
                    return reg.test(value);
                }
                return true;
            }
        }
    };

    //验证移动电话
    var ValidateMobileVal = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        formValidate[templateItemData.Field].validators.callback = {
            message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateMobile,
            callback: function (value, validator, $field) {
                //手机号码为必填，验证长度
                if (value.length == 0 && templateItemData.IsRequred) {
                    return false;
                }
                if (value.length != 0) {
                    //var reg = /^0{0,1}(13[4-9]|15[7-9]|15[0-2]|18[7-8])[0-9]{8}$/;
                    //var reg = /^(13[0-9]{9}|17[0-9]{9}|15[0-9]{9}|18[0-9]{9}|147[0-9]{8})$/;
                    var reg = /^(13[0-9]{9}|17[678][0-9]{8}|170[059][0-9]{7}|15[012356789][0-9]{8}|18[0-9]{9}|14[57][0-9]{8})$/;
                    return reg.test(value);
                }
                return true;
            }
        }
    }

    //验证固定电话号码
    var ValidateTelVal = function (templateItemData, formValidate) {
        formValidate[templateItemData.Field] = {};
        formValidate[templateItemData.Field].validators = {};
        formValidate[templateItemData.Field].validators.callback = {
            message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateTel,
            callback: function (value, validator, $field) {
                if (value.length != 0) {
                    var reg = /^((\d{11})|^((\d{7,8})|(\d{4}|\d{3})-(\d{7,8})|(\d{4}|\d{3})-(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1})|(\d{7,8})-(\d{4}|\d{3}|\d{2}|\d{1}))$)$/;
                    return reg.test(value);
                }
                return true;
            }
        }
    }

    // 验证Date
    var ValidateDateAndTime = function (templateItemData, formValidate, isDateTime) {
        if (templateItemData.IsRequred) {
            formValidate[templateItemData.Field] = {};
            formValidate[templateItemData.Field].validators = {};
            AddNotEmptyValidate(templateItemData, formValidate);
        };
    };

    // 验证下拉框
    var ValidateDict = function (templateItemData, formValidate) {
        if (templateItemData.IsRequred) {
            formValidate[templateItemData.Field] = {};
            formValidate[templateItemData.Field].validators = {};
            formValidate[templateItemData.Field].validators.callback = {
                message: '"' + GetFieldName(templateItemData) + '"' + clientMessage.ValidateNotEmpty,
                callback: function (value, validator, $field) {
                    return !(value == '? number:0 ?' || value == "? string:null ?" || value == "? string:0 ?");
                }
            }
        };
    };

    formService.CreateValidate = function (createFormData) {
        formData = createFormData;
        var formValidate = {};
        clientMessage = ckFramework.ClientMessage.GetMessage();

        for (var i = 0; i < createFormData.TemplateItemDatas.length; i++) {
            var tempTemplateItemData = createFormData.TemplateItemDatas[i];
            switch (tempTemplateItemData.ValidateType) {
                case 'stringLength':
                    ValidateStringLength(tempTemplateItemData, formValidate);
                    break;
                case 'phone':
                    ValidatePhone(tempTemplateItemData, formValidate);
                    break;
                case 'emailAddress':
                    ValidateEmailAddress(tempTemplateItemData, formValidate);
                    break;
                case 'date':
                    ValidateDateAndTime(tempTemplateItemData, formValidate, false);
                    break;
                case 'datetime':
                    ValidateDateAndTime(tempTemplateItemData, formValidate, true);
                    break;
                case 'dict':
                    ValidateDict(tempTemplateItemData, formValidate);
                    break;
                case 'double':
                    ValidateDouble(tempTemplateItemData, formValidate);
                    break;
                case 'int':
                    ValidateInt(tempTemplateItemData, formValidate);
                    break;
                case 'idcard':
                    ValidateIdcardVal(tempTemplateItemData, formValidate);
                    break;
                case 'mobile':
                    ValidateMobileVal(tempTemplateItemData, formValidate);
                    break;
                case 'tel':
                    ValidateTelVal(tempTemplateItemData, formValidate);
                    break;
            }
            AddRegexpValidate(tempTemplateItemData, formValidate);

        };

        $('#' + createFormData.FormId).formValidation({
            message: 'This value is not valid',
            excluded: [':disabled'],
            locale: ckFramework.HomeData.Language,
            icon: {
                valid: 'glyphicon glyphicon-ok',
                invalid: 'glyphicon glyphicon-remove',
                validating: 'glyphicon glyphicon-refresh'
            },
            fields: formValidate
        });
    };

    return formService;
}(ckFramework.FormService || {}));


/*
createTableData{
    TableId:表格Id,
    IsAddOperate:是否添加操作列,
    Language: // 语言
    OperateColumn: // 操作列
    DivTable: // Table的Div容器,
    TemplateItemDatas: //模版列
}

[
                { "data": null, "orderable": false, "defaultContent": '<a herf="#" class="btn columnTool Edit"><i class="ui-tooltip fa fa-edit" data-original-title="Edit"></i></a><a herf="#" class="btn columnTool"><i class="ui-tooltip fa fa-trash-o" data-original-title="Delete"></i></a>' },
                { "data": "UserName" },
                { "data": "Password", "orderable": false, "visible": false },
                { "data": "RealName" },
                { "data": "IsEnabled", "orderable": false },
                { "data": "MobilePhone", "orderable": false },
                { "data": "OfficePhone", "orderable": false },
                { "data": "Email", "orderable": false }
                ]
*/
ckFramework.TableService = (function (tableService) {
    var GetFieldName = function (itemData) {
        return (ckFramework.HomeData.Language == 'zh-CN' ? itemData.CnName : itemData.EnName);
    };

    tableService.CreateBusinessTable = function (createTableData) {
        var table = $('<table />', {
            'id': createTableData.TableId,
            'class': 'table table-bordered table-striped dataTable'
        });

        var thead = $('<thead />');
        var tr = $('<tr />');
        if (createTableData.IsAddOperate) {
            if (createTableData.IsAddAllCheck) {
                var ck_all_check_id = 'tableallCheck';
                $('<th style="word-wrap: break-word;white-space: nowrap;"><input type="checkbox"  id = "' + ck_all_check_id + '" onclick="allTableCheck(this)">全选</th>').appendTo(tr);
            } else if (createTableData.IsAddAllCheck1) {
                var ck_all_check_id1 = 'tableallCheckView';
                $('<th style="word-wrap: break-word;white-space: nowrap;"><input type="checkbox"  id = "' + ck_all_check_id1 + '" onclick="allCheckView()">全选</th>').appendTo(tr);
            }
            else if (createTableData.IsAddAllCheck2) {
                var ck_all_check_id2 = 'tableallCheckList';
                $('<th style="word-wrap: break-word;white-space: nowrap;"><input type="checkbox"  id = "' + ck_all_check_id2 + '" onclick="allCheckList()">全选</th>').appendTo(tr);
            }
            else {
                $('<th style="word-wrap: break-word;white-space: nowrap;">' + ckFramework.ClientMessage.GetMessage().Operate + '</th>').appendTo(tr);
            }
        }



        for (var i = 0; i < createTableData.TemplateItemDatas.length; i++) {
            if (createTableData.jumpFields) {
                var isJump = false;
                for (var k = 0; k < createTableData.jumpFields.length; k++) {
                    if (createTableData.TemplateItemDatas[i].Field == createTableData.jumpFields[k]) {
                        isJump = true;
                        break;
                    }
                };
                if (isJump) {
                    continue;
                }
            }
            var tempItemData = createTableData.TemplateItemDatas[i];
            if (tempItemData.IsListColumn) {
                $('<th style="word-wrap: break-word;white-space: nowrap;">' + GetFieldName(tempItemData) + '</th>').appendTo(tr);
            }
        };

        tr.appendTo(thead);
        thead.appendTo(table);
        $('<tbody />').appendTo(table);
        table.appendTo(createTableData.DivTable)
    };

    tableService.CreateTableColumns = function (createTableData) {
        var columns = [];
        if (createTableData.IsAddOperate) {
            columns.push(createTableData.OperateColumn);
        }
        for (var i = 0; i < createTableData.TemplateItemDatas.length; i++) {
            if (createTableData.jumpFields) {
                var isJump = false;
                for (var k = 0; k < createTableData.jumpFields.length; k++) {
                    if (createTableData.TemplateItemDatas[i].Field == createTableData.jumpFields[k]) {
                        isJump = true;
                        break;
                    }
                };
                if (isJump) {
                    continue;
                }
            }
            var tempItemData = createTableData.TemplateItemDatas[i];
            if (tempItemData.IsListColumn) {
                var column = {};
                column.data = tempItemData.Field;
                column.orderable = tempItemData.IsOrderColumn;
                if (tempItemData.Type == 'dict') {
                    column.render = function (data, type, full, meta) {
                        if (type != 'display') {
                            return data;
                        }
                        for (var k = 0; k < createTableData.TemplateItemDatas.length; k++) {
                            if (GetFieldName(createTableData.TemplateItemDatas[k]) == $($('#' + meta.settings.sTableId).DataTable().column(meta.col).header()).text().trim()) {
                                for (var j = 0; j < createTableData.TemplateItemDatas[k].DictionaryModels.length; j++) {
                                    if (createTableData.TemplateItemDatas[k].DictionaryModels[j].Code == data) {
                                        return GetFieldName(createTableData.TemplateItemDatas[k].DictionaryModels[j]);
                                    }
                                }
                            }
                        }

                        return data;
                    }
                }
                else if (tempItemData.Type == 'bool') {
                    column.render = function (data, type, full, meta) {
                        if (data) {
                            return "是";
                        }
                        else {
                            return "否";
                        }
                    }
                }
                else if (tempItemData.Type == 'datetime') {
                    column.render = function (data, type, full, meta) {
                        return timeFormatter(data);
                    }
                }
                else if (tempItemData.Type == 'date') {
                    column.render = function (data, type, full, meta) {
                        return dateFormatter(data);
                    }
                }

                columns.push(column);
            }
        };

        return columns;
    }

    return tableService;
}(ckFramework.TableService || {}));

//function timeFormatter(value) {
//    if (value == null) {
//        return "";
//    }
//    if (value.indexOf('/Date(') >= 0) {
//        var da = new Date(parseInt(value.replace("/Date(", "").replace(")/", "").split("+")[0]));

//        return da.getFullYear() + "-" + (da.getMonth() + 1) + "-" + da.getDate() + " " + da.getHours() + ":" + da.getMinutes() + ":" + da.getSeconds();
//    }
//    else {
//        return value;
//    }
//}

function timeFormatter(value) {
    if (null != value && value.indexOf('/Date(') >= 0) {
        var da = new Date(parseInt(value.replace("/Date(", "").replace(")/", "").split("+")[0]));

        var year = da.getFullYear();       //年
        var month = da.getMonth() + 1;     //月
        var day = da.getDate();            //日
        var hh = da.getHours();            //时
        var mm = da.getMinutes();          //分
        var ss = da.getSeconds();          //秒

        var clock = year + "-";

        if (month < 10)
            clock += "0";
        clock += month + "-";

        if (day < 10)
            clock += "0";
        clock += day + " ";

        if (hh < 10)
            clock += "0";
        clock += hh + ":";

        if (mm < 10)
            clock += '0';
        clock += mm + ":";

        if (ss < 10)
            clock += '0';
        clock += ss;

        return clock;
        //return da.getFullYear() + "-" + (da.getMonth() + 1) + "-" + da.getDate() + " " + da.getHours() + ":" + da.getMinutes() + ":" + da.getSeconds();
    }
    else {
        return value;
    }
}


function dateFormatter(value) {
    if (value == null) {
        return "";
    }
    if (value.indexOf('/Date(') >= 0) {
        var da = new Date(parseInt(value.replace("/Date(", "").replace(")/", "").split("+")[0]));

        var year = da.getFullYear();       //年
        var month = da.getMonth() + 1;     //月
        var day = da.getDate();            //日

        var clock = year + "-";

        if (month < 10)
            clock += "0";
        clock += month + "-";

        if (day < 10)
            clock += "0";
        clock += day + " ";

        return clock;
        //return da.getFullYear() + "-" + (da.getMonth() + 1) + "-" + da.getDate();
    }
    else {
        return value;
    }
}
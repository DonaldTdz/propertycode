var ckFramework = ckFramework || {};

ckFramework.ResourceService = (function (resourceService) {
    var _ResourceConfig;

    var _ResourcePageElements = {
        firstAddBtnId: "btnResourceFirstAdd",
        secondAddBtnId: "btnResourceSecondAdd",
        thirdAddBtnId: "btnResourceThirdAdd",
        fourthAddBtnId: "btnResourceFourthAdd",
        saveBtnId: "btnResourceSave",
        resourceViewId: "divDeptDetail",
    }

    resourceService.Init = function (resourceConfig) {
        var initConfig =
        {
            jsTreeObject: undefined,

            addFirstTitle: "新增1", // 第一个新增按钮Title
            addFirstAuthorName: "", // 第一个新增按钮权限对应名称
            addFirstUrl: "", // 第一个新增按钮加载的页面
            addFirstControllerUrl: "", // Controller地址
            addFirstControllerContext: "",
            addFirstShow: false, // 第一个新增按钮是否显示

            addSecondTitle: "新增2", // 第二个新增按钮Title
            addSecondAuthorName: "", // 第二个新增按钮权限对应名称
            addSecondUrl: "", // 第二个新增按钮加载的页面
            addSecondControllerUrl: "", // Controller地址
            addSecondControllerContext: "",
            addSecondShow: false, // 第二个新增按钮是否显示

            addThirdTitle: "新增3", // 第三个新增按钮Title
            addThirdAuthorName: "", // 第三个新增按钮权限对应名称
            addThirdUrl: "", // 第三个新增按钮加载的页面
            addThirdControllerUrl: "", // Controller地址
            addThirdControllerContext: "",
            addThirdShow: false, // 第三个新增按钮是否显示

            addFourthTitle: "新增4", // 第四个新增按钮Title
            addFourthAuthorName: "", // 第四个新增按钮权限对应名称
            addFourthUrl: "", // 第四个新增按钮加载的页面
            addFourthControllerUrl: "", // Controller地址
            addFourthControllerContext: "",
            addFourthShow: false, // 第四个新增按钮是否显示

            editType: "Edit",

            saveTitle: "保存", // 保存按钮Title
            SaveShow: true, // 第三个新增按钮是否显示
            SaveAuthorName: "", // 第四个新增按钮权限对应名称
            saveAddValidateMethod: undefined, // 新增保存验证方法
            saveEditValidateMethod: undefined, // 编辑保存验证方法
            saveAddMethod: undefined, // 新增保存方法
            saveEditMethod: undefined, // 编辑保存方法
        };
        _ResourceConfig = $.extend(true, {}, initConfig, resourceConfig);

        SetResourceButtonShow((_ResourceConfig.addFirstShow && _ResourceConfig.editType == "Edit"), _ResourceConfig.addFirstAuthorName, _ResourcePageElements.firstAddBtnId, _ResourceConfig.addFirstTitle);
        SetResourceButtonShow((_ResourceConfig.addSecondShow && _ResourceConfig.editType == "Edit"), _ResourceConfig.addSecondAuthorName, _ResourcePageElements.secondAddBtnId, _ResourceConfig.addSecondTitle);
        SetResourceButtonShow((_ResourceConfig.addThirdShow && _ResourceConfig.editType == "Edit"), _ResourceConfig.addThirdAuthorName, _ResourcePageElements.thirdAddBtnId, _ResourceConfig.addThirdTitle);
        SetResourceButtonShow((_ResourceConfig.addFourthShow && _ResourceConfig.editType == "Edit"), _ResourceConfig.addFourthAuthorName, _ResourcePageElements.fourthAddBtnId, _ResourceConfig.addFourthTitle);
        SetResourceButtonShow((_ResourceConfig.SaveShow), _ResourceConfig.SaveAuthorName, _ResourcePageElements.saveBtnId, _ResourceConfig.saveTitle);
    }

    // 设置按钮是否显示
    var SetResourceButtonShow = function (isBtnShow, authorityName, btnId, btnTitle) {
        if (isBtnShow && ckFramework.OperateAuthorityService.CheckAuthority(authorityName)) {
            $("#" + btnId).show().val(btnTitle);
        }
        else {
            $("#" + btnId).hide();
        }
    }

    var ResourceNodeView = function (controllerUrl, viewUrl, controllerContext, rootScope, compile) {
        ckFramework.ModalHelper.OpenWait();
        if (viewUrl.indexOf('?')<1)
        {
            viewUrl += '?PId=' + $('#SelectDeptId').val();
        }
        else {
            viewUrl += '&PId=' + $('#SelectDeptId').val();
        }
        require(['apps/HomeApp', controllerUrl], function (app) {
            $("#" + _ResourcePageElements.resourceViewId).load(viewUrl, function () {
                compile($('#' + controllerContext))(rootScope);
                rootScope.$apply();
                ckFramework.ModalHelper.CloseWait();
            });
        });

        var currentNode = _ResourceConfig.jsTreeObject.jstree("get_selected");
        _ResourceConfig.jsTreeObject.jstree("open_node", currentNode);
    }

    resourceService.AddFirst = function (rootScope, compile) {
        ResourceNodeView(_ResourceConfig.addFirstControllerUrl, _ResourceConfig.addFirstUrl, _ResourceConfig.addFirstControllerContext, rootScope, compile);
    }

    resourceService.AddSecond = function (rootScope, compile) {
        ResourceNodeView(_ResourceConfig.addSecondControllerUrl, _ResourceConfig.addSecondUrl, _ResourceConfig.addSecondControllerContext, rootScope, compile);
    }

    resourceService.AddThird = function (rootScope, compile) {
        ResourceNodeView(_ResourceConfig.addThirdControllerUrl, _ResourceConfig.addThirdUrl, _ResourceConfig.addThirdControllerContext, rootScope, compile);
    }

    resourceService.AddFourth = function (rootScope, compile) {
        ResourceNodeView(_ResourceConfig.addFourthControllerUrl, _ResourceConfig.addFourthUrl, _ResourceConfig.addFourthControllerContext, rootScope, compile);
    }

    resourceService.Save = function () {
        if (_ResourceConfig.editType == "Edit") {
            if (_ResourceConfig.saveEditValidateMethod && _ResourceConfig.saveEditValidateMethod()) {
                _ResourceConfig.saveEditMethod();
            }
        }
        else if (_ResourceConfig.editType == "Add") {
            if (_ResourceConfig.saveAddValidateMethod && _ResourceConfig.saveAddValidateMethod()) {
                _ResourceConfig.saveAddMethod();
            }
        }
    }

    resourceService.AddedCallback = function (deptType,deptId,deptName) {
        // 在树节点新增一条数据，并定位到新增的数据
        var currentNode = _ResourceConfig.jsTreeObject.jstree("get_selected");
        var deptIcon;
        switch(deptType)
        {
            case DeptTypeInfos.WuYE:
                deptIcon = "fa fa-briefcase";
                break;
            case DeptTypeInfos.XiaoQu:
                deptIcon = "fa fa-comments";
                break;
            case DeptTypeInfos.LouYu:
                deptIcon = "fa fa-building";
                break;
            case DeptTypeInfos.FangWu:
                deptIcon = "fa fa-institution";
                break;
            case DeptTypeInfos.CheWei:
                deptIcon = "fa fa-car";
                break;
            default:
                deptIcon = "fa fa-recycle";
                break;
        }
        var id = _ResourceConfig.jsTreeObject.jstree('create_node', currentNode, { icon: deptIcon, id: deptId.toString() + '_' + deptType.toString(), text: deptName, children: [] }, 'first');
        _ResourceConfig.jsTreeObject.jstree("deselect_all");
        _ResourceConfig.jsTreeObject.jstree("select_node", deptId.toString() + '_' + deptType.toString(), false);
        $('#SelectDeptId').val(deptId);
    }

    resourceService.EditCallback = function (nodeName) {
        // 修改树节点名称
        var currentNode = _ResourceConfig.jsTreeObject.jstree("get_selected");
        _ResourceConfig.jsTreeObject.jstree('set_text', currentNode, nodeName);
    }

    return resourceService;
}(ckFramework.ResourceService || {}));

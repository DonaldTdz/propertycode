var ckFramework = ckFramework || {};

ckFramework.BalanceRedPacketService = (function (balanceRedPacketService) {
    var _ResourceConfig;

    // -------------------------------Public Methods and Properties ---------------------
    balanceRedPacketService.ParentNoteIds;

    balanceRedPacketService.Init = function (balanceRedPacketConfig) {
        var initConfig =
        {
            selectParentNodeIds: [], // 编辑保存方法
            angularControllerDiv: "", // angular 控制器Div
        };
        _ResourceConfig = $.extend(true, {}, initConfig, balanceRedPacketConfig);
        balanceRedPacketService.ParentNoteIds = _ResourceConfig.selectParentNodeIds;
    }

    balanceRedPacketService.MakeResultNodes = function (treeInstance, selectedNodes) {
        var wuyeIds = "";
        var wuyeTexts = "";
        var xiaoquIds = "";
        var xiaoquCodes = "";
        var xiaoquTexts = "";
        var merchantIds = "";
        var merchantNames = "";
        var parentIds = Unique(balanceRedPacketService.ParentNoteIds);
        var parentNodes = [];
        for (var i = 0; i < parentIds.length; i++) {
            parentNodes.push(treeInstance.get_node(parentIds[i]));
        }

        var allNeedNodes = selectedNodes.concat(parentNodes);
        var wuyeNodes = $.grep(allNeedNodes, function (val, index) { // 获取所有选中的物业
            return val.icon == "fa fa-briefcase";
        });
        wuyeNodes = Unique(wuyeNodes).sort(function (first, second) {
            return parseInt(first.id.split("_")[0]) > parseInt(second.id.split("_")[0]);
        });
        for (var i = 0; i < wuyeNodes.length; i++) {
            wuyeIds = wuyeIds + wuyeNodes[i].id.split("_")[0] + ";";
            wuyeTexts = wuyeTexts + wuyeNodes[i].text + ";";
        }

        var xiaoquNodes = $.grep(allNeedNodes, function (val, index) { // 获取所有选中的小区
            return val.icon == "fa fa-comments";
        });
        xiaoquNodes = Unique(xiaoquNodes).sort(function (first, second) {
            return parseInt(first.id.split("_")[0]) > parseInt(second.id.split("_")[0]);
        });
        for (var i = 0; i < xiaoquNodes.length; i++) {
            xiaoquIds = xiaoquIds + xiaoquNodes[i].id.split("_")[0] + ";";
            xiaoquCodes = xiaoquCodes + xiaoquNodes[i].id.split("_")[2] + ";";
            xiaoquTexts = xiaoquTexts + xiaoquNodes[i].text + ";";
        }

        var tempMerChants = [];
        var merchantNodes = $.grep(allNeedNodes, function (val, index) { // 获取所有选中的物业
            return val.icon == "fa fa-users";
        });
        merchantNodes = Unique(merchantNodes).sort(function (first, second) {
            return parseInt(first.id.split("_")[0]) > parseInt(second.id.split("_")[0]);
        });
        for (var i = 0; i < merchantNodes.length; i++) {
            var merchantInfo = merchantNodes[i].id.split("_")[0] + merchantNodes[i].text;
            if (!CheckMerchantInfoAdded(merchantInfo, tempMerChants)) {
                merchantIds = merchantIds + merchantNodes[i].id.split("_")[0] + ";";
                merchantNames = merchantNames + merchantNodes[i].text + ";";
            }
        }

        SetDeptTreeSelecteed(wuyeIds, wuyeTexts, xiaoquIds, xiaoquCodes, xiaoquTexts, merchantIds, merchantNames);
    }

    // -----------------------Private Methods -----------------------------------------
    var CheckMerchantInfoAdded = function (merchantInfo, tempMerChants) {
        if ($.inArray(merchantInfo, tempMerChants) == -1) {
            tempMerChants.push(merchantInfo);
            return false;
        }
        else {
            return true;
        }
    }

    var Unique = function (list) {
        var result = [];
        $.each(list, function (i, e) {
            if ($.inArray(e, result) == -1) {
                result.push(e);
            }
        });
        return result;
    }

    var SetDeptTreeSelecteed = function (wuyeIds, wuyeTexts, xiaoquIds, xiaoquCodes, xiaoquTexts, merchantIds, merchantNames) {
        var ruleController = angular.element(document.getElementById(_ResourceConfig.angularControllerDiv)).scope();
        ruleController.FormData.PropertyIds = wuyeIds;
        ruleController.FormData.PropertyNames = wuyeTexts;
        ruleController.FormData.CommunityIds = xiaoquIds;
        ruleController.FormData.CommunityCodes = xiaoquCodes;
        ruleController.FormData.CommunityNames = xiaoquTexts;
        ruleController.FormData.MerchantIds = merchantIds;
        ruleController.FormData.MerchantNames = merchantNames;
        ruleController.$apply();
    }

    return balanceRedPacketService;
}(ckFramework.BalanceRedPacketService || {}));

ckFramework.BalanceRuleService = (function (balanceRuleService) {
    var _ResourceConfig;

    // -------------------------------Public Methods and Properties ---------------------

    balanceRuleService.Init = function (balanceRuleConfig) {
        var initConfig =
        {
            privilegeType: "101", // 优惠类型
            privilegeScenario: "101", // 优惠场景
            couponDatas: [], // 优惠券数据集合 
            divBalanceRuleAddController: "divBalanceRuleAddController"
        };

        _ResourceConfig = $.extend(true, {}, initConfig, balanceRuleConfig);
        $('.BalanceRuleRow').remove();
        if (_ResourceConfig.privilegeType == "101") {
            $('#btnAddBalanceRule').show();
            //AddPayInRule();
        }
        else {
            $('#btnAddBalanceRule').show();
        }

        var ruleController = angular.element(document.getElementById(_ResourceConfig.divBalanceRuleAddController)).scope();
        ruleController.FormData.PrivilegeContent = "";
        ruleController.FormData.PrivilegeContentText = "";
        ruleController.$apply();
    }

    // 优惠类型改变时调用的方法
    balanceRuleService.SetPrivilegeTypeChangedRule = function (privilegeType, privilegeScenario, selectedCommunityCodes,selectMerchantIds) {
        var balanceRuleConfig = {};
        balanceRuleConfig.privilegeType = privilegeType;
        balanceRuleConfig.privilegeScenario = privilegeScenario;
        if (privilegeType == "301") {
            balanceRuleService.Init(balanceRuleConfig);
            SetBalanceRuleCoupon(selectedCommunityCodes, selectMerchantIds);
        }
        else {
            balanceRuleService.Init(balanceRuleConfig);
        }

        if (privilegeScenario == "301") {
            $("form").find(".row div").last().show();
        }
        else {
            $("form").find(".row div").last().hide();
        }
    }

    // 改变树选择时，调用的方法
    balanceRuleService.SetTreeSelectedChangedRule = function (privilegeType, privilegeScenario, selectedCommunityCodes, selectMerchantIds) {
        var balanceRuleConfig = {};
        balanceRuleConfig.privilegeType = privilegeType;
        if (privilegeType == "301") {
            balanceRuleService.SetPrivilegeTypeChangedRule(privilegeType, privilegeScenario, selectedCommunityCodes, selectMerchantIds);
        }
    }

    balanceRuleService.AddBalanceRule = function () {
        switch (_ResourceConfig.privilegeType) {
            case "101":
                AddPayInRule();
                break;
            case "201":
                AddCashAndIntegralRule();
                break;
            case "301":
                AddCouponRule();
                break;
            case "401":
                AddCashAndIntegralRule();
                break;
        }
    }

    balanceRuleService.CaculatePrivilegeContent = function (e) {
        switch (_ResourceConfig.privilegeType) {
            case "101":
                CaculatePayInAndValue(e);
                break;
            case "201":
                CaculateCashAndIntegralValue(e,"现金");
                break;
            case "301":
                CaculateCouponValue(e);
                break;
            case "401":
                CaculateCashAndIntegralValue(e, "积分");
                break;
        }
    }

    // -----------------------Private Methods -----------------------------------------
    // 根据选中的小区Code集合获取可使用的优惠券，在回调函数调用Init方法
    var SetBalanceRuleCoupon = function (selectedCommunityCodes, selectMerchantIds) {
        $('#btnAddBalanceRule').attr("disabled", true);
        if (_ResourceConfig.privilegeScenario == "301") {
            $.post("BalanceRedPacketRule/GetCouponByMerchantIds", { merchantIds: selectMerchantIds }, function (result) {
                _ResourceConfig.couponDatas = result;
                $('#btnAddBalanceRule').attr("disabled", false);
            });
        }
        else {
            $.post("BalanceRedPacketRule/GetCouponByCodes", { valligeCodes: selectedCommunityCodes }, function (result) {
                _ResourceConfig.couponDatas = result;
                $('#btnAddBalanceRule').attr("disabled", false);
            });
        }
    }

    // 新增现金和积分
    var AddCashAndIntegralRule = function () {
        var divCashRule = $('<div style="margin-top:5px" class="row BalanceRuleRow"/>');
        $('<div class="col-md-2">满</div>').appendTo(divCashRule);
        $('<div class="col-md-3" style="margin-left:0px; margin-right:0px;padding-left:0px;padding-right:0px"><input type="text" onchange="ckFramework.BalanceRuleService.CaculatePrivilegeContent(this)" class="PrivilegeStart" style="width:100%" /></div>').appendTo(divCashRule);
        $('<div class="col-md-2">送</div>').appendTo(divCashRule);
        $('<div class="col-md-5" style="margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 2px"><input type="text" onchange="ckFramework.BalanceRuleService.CaculatePrivilegeContent(this)" class="PrivilegeEnd" style="width:100%;margin-left: -10px;" /></div>').appendTo(divCashRule);
        divCashRule.appendTo('#divRuleContent');
    }

    // 折扣规则
    var AddPayInRule = function () {
        var divPayInRule = $('<div style="margin-top:5px" class="row BalanceRuleRow"/>');
        $('<div class="col-md-1">满</div>').appendTo(divPayInRule);
        $('<div class="col-md-3" style="margin-left:1px; margin-right:2px;padding-left:0px;padding-right:0px"><input type="text" onchange="ckFramework.BalanceRuleService.CaculatePrivilegeContent(this)" class="PrivilegeStart" style="width:100%" /></div>').appendTo(divPayInRule);
        $('<div class="col-md-3" style="margin-left: 0px; margin-right: 0px; padding-left: 0px; padding-right: 2px"><input type="text" onchange="ckFramework.BalanceRuleService.CaculatePrivilegeContent(this)" class="PrivilegeEnd" style="width:100%" /></div>').appendTo(divPayInRule);
        $('<div class="col-md-5" style="margin-left: -12px;">折(2位小数)</div>').appendTo(divPayInRule);
        divPayInRule.appendTo('#divRuleContent');
    }

    // 憎送优惠券
    var AddCouponRule = function () {
        var divCouponRule = $('<div style="margin-top:5px" class="row BalanceRuleRow"/>');
        $('<div class="col-md-2">满</div>').appendTo(divCouponRule);
        $('<div class="col-md-3" style="margin-left:0px; margin-right:0px;padding-left:0px;padding-right:0px"><input type="text" onchange="ckFramework.BalanceRuleService.CaculatePrivilegeContent(this)" class="PrivilegeStart" style="width:100%" /></div>').appendTo(divCouponRule);
        $('<div class="col-md-2">送</div>').appendTo(divCouponRule);
        var divCoupon = $('<div class="col-md-5" style="padding-right: 0px;"/>').appendTo(divCouponRule);
        var couponRuleSelect = $('<select style="width:100%;margin-left: -15px;" class="PrivilegeEnd" onchange="ckFramework.BalanceRuleService.CaculatePrivilegeContent(this)"/>').appendTo(divCoupon);
        $('<option value=""></option>').appendTo(couponRuleSelect);
        for (var i = 0; i < _ResourceConfig.couponDatas.length; i++) {
            $('<option value="' + _ResourceConfig.couponDatas[i].ID + '">' + _ResourceConfig.couponDatas[i].Name + '</option>').appendTo(couponRuleSelect);
        }
        divCouponRule.appendTo('#divRuleContent');
    }

    // 折扣计算优惠内容
    var CaculatePayInAndValue = function (e)
    {
        if ($(e).hasClass("PrivilegeStart"))
        {
            var val = $(e).val();
            if (!ckFramework.RegularService.CheckRegular(/^\d+(\.\d+)?$/, val)) {
                $(e).val("");
            }
        }

        if ($(e).hasClass("PrivilegeEnd")) {
            var val = $(e).val();
            var valText = "";

            if (!ckFramework.RegularService.CheckRegular(/^[1-9]{1}(\.\d{1,2})?$/, val)) {
                $(e).val("");
                val = "";
                valText = "";
            }
            else {
                valText = val + "折";
            }
        }

        var ruleController = angular.element(document.getElementById(_ResourceConfig.divBalanceRuleAddController)).scope();
        var privilegeStartEle = $('.PrivilegeStart');
        var privilegeEndEle = $('.PrivilegeEnd');
        var privilegeContent = "";
        var PrivilegeContentText = "";
        for (var i = 0; i < privilegeStartEle.length; i++) {
            if ($(privilegeStartEle[i]).val() && $(privilegeEndEle[i]).val()) {
                privilegeContent = privilegeContent + $(privilegeStartEle[i]).val() + "," + $(privilegeEndEle[i]).val() + ";";
                PrivilegeContentText = PrivilegeContentText + "满" + $(privilegeStartEle[i]).val() +"打"+ $(privilegeEndEle[i]).val() + "折;";
            }
        }

        ruleController.FormData.PrivilegeContent = privilegeContent;
        ruleController.FormData.PrivilegeContentText = PrivilegeContentText;
        ruleController.$apply();

        //var ruleController = angular.element(document.getElementById(_ResourceConfig.divBalanceRuleAddController)).scope();
        //ruleController.FormData.PrivilegeContent = val;
        //ruleController.FormData.PrivilegeContentText = valText;
        //ruleController.$apply();
    }

    // 现金计算优惠内容
    var CaculateCashAndIntegralValue = function (e, suffix) {
        var val = $(e).val();
        if (!ckFramework.RegularService.CheckRegular(/^\d+(\.\d+)?$/, val)) {
            $(e).val("");
        }

        var ruleController = angular.element(document.getElementById(_ResourceConfig.divBalanceRuleAddController)).scope();
        var privilegeStartEle = $('.PrivilegeStart');
        var privilegeEndEle = $('.PrivilegeEnd');
        var privilegeContent = "";
        var PrivilegeContentText = "";
        for (var i = 0; i < privilegeStartEle.length; i++) {
            if ($(privilegeStartEle[i]).val() && $(privilegeEndEle[i]).val()) {
                privilegeContent = privilegeContent + $(privilegeStartEle[i]).val() + "," + $(privilegeEndEle[i]).val() + ";";
                PrivilegeContentText = PrivilegeContentText + "满" + $(privilegeStartEle[i]).val() + "送" + $(privilegeEndEle[i]).val() + suffix + ";";
            }
        }

        ruleController.FormData.PrivilegeContent = privilegeContent;
        ruleController.FormData.PrivilegeContentText = PrivilegeContentText;
        ruleController.$apply();
    }

    // 优惠券计算优惠内容
    var CaculateCouponValue = function (e) {
        var val = $(e).val();
        if (!ckFramework.RegularService.CheckRegular(/^\d+(\.\d+)?$/, val)) {
            $(e).val("");
        }

        var ruleController = angular.element(document.getElementById(_ResourceConfig.divBalanceRuleAddController)).scope();
        var privilegeStartEle = $('.PrivilegeStart');
        var privilegeEndEle = $('.PrivilegeEnd');
        var privilegeContent = "";
        var PrivilegeContentText = "";
        for (var i = 0; i < privilegeStartEle.length; i++) {
            
            if ($(privilegeStartEle[i]).val() && $(privilegeEndEle[i]).val()) {
                privilegeContent = privilegeContent + $(privilegeStartEle[i]).val() + "," + $(privilegeEndEle[i]).val() + ";";
                PrivilegeContentText = PrivilegeContentText + "满" + $(privilegeStartEle[i]).val() + "送[" + $(privilegeEndEle[i]).find("option:selected").text() + "]优惠券;";
            }
        }

        ruleController.FormData.PrivilegeContent = privilegeContent;
        ruleController.FormData.PrivilegeContentText = PrivilegeContentText;
        ruleController.$apply();
    }

    return balanceRuleService;
}(ckFramework.BalanceRuleService || {}));

ckFramework.RedPacketRuleService = (function (redPacketRuleService) {
    var _ResourceConfig;

    // -------------------------------Public Methods and Properties ---------------------

    redPacketRuleService.Init = function (redPacketRuleConfig) {
        var initConfig =
        {
            redPacketType: "101", // 优惠类型
            selectedCommunityCodes: "",
            couponDatas:[],
            divRedPacketRuleAddController: "divRedPacketRuleAddController"
        };

        _ResourceConfig = $.extend(true, {}, initConfig, redPacketRuleConfig);
        
        $('.CouponNameSelect').val("");
        var ruleController = angular.element(document.getElementById(_ResourceConfig.divRedPacketRuleAddController)).scope();
        if (_ResourceConfig.redPacketType == "301") {
            $("form input[name=RandomStartLimit]").attr("disabled", true);
            $("form input[name=RandomEndLimit]").attr("disabled", true);
            $('.CouponNameSelect').removeAttr("disabled");

            $("form input[name=RedPacketCount]").attr("disabled", true);
            $("form input[name=MaxMoney]").attr("disabled", true);
            ruleController.FormData.RedPacketCount = 1;
            ruleController.FormData.MaxMoney = 0;
        }
        else {
            $("form input[name=RandomStartLimit]").attr("disabled", false);
            $("form input[name=RandomEndLimit]").attr("disabled", false);
            $('.CouponNameSelect').attr("disabled", "disabled");

            $("form input[name=RedPacketCount]").attr("disabled", false);
            $("form input[name=MaxMoney]").attr("disabled", false);
        }

        ruleController.$apply();
    }

    // 优惠类型改变时调用的方法
    redPacketRuleService.SetPrivilegeTypeChangedRule = function (redPacketType, selectedCommunityCodes) {
        var redPacketRuleConfig = {};
        redPacketRuleConfig.redPacketType = redPacketType;
        redPacketRuleConfig.selectedCommunityCodes = selectedCommunityCodes;
        redPacketRuleService.Init(redPacketRuleConfig);
        if (redPacketType == "301") {
            SetRedPacketRuleCoupon(selectedCommunityCodes);
        }
        SetRedPacketRuleAD(selectedCommunityCodes);
    }

    var SetRedPacketRuleCoupon = function (selectedCommunityCodes) {
        $('.CouponNameSelect').attr("disabled", "disabled");
        $.post("BalanceRedPacketRule/GetCouponByCodes", { valligeCodes: selectedCommunityCodes }, function (result) {
            _ResourceConfig.couponDatas = result;
            AddCouponRule();
            $('.CouponNameSelect').attr("disabled", false);
        });
    }

    // 赠送优惠券
    var AddCouponRule = function () {
        var couponRuleSelectDiv = $('.CouponNameSelect');
        couponRuleSelectDiv.children().remove();
        $('<option value=""></option>').appendTo(couponRuleSelectDiv);
        for (var i = 0; i < _ResourceConfig.couponDatas.length; i++) {
            $('<option value="' + _ResourceConfig.couponDatas[i].ID + '">' + _ResourceConfig.couponDatas[i].Name + '</option>').appendTo(couponRuleSelectDiv);
        }
    }

    var SetRedPacketRuleAD = function (selectedCommunityCodes) {
        $('.ADNameSelect').attr("disabled", "disabled");
        $.post("BalanceRedPacketRule/GetADByCodes", { valligeCodes: selectedCommunityCodes }, function (result) {
            _ResourceConfig.ADDatas = result;
            AddADs();
            $('.ADNameSelect').attr("disabled", false);
        });
    }

    // 广告
    var AddADs = function () {
        var ADNameSelectDiv = $('.ADNameSelect');
        ADNameSelectDiv.children().remove();
        $('<option value=""></option>').appendTo(ADNameSelectDiv);
        for (var i = 0; i < _ResourceConfig.ADDatas.length; i++) {
            $('<option value="' + _ResourceConfig.ADDatas[i].ID + '">' + _ResourceConfig.ADDatas[i].Name + '</option>').appendTo(ADNameSelectDiv);
        }
    }

    return redPacketRuleService;
}(ckFramework.RedPacketRuleService || {}));

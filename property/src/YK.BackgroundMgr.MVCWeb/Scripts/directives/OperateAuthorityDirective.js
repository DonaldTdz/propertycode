var ckFramework = ckFramework || {};
ckFramework.OperateAuthorityDirective = function () {
    return {
        scope: {
            authority: '@',
            authortyType:'@',
        },
        link: function (scope, element, attrs) {
            if (ckFramework.OperateAuthorityService.CheckAuthority(scope.authority))
            {
                return;
            }
            if (scope.authortyType == 'hide') {
                element.remove();
                //element.hide();
            }
            else {
                element.attr('disabled', 'disabled');
                //element.prop("disabled", true);
            }
        }
    };
}

ckFramework.OperateAuthorityService = (function (operateAuthorityService) {
    operateAuthorityService.CheckAuthority = function (authority)
    {
        var userOperates = authority.split(';');
        for (var i = 0; i < userOperates.length; i++) {
            var tempUserOperate = userOperates[i];
            for (var j = 0; j < ckFramework.HomeData.OperateCodeAndRoleInfos.length; j++) {
                var tempCodeAndRole = ckFramework.HomeData.OperateCodeAndRoleInfos[j];
                if (tempCodeAndRole.Code == tempUserOperate) {
                    return true;
                }
            }
        }

        return false;
    }

    return operateAuthorityService;
}(ckFramework.OperateAuthorityService || {}));
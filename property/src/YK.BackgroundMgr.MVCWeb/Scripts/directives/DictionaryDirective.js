var ckFramework = ckFramework || {};
ckFramework.DictionaryDirective = function () {
    return {
        restrict: 'EA',
        replace: true,
        scope: {
            dictSource: '@',
            dictData: '@',//add by donald 2016-9-8 直接传入数据
            dictFieldName: '@',
            dictName: '@',
            dictLabel: '@',
        },
        template: '<div style="width:100%">\
                <label data-ng-show="dictLabel!=\'\'">{{dictLabel}}</label>\
                <select data-ng-options="l.CnName for l in DictSources track by l.Code" data-ng-model="selectedDictItem" name="{{dictName}}" class="form-control"><option value="">--请选择--</option></select>\
                </div>',
        link: function (scope, element, attrs) {
            scope.selectedDictItem = "";
            var bindDatas = eval(scope.dictSource);
            //alert(JSON.stringify(bindDatas))
            //alert(JSON.stringify(eval(scope.dictData)))
            if (bindDatas == undefined) {
                scope.DictSources = eval(scope.dictData);
            }
            else {
                for (var i = 0; i < bindDatas.length; i++) {
                    if (bindDatas[i].EnName == scope.dictFieldName) {
                        scope.DictSources = eval(scope.dictSource)[i].DictionaryModels;
                        //alert(JSON.stringify(scope.DictSources))
                    }
                }
            }
        }
    };
}
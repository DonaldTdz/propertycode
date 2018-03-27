var ckFramework = ckFramework || {};
ckFramework.Months = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12];
ckFramework.Years = function () {
    var years = [];
    var currentYear = (new Date()).getFullYear();
    var tempYear = ckFramework.HomeData.LogStartYear
    while (tempYear <= currentYear) {
        years.push(tempYear);
        tempYear = tempYear + 1;
    }

    return years;
}
ckFramework.LogYearMonthDirective = function () {
    return {
        restrict: 'EA',
        replace: true,
        transclude: true,
        scope: {
            addFrom: '@',
            logFromSource: '@',
            showYearMonthLog: '@',
        },
        template: '<div ng-show="ShowLog" class="row"><div class="col-md-4">\
                    <label>年</label>\
                    <select data-ng-options="y for y in Years track by y" data-ng-model="selectedYear" name="Year" class="form-control ckFormControl"></select>\
                </div>\
                <div class="col-md-4">\
                    <label>月</label>\
                    <select data-ng-options="m for m in Months track by m" data-ng-model="selectedMonth" name="Month" class="form-control ckFormControl"></select>\
                </div>\
                <div ng-show="ShowLogFrom" class="col-md-4">\
                <label>日志来源</label>\
                <select data-ng-options="l.CnName for l in LogFromSources track by l.Code" data-ng-model="selectedLogFrom" name="LogFrom" class="form-control ckFormControl"><option value="">--请选择--</option></select>\
            </div></div>',
        link: function (scope, element, attrs) {
            scope.Months = ckFramework.Months;
            scope.Years = ckFramework.Years();
            scope.selectedMonth = (new Date()).getMonth() + 1;
            scope.selectedYear = (new Date()).getFullYear();
            scope.ShowLogFrom = false;
            scope.selectedLogFrom = "";
            if (scope.showYearMonthLog && scope.showYearMonthLog == "false")
            {
                scope.ShowLog = false;
            }
            else {
                scope.ShowLog = true;
            }

            if (scope.addFrom == "true") {
                var bindDatas = eval(scope.logFromSource);
                for (var i = 0; i < bindDatas.length; i++) {
                    if (bindDatas[i].EnName == "LogFrom") {
                        scope.LogFromSources = eval(scope.logFromSource)[i].DictionaryModels;
                        scope.ShowLogFrom = true;
                    }
                }
            }
            else {
                $(element).append($("#divOtherQuery"));
            }
        }
    };
}
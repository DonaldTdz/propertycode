var ckFramework = ckFramework || {};
ckFramework.DatetimePickerDirective = function (dateFilter) {
    return {
        require: 'ngModel',
        scope: {
            recipient: '=',

        },
        link: function (scope, element, attrs, ctrl) {
            element.bind('blur keyup change', function (element, attrs) {
                scope.recipient = $(element.target).val();
            });

            var dateFormat = attrs['timeformat'] || 'yyyy-MM-dd';

            ctrl.$formatters.unshift(function (modelValue) {
                return dateFilter(new Date(modelValue), dateFormat);
            });
        },
    };
}
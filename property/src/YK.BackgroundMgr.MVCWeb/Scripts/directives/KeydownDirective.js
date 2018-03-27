var ckFramework = ckFramework || {};
ckFramework.KeydownDirective = function () {
    return {
        restrict: 'A',
        link: function (scope, elem, attrs) {
            elem.on('keydown', function (e) {
                if (e.which == 13) {
                    scope.$eval(attrs.keydownDirective);
                }
            });
        }
    };
}
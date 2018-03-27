require.config({
    baseUrl: 'Scripts',
    urlArgs: 'v=1.0',
});

require(
    [
        'services/HomeService',
        'configs/PageInterceptor',
        'configs/routeResolver',
        'controllers/HomeController',
        'directives/ModalDirective',
        'directives/KeydownDirective',
        'directives/ConvertToNumberDirective',
        'directives/DatetimePickerDirective',
        'apps/HomeApp'
    ],
    function () {
        angular.bootstrap(document, ['HomeApp']);
    });

require.config({
    baseUrl: 'Scripts',
    urlArgs: 'v=1.0'
});

require(
    [
        'widgets/Modal/js/bootstrap-modal',
        'services/LoginService',
        'controllers/LoginController',
        'directives/ModalDirective',
        'directives/KeydownDirective',
        'apps/LoginApp'
    ],
    function () {
        angular.bootstrap(document, ['LoginApp']);
    });

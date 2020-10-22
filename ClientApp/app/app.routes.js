(function () {

    angular.module('EmediCodesApplication')
    .config(Configuration);

    Configuration.$inject = ['$stateProvider', '$locationProvider', '$urlRouterProvider', 'angularAuth0Provider', 'stateHelperProvider'];

    function Configuration($stateProvider, $locationProvider, $urlRouterProvider, angularAuth0Provider, stateHelperProvider) {
        angularAuth0Provider.init({
            clientID: AUTH0_CLIENT_ID,
            domain: AUTH0_DOMAIN,
            responseType: 'token id_token',
            audience: 'https://' + AUTH0_DOMAIN + '/userinfo',
            redirectUri: AUTH0_CALLBACK_URL,
            scope: 'openid profile'
        });

        
        stateHelperProvider.state({
            name: 'login',
            url: '/login',
            templateUrl: 'ClientApp/app/login/login.component.html',
        }).state({
            name: 'callback',
            url: '/callback',
            templateUrl: 'ClientApp/app/callback/callback.component.html'
        }).state({
            name: 'register',
            url: '/register',
            templateUrl: 'ClientApp/app/register/register.component.html'
        }).state({
            name: 'payment',
            url: '/payment',
            templateUrl: 'ClientApp/app/payment/payment.component.html'
        }).state({
            name: 'app',
            url: '/app',
            templateUrl: 'ClientApp/app/app.component.html',
            children: [
                {
                    name: 'case-list',
                    url: '/case-list',
                    templateUrl: 'ClientApp/app/case/case-list.component.html'
                },
                {
                    name: 'disease-site',
                    url: '/disease-site',
                    templateUrl: 'ClientApp/app/disease-site/disease-site.component.html'
                },
                {
                    name: 'cpt-list',
                    url: '/cpt-list',
                    templateUrl: 'ClientApp/app/cpt/cpt-list.component.html'
                },
                {
                    name: 'icd-10',
                    url: '/icd-10',
                    templateUrl: 'ClientApp/app/icd10/icd-10.component.html'
                },
                {
                    name: 'cpt',
                    url: '/cpt',
                    templateUrl: 'ClientApp/app/cpt/cpt.component.html'
                },
                {
                    name: 'create-case',
                    url: '/create-case',
                    templateUrl: 'ClientApp/app/case/create-case.component.html'
                },
                {
                    name: 'case',
                    url: '/case',
                    templateUrl: 'ClientApp/app/case/case.component.html'
                },
                {
                    name: 'modality',
                    url: '/modality',
                    templateUrl: 'ClientApp/app/modality/modality.component.html'
                },
                {
                    name: 'locale',
                    url: '/locale',
                    templateUrl: 'ClientApp/app/locale/locale.component.html'
                },
                {
                    name: 'treatment-summary',
                    url: '/treatment-summary',
                    templateUrl: 'ClientApp/app/treatment-summary/treatment-summary.component.html'
                },
                {
                    name: 'profile',
                    url: '/profile',
                    templateUrl: 'ClientApp/app/profile/profile.component.html'
                },
                {
                    name: 'bugs',
                    url: '/bugs',
                    templateUrl: 'ClientApp/app/bug-reporting/bug-reporting.component.html'
                },
                {
                    name: 'invalid-subscription',
                    url: '/invalid-subscription',
                    templateUrl: 'ClientApp/app/invalid-subscription/invalid-subscription.component.html'
                }
            ]
        });

        $urlRouterProvider.otherwise('/login');

    }
       

})();
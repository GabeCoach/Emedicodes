(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .factory('LoginService', LoginService);
    
        LoginService.$inject = ['angularAuth0'];
    
        function LoginService (angularAuth0) {
            function login() {
                angularAuth0.authorize();
            }
        
            return {
                login: login,
            };
        }
    
    })();
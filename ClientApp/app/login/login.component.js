(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .controller('LoginCtrl', LoginController);
    
        LoginController.$inject = ['$scope', '$state', 'LoginService'];
    
        function LoginController ($scope, $state, LoginService) {
            $scope.goToRegistration = function () {
                $state.go('register');
            };
        
            $scope.AuthenticateUser = function () {
                LoginService.login();
            };
        }
    
    })();
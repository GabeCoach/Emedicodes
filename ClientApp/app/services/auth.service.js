(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .factory('authService', authService);
    
        authService.$inject = ['$state', 'angularAuth0', 'EMIService', '$timeout', '$rootScope', '$http', 'toaster'];
    
        function authService($state, angularAuth0, EMIService, $timeout, $rootScope, $http, toaster) {
            var BaseUrl = EMIService.BaseUrl;
        
            /**
             * @name  ValidateAuthorization
             * @function ValidateAuthorization
             * @description Validate if user requesting to access a component resource is
                            authenticated and authorized, and redirect accordingly
             * @param {any} Headers
             */
            function ValidateAuthorization(Headers) {
        
                $http({
                    method: 'GET',
                    url: BaseUrl + 'api/Validator',
                    headers: Headers
                }).then(function (response) {
                    EMIService.isAuthorized = true;
                }).catch(function (status, headers, config, error) {
                    EMIService.isAuthorized = false;
                    handleUnauthorized();
                });
        
            }
        
            function handleUnauthorized() {
                toaster.error('Your session has expired, you will be redirected to the login.');
                $timeout(3000);
                $state.go('login');
            }
        
            /**
             * @name handleAuthentication
             * @function handleAuthentication
             * @description Retrieve auth tokens and set proper local storage variables
             */
            function handleAuthentication() {
                angularAuth0.parseHash(function (err, authResult) {
                    if (authResult && authResult.accessToken && authResult.idToken) {
                        setSession(authResult);
                        _AuthToken = localStorage.getItem('id_token');
                        $rootScope.$broadcast('AuthTokenSuccess');
                    } else if (err) {
                        $timeout(function () {
        
                        });
                        console.log(err);
                        alert('Error: ' + err.error + '. Check the console for further details.');
                    }
                });
            }
        
            function getProfile(cb) {
                var accessToken = localStorage.getItem('access_token');
                if (!accessToken) {
                    throw new Error('Access token must exist to fetch profile');
                }
                angularAuth0.client.userInfo(accessToken, function (err, profile) {
                    if (profile) {
                        EMIService.Auth0ID = profile.sub;
                        $rootScope.$broadcast('Auth0IDSet');
                        return profile.sub;
                    }
                    
                });
            }
        
            function setUserProfile(profile) {
               userProfile = profile;
            }
        
            /**
             * @name logout
             * @function logout
             * @description clear localStorage variables and redirect user to the login page.
             */
            function logout() {
                // Remove tokens and expiry time from localStorage
                localStorage.removeItem('access_token');
                localStorage.removeItem('id_token');
                localStorage.removeItem('expires_at');
                localStorage.removeItem('currentCPTCode');
                localStorage.removeItem('UserID');
                $state.go('login');
            }
        
            function isAuthenticated() {
                // Check whether the current time is past the 
                // access token's expiry time
                var sToken = 'Bearer ' + localStorage.getItem('id_token');
        
                var oHeaders = {
                    'Authorization': sToken
                };
        
                ValidateAuthorization(oHeaders);
        
                var expiresAt = JSON.parse(localStorage.getItem('expires_at'));
                return new Date().getTime() < expiresAt;
            }
        
            return {
                handleAuthentication: handleAuthentication,
                logout: logout,
                isAuthenticated: isAuthenticated,
                ValidateAuthorization: ValidateAuthorization,
                getProfile: getProfile,
                handleUnauthorized: handleUnauthorized
            };
        }
    
    })();
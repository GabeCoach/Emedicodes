(function(){

    angular.module('EmediCodesApplication')
    .factory('callBackService', CallBackService);

    CallBackService.$inject = ['angularAuth0', '$state'];

    function CallBackService(angularAuth0, $state) {
        function handleAuthentication() {
            angularAuth0.parseHash(function (err, authResult) {
                if (authResult && authResult.accessToken && authResult.idToken) {
                    setSession(authResult);
                    $state.go('app.case-list');
                } else if (err) {   
                    console.log(err);
                    alert('Error: ' + err.error + '. Check the console for further details.');
                }
            });
        }
    
        function setSession(authResult) {
            // Set the time that the access token will expire at
            var expiresAt = JSON.stringify((authResult.expiresIn) + new Date().getTime());
            localStorage.setItem('access_token', authResult.accessToken);
            localStorage.setItem('id_token', authResult.idToken);
            localStorage.setItem('expires_at', expiresAt);
        }
    
        return {
            handleAuthentication: handleAuthentication
        }
    }

})()
(function () {
    
        'use strict';
    
        angular
            .module('EmediCodesApplication')
            .run(run);
    
        run.$inject = ['callBackService'];
    
        function run(callBackService) {
            // Handle the authentication
            // result in the hash
            callBackService.handleAuthentication();
        }
    
    })();
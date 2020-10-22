(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .controller('DiseaseSiteCtrl', DiseaseSiteController);
    
        DiseaseSiteController.$inject = ['$scope', '$http', '$state', 'EMIService', 'authService', 'toaster'];
    
        function DiseaseSiteController ($scope, $http, $state, EMIService, authService, toaster) {
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
        
            authService.ValidateAuthorization(defaultRequestHeaders.headers);
            var UserId = localStorage.getItem('UserID');
            //var isSubscriptionValid = localStorage.getItem('ValidSubscription');
    
            
    
            EMIService.GetDiseaseSites(defaultRequestHeaders)
            .then(function(response){
                EMIService.serviceDiseaseSites = response.data;
                $scope.DiseaseSites = EMIService.serviceDiseaseSites;
            }).catch(function(error){
        
            });
        
            $scope.chooseDiseaseSite = function() {
                EMIService.serviceChosenDiseaseSite = $scope.DiseaseSite;
                $state.go('app.modality');
            };
        }
    })();
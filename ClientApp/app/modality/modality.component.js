(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .controller('ModalityCtrl', ModalityController);
    
        ModalityController.$inject = ['$scope', '$state', 'authService', 'EMIService', 'toaster'];
    
        function ModalityController ($scope, $state, authService, EMIService, toaster) {
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
        
            authService.ValidateAuthorization(defaultRequestHeaders.headers);

            var chosenDiseaseSite = EMIService.serviceChosenDiseaseSite;

            EMIService.GetApplicableModalities(chosenDiseaseSite, defaultRequestHeaders)
            .then(function(response){
                EMIService.serviceApplicableModalities = response;
                $scope.ApplicableModalities = EMIService.serviceApplicableModalities.data;
            });
        
            $scope.chooseModality = function(){
                EMIService.serviceChosenModalityID = $scope.Modality;
                $state.go('app.locale');
            };
        }
    
    })();
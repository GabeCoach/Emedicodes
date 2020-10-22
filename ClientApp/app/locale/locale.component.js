(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .controller('LocaleCtrl', LocaleController);
    
        LocaleController.$inject = ['$scope', '$state', 'EMIService', 'toaster', 'authService'];
    
        function LocaleController ($scope, $state, EMIService, toaster, authService) {
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
            
            authService.ValidateAuthorization(defaultRequestHeaders.headers);
            
            $scope.chooseSetting = function(){
                EMIService.serviceChosenTreatmentSetting = $scope.TreatmentSetting;
                $state.go('app.treatment-summary');
            };
        }
    
    })();
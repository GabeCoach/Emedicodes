(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .controller('TreatmentSummaryCtrl', TreatmentCodesSummaryController);
    
        TreatmentCodesSummaryController.$inject = ['$scope', 'EMIService', 'toaster', 'authService'];
    
        function TreatmentCodesSummaryController ($scope, EMIService, toaster, authService) {
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
        
            authService.ValidateAuthorization(defaultRequestHeaders.headers);

            var TreatmentSetting = EMIService.serviceChosenTreatmentSetting;
            var ModalityID = EMIService.serviceChosenModalityID;
              
            if(TreatmentSetting != undefined && TreatmentSetting != null && ModalityID != undefined && ModalityID != null)   
                CalculateTreatments(TreatmentSetting, ModalityID);
        

            $scope.TreatmentSetting = EMIService.serviceChosenTreatmentSetting;
        
            $scope.toggleTreatmentAmount = function(index, action){
                var currentRow = $scope.TreatmentCodes[index];
                if(action === "plus"){
                    currentRow.number_of_treatments++;
                    currentRow.treatment_dollar_amount = currentRow.opps_payment_rate * currentRow.number_of_treatments;
                    $scope.TreatmentCodes[index] = currentRow;
                }
                else if(action === "minus"){
                    currentRow.treatment_dollar_amount = currentRow.treatment_dollar_amount - currentRow.opps_payment_rate;
                    currentRow.number_of_treatments--;
                    $scope.TreatmentCodes[index] = currentRow;
                }
            };

            function CalculateTreatments(TreatmentSetting, ModalityID) {
                var apiResource = "";
    
                if(TreatmentSetting === undefined || 
                    TreatmentSetting === null || 
                    TreatmentSetting === "" ) {
                        toaster.error('Please select a treatment setting to continue.');
                    }
                    else if(TreatmentSetting.length === 0){
                        toaster.error('Please select a treatment setting to continue.');
                    }
                    else {
                        if(TreatmentSetting == "Hospital"){
                            apiResource = "HospitalTreatmentCodes";
                            EMIService.GetTreatmentCodesByModality(ModalityID, apiResource, defaultRequestHeaders)
                            .then(function(response){
                                EMIService.serviceTreatmentCodes = response;
                                $scope.TreatmentCodes = EMIService.serviceTreatmentCodes.data;
                            });
                        }
                        else if(TreatmentSetting == "Global"){
                            toaster.error('Feature Not Implemented');
                        }
                        else if(TreatmentSetting == "Professional"){
                            toaster.error('Feature Not Implemented');
                        }
                        else if(TreatmentSetting == "Technical"){
                            toaster.error('Feature Not Implemented');
                        }
                    }
            }
        }
    })();
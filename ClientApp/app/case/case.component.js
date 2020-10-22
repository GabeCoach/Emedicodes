(function () {
    
        angular.module('EmediCodesApplication')
            .controller('CaseCtrl', CaseController);
    
        CaseController.$inject = ['$scope', 'authService', 'EMIService', '$state', 'toaster'];
    
        function CaseController($scope, authService, EMIService, $state, toaster) {
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
    
            authService.ValidateAuthorization(defaultRequestHeaders.headers);
    
            var CaseID = EMIService.SelectedCaseID;
    
            EMIService.GetGPCI(defaultRequestHeaders)
                .success(function (response) {
                    EMIService.serviceGPCI = response;
                    $scope.GPCILocality = EMIService.serviceGPCI;
                }).catch(function (error) {
                    toaster.error(error);
                });
    
            $scope.CaseSummaryRequest = {
                treatment_setting: "Hospital",
                locality: "National",
                scope: "National"
            };
    
            var data = JSON.stringify($scope.CaseSummaryRequest);
    
            $scope.treatment_cost = 0;
    
            EMIService.GetCaseCodeSummary(CaseID, data, defaultRequestHeaders)
                .then(function (response) {
                    EMIService.serviceCaseCodeSummary = response;
                    $scope.CaseSummary = EMIService.serviceCaseCodeSummary.data;
                    $scope.TotalTreatmentCost = CalculateTotalTreatmentCost($scope.CaseSummary);
                }).catch(function (error) {
                    toaster.error(error);
                });
    
            $scope.toggleTreatmentAmount = function (index, action) {
                var currentRow = $scope.CaseSummary[index];
                if (action === "plus") {
                    currentRow.number_of_treatments++;
                    currentRow.treatment_cost = currentRow.payment_rate * currentRow.number_of_treatments;
                    $scope.CaseSummary[index] = currentRow;
                    $scope.TotalTreatmentCost = CalculateTotalTreatmentCost($scope.CaseSummary);
                }
                else if (action === "minus") {
                    currentRow.treatment_cost = currentRow.treatment_cost - currentRow.payment_rate;
                    currentRow.number_of_treatments--;
                    $scope.CaseSummary[index] = currentRow;
                    $scope.TotalTreatmentCost = CalculateTotalTreatmentCost($scope.CaseSummary);
                }
            };
    
            function CalculateTotalTreatmentCost(CaseCodesData){
                var totalTreatmentCost = 0;
    
                for(var x=0; x<CaseCodesData.length; x++){
                    totalTreatmentCost = totalTreatmentCost + CaseCodesData[x].treatment_cost;
                }
    
                return totalTreatmentCost;
            }
    
            $scope.onSettingsChange = function(){
                $scope.CaseSummaryRequest.treatment_setting = $scope.TreatmentSetting;
    
                var data = JSON.stringify($scope.CaseSummaryRequest);
    
                EMIService.GetCaseCodeSummary(CaseID, data, defaultRequestHeaders)
                .then(function (response) {
                    EMIService.serviceCaseCodeSummary = response;
                    $scope.CaseSummary = EMIService.serviceCaseCodeSummary.data;
                }).catch(function (error) {
                    toaster.error(error);
                });
            };
    
            $scope.onLocaleChange = function () {
                if ($scope.GPCI === "NATIONAL") {
                    $scope.CaseSummaryRequest.locality = "National";
                    $scope.CaseSummaryRequest.scope = "National";
    
                    var data = JSON.stringify($scope.CaseSummaryRequest);
    
                    EMIService.GetCaseCodeSummary(CaseID, data, defaultRequestHeaders)
                        .then(function (response) {
                            EMIService.serviceCaseCodeSummary = response;
                            $scope.CaseSummary = EMIService.serviceCaseCodeSummary.data;
                        }).catch(function (error) {
                            toaster.error(error);
                        });
                }
                else {
                    if($scope.CaseSummaryRequest.treatment_setting === "Hospital"){
                        toaster.error('Please choose a different setting to change the location');
                    }
                    else{
                        $scope.CaseSummaryRequest.locality = $scope.GPCI;
                        $scope.CaseSummaryRequest.scope = "Regional";
        
                        var sData = JSON.stringify($scope.CaseSummaryRequest);
        
                        EMIService.GetCaseCodeSummary(CaseID, sData, defaultRequestHeaders)
                            .then(function (response) {
                                EMIService.serviceCaseCodeSummary = response;
                                $scope.CaseSummary = EMIService.serviceCaseCodeSummary.data;
                            }).catch(function (error) {
                                toaster.error(error);
                            });
                    }
                }
            };
        }
    
    })();
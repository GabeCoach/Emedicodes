(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .controller('CPTCtrl', CPTController);
    
        CPTController.$inject = ['$scope', '$state', '$http', 'EMIService', 'authService', 'toaster'];
    
        function CPTController ($scope, $state, $http, EMIService, authService, toaster) {
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
        
            authService.ValidateAuthorization(defaultRequestHeaders.headers);
    
            var UserId = localStorage.getItem('UserID');
    
            EMIService.CheckUserSubscription(UserId, defaultRequestHeaders)
            .then(function(response){
                localStorage.setItem('ValidSubscription', response.ValidSubscription);
                if(!response.data.ValidSubscription)
                    $state.go('InvalidSubscription');
            });
        
            $scope.hideWageIndexTable = true;
            $scope.hideOppsData = false;
            $scope.hideWageIndexCalculations = true;
            $scope.WageIndexBtnText = "Calculate Wage Index Adjusted Payment";
        
            var selectedCPTCode;
        
            if(EMIService.SelectedCPTCode === null || EMIService.SelectedCPTCode === undefined){
                selectedCPTCode = localStorage.getItem('currentCPTCode');
            }
            else{
                selectedCPTCode = EMIService.SelectedCPTCode;
            }
        
            var Locality = "";
            $scope.PaymentScope = "National";
                  
            EMIService.GetWageIndex(defaultRequestHeaders)
            .then(function(response){
                EMIService.serviceWageIndex = response;
                $scope.WageIndexGridOptions.data = EMIService.serviceWageIndex.data;
            });
        
            $scope.ShowWageIndexTable = function(){
                
                $scope.hideWageIndexTable = false;
                $scope.hideOppsData = true;
                $scope.WageIndexBtnText = "Perform Adjusted Payment Calculation";
                $scope.hideWageIndexCalculations = true;
        
                $scope.WageIndexGridApi.core.refresh();
            };
        
            $scope.PerformWageIndexCalculation = function() {
                if ($scope.items != undefined && $scope.items.length != 0) {
                    var WageIndexItems = $scope.items[0];
        
                    var obj = {};
        
                    obj.geo_location = WageIndexItems.Area_Name;
                    obj.wage_index = WageIndexItems.Wage_Index;
                    obj.national_payment_rate = $scope.CurrentOPPS.Payment_Rate_.toString();
                    obj.state_code = WageIndexItems.State_Code;
                    obj.state = WageIndexItems.State;
        
                    var data = JSON.stringify(obj);
        
                    EMIService.PerformWageIndexCalculation(data, defaultRequestHeaders)
                    .then(function(response){
                        $scope.preliminaryAdjustmentAmount = response.data.preliminary_adjustment_amount;
                        $scope.wageIndexAdjustedAmount = response.data.wage_index_adjusted_payment;
                        $scope.hideWageIndexCalculations = false;
                        $scope.hideWageIndexTable = true;
                        $scope.hideOppsData = false;
                    });
                }
                else {
                    toaster.error('Error', 'Please select a wage index area.');
                }
            };
        
            $scope.onGPCILocaleChange = function () {
                Locality = $scope.GPCILocality;
                $scope.PaymentScope = Locality;
                var CurrentCPT = JSON.parse(localStorage.getItem("currentCPTCode"));
        
                if (Locality === "NATIONAL") {
                    var oRegionalPaymentRateRequest = {
                        CPTCode: selectedCPTCode,
                        Locale: Locality,
                        PaymentRateScope: "National"
                    };
                } else {
                     oRegionalPaymentRateRequest = {
                        CPTCode: selectedCPTCode,
                        Locale: Locality,
                        PaymentRateScope: "Regional"
                    };
                }
        
                var regionalPaymentRateData = JSON.stringify(oRegionalPaymentRateRequest);
        
                if (CurrentCPT.length === 1) {
                    if (CurrentCPT[0].Work_RVUs !== "0") {
        
                        EMIService.GetGlobalPaymentRate(regionalPaymentRateData, defaultRequestHeaders)
                            .success(function (response) {
                                EMIService.serviceGlobalPaymentRate = response.PaymentRate;
                                $scope.GlobalPaymentRate = EMIService.serviceGlobalPaymentRate;
                            }).error(function (status, config, error) {
                                switch (status) {
                                    case 401: {
                                        toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                        $timeout(3000);
                                        $window.location = LoginRedirectLocation;
                                        break;
                                    }
                                    case 500: {
                                        toaster.pop('error', 'Internal Server Error');
                                    }
                                }
                            });
        
                        EMIService.GetProfessionalPaymentRate(regionalPaymentRateData, defaultRequestHeaders)
                            .success(function (response) {
                                EMIService.serviceProfessionalPaymentRate = response.PaymentRate;
                                $scope.ProfessionalPaymentRate = EMIService.serviceProfessionalPaymentRate;
                            }).error(function (status, config, error) {
                                switch (status) {
                                    case 401: {
                                        toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                        $timeout(3000);
                                        $window.location = LoginRedirectLocation;
                                        break;
                                    }
                                    case 500: {
                                        toaster.pop('error', 'Internal Server Error');
                                    }
                                }
                            });
        
                    } else {
                        EMIService.GetGlobalPaymentRate(regionalPaymentRateData, defaultRequestHeaders)
                            .success(function (response) {
                                EMIService.serviceGlobalPaymentRate = response.PaymentRate;
                                $scope.GlobalPaymentRate = EMIService.serviceGlobalPaymentRate;
                            }).error(function (status, config, error) {
                                switch (status) {
                                    case 401: {
                                        toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                        $timeout(3000);
                                        $window.location = LoginRedirectLocation;
                                        break;
                                    }
                                    case 500: {
                                        toaster.pop('error', 'Internal Server Error');
                                    }
                                }
                            });
        
                        EMIService.GetTechnicalPaymentRate(regionalPaymentRateData, defaultRequestHeaders)
                            .success(function (response) {
                                EMIService.serviceTrchnicalPaymentRate = response.PaymentRate;
                                $scope.TechnicalPaymentRate = EMIService.serviceTrchnicalPaymentRate;
                            }).catch(function (error) {
                                switch (status) {
                                    case 401: {
                                        toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                        $timeout(3000);
                                        $window.location = LoginRedirectLocation;
                                        break;
                                    }
                                    case 500: {
                                        toaster.pop('error', 'Internal Server Error');
                                    }
                                }
                        });
        
                        
                    }
                } else {
                    EMIService.GetGlobalPaymentRate(regionalPaymentRateData, defaultRequestHeaders)
                        .success(function (response) {
                            EMIService.serviceGlobalPaymentRate = response.PaymentRate;
                            $scope.GlobalPaymentRate = EMIService.serviceGlobalPaymentRate;
                        }).error(function (status, config, error) {
                            switch (status) {
                                case 401: {
                                    toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                    $timeout(3000);
                                    $window.location = LoginRedirectLocation;
                                    break;
                                }
                                case 500: {
                                    toaster.pop('error', 'Internal Server Error');
                                }
                            }
                        });
        
                    EMIService.GetTechnicalPaymentRate(regionalPaymentRateData, defaultRequestHeaders)
                        .success(function (response) {
                            EMIService.serviceTrchnicalPaymentRate = response.PaymentRate;
                            $scope.TechnicalPaymentRate = EMIService.serviceTrchnicalPaymentRate;
                        }).catch(function (error) {
                            switch (status) {
                                case 401: {
                                    toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                    $timeout(3000);
                                    $window.location = LoginRedirectLocation;
                                    break;
                                }
                                case 500: {
                                    toaster.pop('error', 'Internal Server Error');
                                    break;
                                }
                            }
                        });
        
                    EMIService.GetProfessionalPaymentRate(regionalPaymentRateData, defaultRequestHeaders)
                    .success(function (response) {
                        EMIService.serviceProfessionalPaymentRate = response.PaymentRate;
                        $scope.ProfessionalPaymentRate = EMIService.serviceProfessionalPaymentRate;
                    }).error(function (status, config, error) {
                        switch (status) {
                            case 401: {
                                toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                $timeout(3000);
                                $window.location = LoginRedirectLocation;
                                break;
                            }
                            case 500: {
                                toaster.pop('error', 'Internal Server Error');
                            }
                        }
                    });
                }
            };
        
            EMIService.GetMPFSByCPT(selectedCPTCode, defaultRequestHeaders)
                .success(function (response) {
        
                    if (response.length === 1) {
                        localStorage.setItem('currentCPTCode', JSON.stringify(response));
                        $scope.CurrentCPT = response[0];
        
                        var oNationalPaymentRateRequest = {
                            CPTCode: selectedCPTCode,
                            Locale: Locality,
                            PaymentRateScope: "National"
                        };
                        var nationalPaymentRateData = JSON.stringify(oNationalPaymentRateRequest);
        
                        if ($scope.CurrentCPT.Work_RVUs !== "0" && $scope.CurrentCPT.Work_RVUs !== "NA") {
                            $scope.TechnicalRVU = "NA";
                            $scope.TechnicalPaymentRate = 0;
        
                            EMIService.GetGlobalRVU(selectedCPTCode, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceGlobalRVU = response.GlobalRVU;
                                    $scope.GlobalRVU = EMIService.serviceGlobalRVU;
                                }).catch(function (error) {
                                    alert(error);
                                });
        
                            EMIService.GetProfessionalRVU(selectedCPTCode, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceProfessionalRVU = response.ProfessionalRVU;
                                    $scope.ProfessionalRVU = response.ProfessionalRVU;
                                }).catch(function (error) {
                                    alert(error);
                                });
        
                            EMIService.GetGlobalPaymentRate(nationalPaymentRateData, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceGlobalPaymentRate = response.PaymentRate;
                                    $scope.GlobalPaymentRate = EMIService.serviceGlobalPaymentRate;
                                }).error(function (status, config, error) {
                                    switch (status) {
                                        case 401: {
                                            toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                            $timeout(3000);
                                            $window.location = LoginRedirectLocation;
                                            break;
                                        }
                                        case 500: {
                                            toaster.pop('error', 'Internal Server Error');
                                        }
                                    }
                                });
        
                            EMIService.GetProfessionalPaymentRate(nationalPaymentRateData, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceProfessionalPaymentRate = response.PaymentRate;
                                    $scope.ProfessionalPaymentRate = EMIService.serviceProfessionalPaymentRate;
                                }).error(function (status, config, error) {
                                    switch (status) {
                                        case 401: {
                                            toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                            $timeout(3000);
                                            $window.location = LoginRedirectLocation;
                                            break;
                                        }
                                        case 500: {
                                            toaster.pop('error', 'Internal Server Error');
                                        }
                                    }
                                });
                        }
                        else {
                            $scope.ProfessionalPaymentRate = 0;
                            $scope.ProfessionalRVU = "NA";
        
                            EMIService.GetGlobalRVU(selectedCPTCode, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceGlobalRVU = response.GlobalRVU;
                                    $scope.GlobalRVU = EMIService.serviceGlobalRVU;
                                }).catch(function (error) {
                                    alert(error);
                                });
        
                            EMIService.GetTechnicalRVU(selectedCPTCode, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceTechnicalRVU = response.TechnicalRVU;
                                    $scope.TechnicalRVU = EMIService.serviceTechnicalRVU;
                                }).catch(function (error) {
                                    alert(error);
                                });
        
                            EMIService.GetTechnicalPaymentRate(nationalPaymentRateData, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceTrchnicalPaymentRate = response.PaymentRate;
                                    $scope.TechnicalPaymentRate = EMIService.serviceTrchnicalPaymentRate;
                                }).catch(function (error) {
                                    switch (status) {
                                        case 401: {
                                            toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                            $timeout(3000);
                                            $window.location = LoginRedirectLocation;
                                            break;
                                        }
                                        case 500: {
                                            toaster.pop('error', 'Internal Server Error');
                                        }
                                    }
                                });
        
                            EMIService.GetGlobalPaymentRate(nationalPaymentRateData, defaultRequestHeaders)
                                .success(function (response) {
                                    EMIService.serviceGlobalPaymentRate = response.PaymentRate;
                                    $scope.GlobalPaymentRate = EMIService.serviceGlobalPaymentRate;
                                }).error(function (status, config, error) {
                                    switch (status) {
                                        case 401: {
                                            toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                            $timeout(3000);
                                            $window.location = LoginRedirectLocation;
                                            break;
                                        }
                                        case 500: {
                                            toaster.pop('error', 'Internal Server Error');
                                        }
                                    }
                                });
                        }
                    }
                    else {
                         oNationalPaymentRateRequest = {
                            CPTCode: selectedCPTCode,
                            Locale: Locality,
                            PaymentRateScope: "National"
                        };
        
                        localStorage.setItem('currentCPTCode', JSON.stringify(response));
        
                        nationalPaymentRateData = JSON.stringify(oNationalPaymentRateRequest);
        
                        $scope.CurrentCPT = response[0];
        
                        EMIService.GetGlobalRVU(selectedCPTCode, defaultRequestHeaders)
                            .success(function (response) {
                                EMIService.serviceGlobalRVU = response.GlobalRVU;
                                $scope.GlobalRVU = EMIService.serviceGlobalRVU;
                            }).catch(function (error) {
                                alert(error);
                            });
        
                       EMIService.GetProfessionalRVU(selectedCPTCode, defaultRequestHeaders)
                            .success(function (response) {
                                $scope.ProfessionalRVU = response.ProfessionalRVU;
                            }).catch(function (error) {
                                alert(error);
                            });
        
                       EMIService.GetTechnicalRVU(selectedCPTCode, defaultRequestHeaders)
                           .success(function (response) {
                               EMIService.serviceTechnicalRVU = response.TechnicalRVU;
                               $scope.TechnicalRVU = EMIService.serviceTechnicalRVU;
                           }).catch(function (error) {
                               alert(error);
                           });
        
                       EMIService.GetTechnicalPaymentRate(nationalPaymentRateData, defaultRequestHeaders)
                           .success(function (response) {
                               EMIService.serviceTrchnicalPaymentRate = response.PaymentRate;
                               $scope.TechnicalPaymentRate = EMIService.serviceTrchnicalPaymentRate;
                           }).catch(function (error) {
                               switch (status) {
                                   case 401: {
                                       toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                       $timeout(3000);
                                       $window.location = LoginRedirectLocation;
                                       break;
                                   }
                                   case 500: {
                                       toaster.pop('error', 'Internal Server Error');
                                       break;
                                   }
                               }
                           });
        
                       EMIService.GetGlobalPaymentRate(nationalPaymentRateData, defaultRequestHeaders)
                           .success(function (response) {
                               EMIService.serviceGlobalPaymentRate = response.PaymentRate;
                               $scope.GlobalPaymentRate = EMIService.serviceGlobalPaymentRate;
                           }).error(function (status, config, error) {
                               switch (status) {
                                   case 401: {
                                       toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                       $timeout(3000);
                                       $window.location = LoginRedirectLocation;
                                   }
                                   case 500: {
                                       toaster.pop('error', 'Internal Server Error');
                                   }
                               }
                           });
        
                       EMIService.GetProfessionalPaymentRate(nationalPaymentRateData, defaultRequestHeaders)
                           .success(function (response) {
                               EMIService.serviceProfessionalPaymentRate = response.PaymentRate;
                               $scope.ProfessionalPaymentRate = EMIService.serviceProfessionalPaymentRate;
                           }).error(function (status, config, error) {
                               switch (status) {
                                   case 401: {
                                       toaster.pop('error', 'Authentication session may be expired, redirecting to login.');
                                       $timeout(3000);
                                       $window.location = LoginRedirectLocation;
                                       break;
                                   }
                                   case 500: {
                                       toaster.pop('error', 'Internal Server Error');
                                   }
                               }
                           });
                    }
        
                }).catch(function (error) {
                    alert(error);
                });
        
            EMIService.GetGPCI(defaultRequestHeaders)
                .success(function (response) {
                    EMIService.serviceGPCI = response;
                    $scope.GPCILocale = EMIService.serviceGPCI;
                    Locality = EMIService.serviceGPCI.LocalName;
                }).catch(function (error) {
                    alert(error);
                });
        
           EMIService.GetOPPSByCPT(selectedCPTCode, defaultRequestHeaders)
               .success(function (response) {
                   EMIService.serviceOPPS = response;    
                   $scope.CurrentOPPS = EMIService.serviceOPPS;
                    if ($scope.CurrentOPPS.CH === "") {
                        $scope.CurrentOPPS.CH = "N/A";
                    }
                }).catch(function (error) {
                    alert(error);
                });
        
           EMIService.GetCCIByCPT(selectedCPTCode, defaultRequestHeaders)
                .success(function (response) {
                    EMIService.serviceCCI = response;
                    $scope.CCIEditOptions.data = EMIService.serviceCCI;
                });
        
        
           EMIService.GetMUEByCPT(selectedCPTCode, defaultRequestHeaders)
               .success(function (response) {
                    EMIService.serviceMUE = response;
                    $scope.MUEOptions.data = EMIService.serviceMUE;
                });
        
            $scope.CCIEditOptions = {
                multiSelect: false,
                columnDefs: [
                    { field: 'Column_1', displayName: 'Column One', width: 125 },
                    { field: 'Column_2', displayName: 'Column Two', width: 125 },
                    { field: 'Deletion', displayName: 'Deletion', width: 125 },
                    { field: 'Modifier', displayName: 'Modifier', width: 125 },
                    { field: 'PTP_Edit_Rationale', displayName: 'PTP Rationale', width: 125 }
                ],
        
            };
        
            $scope.MUEOptions = {
                multiSelect: false,
                columnDefs: [
                    { field: 'HCPCS_CPT_Code', displayName: ' CPT Code', width: 125 },
                    { field: 'Outpatient_Hospital_Services_MUE_Values', displayName: 'OutPatient MUE', width: 125 },
                    { field: 'MUE_Adjudication_Indicator', displayName: 'Adjudication Indicator', width: 125 },
                    { field: 'MUE_Rationale', displayName: 'Rationale', width: 125 },
                ],
            };
        
            $scope.WageIndexGridOptions = {
                multiSelect: false,
                enableFiltering: true,
                enableRowSelection: true,
                columnDefs: [
                    {field: 'Area_Name', displayName: 'Area Name', width: 225 },
                    {field: 'State', displayName: 'State', width: 225 },
                    {field: 'State_Code', displayName: 'State Code', width: 225 },
                    {field: 'Wage_Index', displayName: 'Wage Index', width: 225},
                ]
            };
        
            $scope.WageIndexGridOptions.onRegisterApi = function (gridApi) {
                //set gridApi on scope
                $scope.WageIndexGridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    $scope.items = $scope.WageIndexGridApi.selection.getSelectedRows();
                });
        
                gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
                    var msg = 'rows changed ' + rows.length;
                    //$log.log(msg);
                });
        
            };
        }
    
    })();
    
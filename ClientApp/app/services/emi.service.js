(function (){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .factory('EMIService', EMIService);
    
        EMIService.$inject = ['$http', 'toaster', '$window', '$timeout', '$rootScope'];
    
        function EMIService ($http, toaster, $window, $timeout, $rootScope) {
            var BaseUrl = 'https://www.app.emedicodes.com/';
        
            var _serviceMPFS = {};
            _RetrieveAllMPFS = {};
            _RetreivePagedMPFS = {};
            _RetreiveMPFSTotal = {};
            var _serviceMPFSTotal = 0;
        
            var _serviceGlobalPaymentRate = 0;
            _GetGlobalPaymentRate = {};
        
            var _serviceWageIndexAreas = {};
            _GetWageIndexAreas = {};
        
            var _serviceProfessionalPaymentRate = 0;
            _GetProfessionalPaymentRate = {};
        
            var _serviceTechnicalPaymentRate = 0;
            _GetTechnicalPaymentRate = {};
        
            var _serviceMPFSByCPT = {};
            _GetMPFSByCPT = {};
        
            var _serviceGlobalRVU = "";
            _GetGlobalRVU = {};
        
            var _serviceTechnicalRVU = "";
            _GetTechnicalRVU = {};
        
            var _serviceProfessionalRVU = "";
            _GetProfessionalRVU = {};
        
            var _serviceGPCI = {};
            _GetGPCI = {};
        
            var _serviceOPPS = {};
            _GetOPPSByCPT = {};
        
            var _serviceCCI = {};
            _GetCCIByCPT = {};
        
            var _serviceMUE = {};
            _GetMUEByCPT = {};
        
            var _serviceSelectedWageArea = {};
            _PerformWageIndexCalculation = {};
        
            var _seviceSearchedMPFSResults = {};
            _SearchMPFSByCPT = {};
        
            var _serviceDiseaseSites = {};
            _GetDiseaseSites = {};
        
            var _serviceChosenDiseaseSite = "";
            _GetApplicableModalities = {};
            var _serviceApplicableModalities = {};
        
            var _serviceChosenModalityID = 0;
        
            var _serviceTreatmentCodes = {};
            var _serviceChosenTreatmentSetting = "";
            var _serviceTreatmentCodes = {};
            _GetTreatmentCodesByModality = {};
        
            var _calculatorPrevBtnDisabled = true;
            var _calculatorNextBtnDisabled = true;
            var _calculatorCalcBtnDisabled = false;
        
            var _isAuthorized = false;
            var _Auth0ID = "";
    
            var _serviceUserProfile = {};
            _GetUserProfile = {};
    
            var _serviceICD10Data = {};
            _GetICD10Data ={};
    
            _GetICD10Count = {};
            _GetICD10Paged = {};
            var _serviceICD10PagedData = {};
    
            var _serviceSearchedICD10 = {};
            _SearchICD10 = {};
    
            var _SelectedCPTCaseCodes = [];
            var _SelectedCodesToRemove = [];
    
            var _CurrentUserID = 0;
    
            _SaveCaseToDB = {};
            _SaveCaseCodesToDB = {};
    
            var _serviceUserCaseList = {};
            _GetUserCaseList = {};
            var _UserID = 0;
    
            var _SelectedCaseID = 0;
            _GetCaseCodeSummary = {};
            var _serviceCaseCodeSummary = {};
    
            _CancelUserSubscription = {};
            _CheckUserSubscription = {};
            _CheckUserPaymentMethod = {};
            _CreateNewSubscription = {};
            _AddUserPaymentMethod = {};
            _GetWageIndex = {};
            var _serviceWageIndex = {};

            _CheckFreeTrialPeriod = {};
            _GetFreeTrialPeriodAmount = {};

            _DeleteCase = {};
            var _deleteSelectedCaseID = 0;

            _SubmitBugReport = {};

            var _SubmitBugReport = function(UserId, config, data){
                return $http.post(BaseUrl + 'api/BugReport/' + UserId, data, config)
            };

            var _GetFreeTrialPeriodAmount = function(UserId, config){
                return $http.get(BaseUrl + 'api/Payments/' + UserId + '/FreeTrialTime', config);
            };

            var _CheckFreeTrialPeriod = function (UserId, config) {
                return $http.get(BaseUrl + 'api/Payments/' + UserId + '/FreeTrialPeriod', config);
            };

            var _DeleteCase = function(CaseId, config){
                return $http.get(BaseUrl + 'api/Case/' + CaseId, config);
            };

            var _GetWageIndex = function(config) {
              return $http.get(BaseUrl + 'api/WageIndex', config);
            };

    
            var _AddUserPaymentMethod = function(data, config){
                return $http.post(BaseUrl + 'api/Payments/PaymentMethod', data, config);
            };
    
            var _CreateNewSubscription = function(UserId, config){
                return $http.get(BaseUrl + 'api/Payments/' + UserId + '/Subscribe');
            };
    
            var _CheckUserPaymentMethod = function(UserId, config){
                return $http.get(BaseUrl + 'api/Payments/' + UserId + '/PaymentMethod', config);
            };
    
            var _CheckUserSubscription = function(UserId, config){
                return $http.get(BaseUrl + 'api/Payments/' + UserId + '/CheckSubscription', config);
            };
    
            var _CancelUserSubscription = function(UserId, config){
                return $http.get(BaseUrl + 'api/Payments/' + UserId + '/CancelSubscription', config);
            };
    
            var _GetCaseCodeSummary = function(CaseID, data, config){
                return $http.post(BaseUrl + 'api/Case/' + CaseID + '/CaseSummary', data, config);
            };
    
            var _GetUserCaseList = function(UserID, config){
                return $http.get(BaseUrl + 'api/Case/' + UserID + '/CaseList', config);
            };
    
            var _SaveCaseCodesToDB = function(data, CaseID, config){
                return $http.post(BaseUrl + 'api/Case/CaseCodes/' + CaseID, data, config);
            };
    
            var _SaveCaseToDB = function(data, config){
                return $http.post(BaseUrl + 'api/Case', data, config);
            };
    
            var _SearchICD10 = function(data, config){
                return $http.get(BaseUrl + 'api/Diseases/' + data + '/Search', config);
            };
    
            var _GetICD10Paged = function(data, config){
                return $http.post(BaseUrl + 'api/Diseases/PageRecords', data, config);
            };
    
            var _GetICD10Count = function(config){
                return $http.get(BaseUrl + 'api/Diseases/Count', config);
            };
        
            var _GetICD10Data = function(config){
                return $http.get(BaseUrl + 'api/Diseases', config);
            };
    
            var _GetUserProfile = function(Auth0ID, config) {
                return $http.get(BaseUrl + 'api/Users/'+ Auth0ID + '/ProfileInfo', config);
            };
    
            var _RegisterUser = function(data){
                return $http.post(BaseUrl + 'api/Users', data);
            };
    
            var _GetTreatmentCodesByModality = function(ModalityID, apiResource, config){
                return $http.get(BaseUrl + 'api/TreatmentCodeCalculator/' + ModalityID + '/' + apiResource, config);
            };
        
            var _GetApplicableModalities = function(chosenDiseaseSite, config){
                return $http.get(BaseUrl + 'api/Modalities/'+ chosenDiseaseSite + '/DiseaseSiteApplicableModalities', config);
            };
        
            var _GetDiseaseSites = function(config){
                return $http.get(BaseUrl + 'api/DiseaseSites', config);
            };
        
            var _SearchMPFSByCPT = function(CptCodePartial, config){
                return $http.get(BaseUrl + 'api/MPFS/' + CptCodePartial + '/Search', config);
            };
        
            var _RetreiveMPFSTotal = function(config){
                return $http.get(BaseUrl + 'api/MPFS/Count', config);
            };
        
            var _RetreivePagedMPFS = function(data, config) {
                return $http.post(BaseUrl + 'api/MPFS/PageRecords', data, config);
            };
        
            var _PerformWageIndexCalculation = function(data, config) {
                return $http.post(BaseUrl + 'api/WageIndex/Calculate', data, config);
            };
        
            var _GetWageIndexAreas = function() {
                return $http.get(BaseUrl + 'api/WageIndex');
            };
        
            var _GetMUEByCPT = function (SelectedCPT, config) {
                return $http.get(BaseUrl + 'api/MUEInformation/' + SelectedCPT + '/CPT', config);
            };
        
            var _GetCCIByCPT = function (SelectedCPT, config) {
                return $http.get(BaseUrl + 'api/CCIEdits/' + SelectedCPT + '/CPT', config);
            };
        
            var _GetOPPSByCPT = function (SelectedCPT, config) {
                return $http.get(BaseUrl + 'api/OPPS/' + SelectedCPT + '/CPT', config);
            };
        
            var _GetGPCI = function (config) {
                return $http.get(BaseUrl + 'api/GPCI', config);
            };
        
            var _GetGlobalRVU = function (SelectedCPTCode, Config) {
                return $http.get(BaseUrl + 'api/MPFS/'+ SelectedCPTCode +'/GlobalRVUCalculation/', Config);
            };
        
            var _GetProfessionalRVU = function (SelectedCPTCode, Config) {
                return $http.get(BaseUrl + 'api/MPFS/' + SelectedCPTCode + '/ProfessionalRVUCalculation', Config);
            };
        
            var _GetTechnicalRVU = function (SelectedCPTCode, Config) {
                return $http.get(BaseUrl + 'api/MPFS/' + SelectedCPTCode + '/TechnicalRVUCalculation', Config);
            };
        
            var _GetGlobalPaymentRate = function (data, config) {
                return $http.post(BaseUrl + 'api/MPFS/GlobalPaymentRate', data, config);
            };
        
            var _GetProfessionalPaymentRate = function (data, config) {
                return $http.post(BaseUrl + 'api/MPFS/ProfessionalPaymentRate', data, config);
            };
        
            var _GetTechnicalPaymentRate = function (data, config) {
                return $http.post(BaseUrl + 'api/MPFS/TechnicalPaymentRate', data, config);
            };
        
            var _GetMPFSByCPT = function (SelectedCPTCode, Config) {
                return $http.get(BaseUrl + 'api/MPFS/'+ SelectedCPTCode +'/CPT', Config);
            };
        
            var _RetrieveAllMPFS = function (Config) {
                return $http.get(BaseUrl + 'api/MPFS', Config);
            };
            
            return {
                serviceMPFS: _serviceMPFS,
                RetrieveAllMPFS: _RetrieveAllMPFS,
                serviceGlobalPaymentRate: _serviceGlobalPaymentRate,
                GetGlobalPaymentRate: _GetGlobalPaymentRate,
                serviceProfessionalPayment: _serviceProfessionalPaymentRate,
                GetProfessionalPaymentRate: _GetProfessionalPaymentRate,
                serviceTechnicalPaymentRate: _serviceTechnicalPaymentRate,
                GetTechnicalPaymentRate: _GetTechnicalPaymentRate,
                serviceMPFSByCPT: _serviceMPFSByCPT,
                GetMPFSByCPT: _GetMPFSByCPT,
                GetGlobalRVU: _GetGlobalRVU,
                serviceGlobalRVU: _serviceGlobalRVU,
                GetProfessionalRVU: _GetProfessionalRVU,
                serviceProfessionalRVU: _serviceProfessionalRVU,
                GetTechnicalRVU: _GetTechnicalRVU,
                serviceTechnicalRVU: _serviceTechnicalRVU,
                serviceGPCI: _serviceGPCI,
                GetGPCI: _GetGPCI,
                serviceOPPS: _serviceOPPS,
                GetOPPSByCPT: _GetOPPSByCPT,
                serviceCCI: _serviceCCI,
                GetCCIByCPT: _GetCCIByCPT,
                serviceMUE: _serviceMUE,
                GetMUEByCPT: _GetMUEByCPT,
                serviceWageIndexAreas: _serviceWageIndexAreas,
                GetWageIndexAreas: _GetWageIndexAreas,
                serviceSelectedWageArea: _serviceSelectedWageArea,
                PerformWageIndexCalculation: _PerformWageIndexCalculation,
                RetreivePagedMPFS: _RetreivePagedMPFS,
                serviceMPFSTotal: _serviceMPFSTotal,
                RetreiveMPFSTotal: _RetreiveMPFSTotal,
                serviceSearchedMPFSResults: _seviceSearchedMPFSResults,
                SearchMPFSByCPT: _SearchMPFSByCPT,
                serviceDiseaseSites: _serviceDiseaseSites,
                GetDiseaseSites: _GetDiseaseSites,
                serviceChosenDiseaseSite: _serviceChosenDiseaseSite,
                serviceApplicableModalities: _serviceApplicableModalities,
                GetApplicableModalities: _GetApplicableModalities,
                serviceChosenModalityID: _serviceChosenModalityID,
                serviceTreatmentCodes: _serviceTreatmentCodes,
                serviceChosenTreatmentSetting: _serviceChosenTreatmentSetting,
                GetTreatmentCodesByModality: _GetTreatmentCodesByModality,
                calculatorPrevBtnDisabled: _calculatorPrevBtnDisabled,
                calculatorNextBtnDisabled: _calculatorNextBtnDisabled,
                calculatorCalcBtnDisabled: _calculatorCalcBtnDisabled,
                isAuthorized: _isAuthorized,
                RegisterUser: _RegisterUser,
                Auth0ID: _Auth0ID,
                serviceUserProfile: _serviceUserProfile,
                GetUserProfile: _GetUserProfile,
                serviceICD10Data: _serviceICD10Data,
                GetICD10Data: _GetICD10Data,
                GetICD10Count: _GetICD10Count,
                GetICD10Paged: _GetICD10Paged,
                serviceICD10PagedData: _serviceICD10PagedData,
                serviceSearchedICD10: _serviceSearchedICD10,
                SearchICD10: _SearchICD10,
                SelectedCPTCaseCodes: _SelectedCPTCaseCodes,
                SelectedCodesToRemove: _SelectedCodesToRemove,
                CurrentUserID: _CurrentUserID,
                SaveCaseToDB: _SaveCaseToDB,
                SaveCaseCodesToDB: _SaveCaseCodesToDB,
                serviceUserCaseList: _serviceUserCaseList,
                GetUserCaseList: _GetUserCaseList,
                UserID: _UserID,
                SelectedCaseID: _SelectedCaseID,
                GetCaseCodeSummary: _GetCaseCodeSummary,
                serviceCaseCodeSummary: _serviceCaseCodeSummary,
                CancelUserSubscription: _CancelUserSubscription,
                CheckUserSubscription: _CheckUserSubscription,
                CheckUserPaymentMethod: _CheckUserPaymentMethod,
                CreateNewSubscription: _CreateNewSubscription,
                AddUserPaymentMethod: _AddUserPaymentMethod,
                BaseUrl: BaseUrl,
                GetWageIndex: _GetWageIndex,
                serviceWageIndex: _serviceWageIndex,
                DeleteCase: _DeleteCase,
                deleteSelectedCaseID: _deleteSelectedCaseID,
                CheckFreeTrialPeriod: _CheckFreeTrialPeriod,
                GetFreeTrialPeriodAmount: _GetFreeTrialPeriodAmount,
                SubmitBugReport: _SubmitBugReport
            };
        }
    
    })();
    
(function(){
    
        angular.module('EmediCodesApplication')
        .controller('CaseFormCtrl', CaseFormController);
    
        CaseFormController.$inject = ['$scope', 'EMIService', 'authService', '$state', 'toaster'];
    
        function CaseFormController($scope, EMIService, authService, $state, toaster) {
            
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
    
            authService.ValidateAuthorization(defaultRequestHeaders.headers);
    
            $scope.Cancel = function(){
                $state.go('CaseList');
            };
    
            $scope.SearchCodeGrid = {
                multiSelect: true,
                enableRowSelection: true,
                columnDefs: [
                    { field: 'CPT1__HCPCS', displayName: 'HCPCS/CPT' },
                    { field: 'Description', displayName: 'Description' }
                ],
            };
    
            $scope.CodeGrid = {
                multiSelect: true,
                enableRowSelection: true,
                columnDefs: [
                    { field: 'CPT1__HCPCS', displayName: 'HCPCS/CPT' },
                    { field: 'Description', displayName: 'Description' }
                ],
            };
    
            $scope.searchMPFSByCPT = function() {
                if($scope.SearchValue === null || $scope.SearchValue === undefined){
                    toaster.error('Please input a CPT code or partial CPT code.');
                }else{
                    EMIService.SearchMPFSByCPT($scope.SearchValue, defaultRequestHeaders)
                    .then(function(response){
                        EMIService.serviceSearchedMPFSResults = response;
                        $scope.SearchCodeGrid.data = EMIService.serviceSearchedMPFSResults.data;
                    });
                }
            };
    
            $scope.SearchCodeGrid.onRegisterApi = function (gridApi) {
                //set gridApi on scope
                $scope.gridApi = gridApi;
                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    $scope.items = $scope.gridApi.selection.getSelectedRows();
                });
        
                gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
                    var msg = 'rows changed ' + rows.length;
                    //$log.log(msg);
                });
            };
    
            $scope.CodeGrid.onRegisterApi = function (codeGridApi) {
                //set gridApi on scope
                $scope.codeGridApi = codeGridApi;
                codeGridApi.selection.on.rowSelectionChanged($scope, function (row) {
                    $scope.addedItems = $scope.codeGridApi.selection.getSelectedRows();
                });
        
                codeGridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
                    var msg = 'rows changed ' + rows.length;
                    //$log.log(msg);
                });
            };
    
            $scope.Save = function(){
                if($scope.CaseName === "" || $scope.CaseName === undefined){
                    toaster.error('Please enter a case name.');
                }
                if($scope.CaseDescription === '' || $scope.CaseDescription === undefined){
                    toaster.error('Please enter a case description.');
                }
                if($scope.CodeGrid.data.length === 0 || $scope.CodeGrid.data === null){
                    toaster.error('Please select codes to add to your case');
                }
    
                var obj = {};
                var UserID = localStorage.getItem('UserID');
    
                obj.UserID = UserID;
                obj.CaseName = $scope.CaseName;
                obj.CaseDescription = $scope.CaseDescription;
               
                var data = JSON.stringify(obj);
    
                EMIService.SaveCaseToDB(data, defaultRequestHeaders)
                .then(function(response){
                    var CaseID = response.data.CaseID;
    
                    var data = [];
    
                    for(var x=0; x<$scope.CodeGrid.data.length; x++){
                        code = {"CaseCode1": $scope.CodeGrid.data[x].CPT1__HCPCS};
                        data.push(code);
                    }
    
                    var jsonData = JSON.stringify(data);
    
                    EMIService.SaveCaseCodesToDB(jsonData, CaseID, defaultRequestHeaders)
                    .then(function(response){
                        toaster.success('Case successfully saved');
                        $state.go('app.case-list');
                    });
                }).catch(function(error){
                    toaster.error(error);
                });
    
            };
    
            $scope.RemoveCode = function(){
                EMIService.SelectedCodesToRemove = $scope.addedItems;
    
                for(var x=0; x<EMIService.SelectedCodesToRemove.length; x++){
                    var Code = EMIService.SelectedCodesToRemove[x];
                    var index = $scope.CodeGrid.data.indexOf(Code);
                    $scope.CodeGrid.data.splice(index,1);
                }
    
            };
    
            $scope.AddCode = function () {
                EMIService.SelectedCPTCaseCodes = $scope.items;
    
                for(var x=0; x < EMIService.SelectedCPTCaseCodes.length; x++){
                    $scope.CodeGrid.data.push(EMIService.SelectedCPTCaseCodes[x]);
                }
    
                $scope.gridApi.selection.clearSelectedRows();
            };
    
            EMIService.RetreiveMPFSTotal(defaultRequestHeaders)
            .then(function(response){
                $scope.totalItems = response.data;
        
                $scope.pageSize = 100;
                $scope.totalPages = $scope.totalItems / $scope.pageSize;
                $scope.currentPage = 1;
        
                var obj = {};
                obj.page = $scope.currentPage;
                obj.pageSize = $scope.pageSize;
        
                var data = JSON.stringify(obj);
        
                EMIService.RetreivePagedMPFS(data, defaultRequestHeaders)
                .then(function(response){
                    $scope.SearchCodeGrid.data = response.data;
                });
            });

            $scope.onPageChanged = function(){
                var obj = {};
                obj.page = $scope.currentPage;
                obj.pageSize = $scope.pageSize;
        
                var data = JSON.stringify(obj);
        
                EMIService.RetreivePagedMPFS(data, defaultRequestHeaders)
                .then(function(response){
                    $scope.SearchCodeGrid.data = response.data;
                });
            };
        }
    
    })();
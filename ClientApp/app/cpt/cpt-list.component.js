(function(){
    
        'use strict';
    
        angular.module('EmediCodesApplication')
        .controller('CPTListCtrl', CPTListController);
    
        CPTListController.$inject = ['$scope', '$state', '$http', 'toaster', 'EMIService', 'authService'];
    
        function CPTListController ($scope, $state, $http, toaster, EMIService, authService) {
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
        
            $scope.ViewCPT = function () {
                if ($scope.items != undefined) {
                    localStorage.setItem('currentCPTCode', $scope.items[0].CPT1__HCPCS);            
                    EMIService.SelectedCPTCode = localStorage.getItem('currentCPTCode');
                    $state.go('app.cpt');
                }
                else {
                    toaster.error('Error', 'Please select a CPT code.');
                }
            };
    
            EMIService.RetreiveMPFSTotal(defaultRequestHeaders)
            .then(function(response){
                $scope.totalItems = response.data;
        
                $scope.pageSize = 200;
                $scope.totalPages = $scope.totalItems / $scope.pageSize;
                $scope.currentPage = 1;
        
                var obj = {};
                obj.page = $scope.currentPage;
                obj.pageSize = $scope.pageSize;
        
                var data = JSON.stringify(obj);
        
                EMIService.RetreivePagedMPFS(data, defaultRequestHeaders)
                .then(function(response){
                    $scope.CPTListOptions.data = response.data;
                });
            });
        
            $scope.onPageChanged = function(){
                var obj = {};
                obj.page = $scope.currentPage;
                obj.pageSize = $scope.pageSize;
        
                var data = JSON.stringify(obj);
        
                EMIService.RetreivePagedMPFS(data, defaultRequestHeaders)
                .then(function(response){
                    $scope.CPTListOptions.data = response.data;
                });
            };
        
            $scope.searchMPFSByCPT = function() {
                if($scope.SearchValue === null || $scope.SearchValue === undefined){
                    toaster.error('Please input a CPT code or partial CPT code.');
                }else{
                    EMIService.SearchMPFSByCPT($scope.SearchValue, defaultRequestHeaders)
                    .then(function(response){
                        EMIService.serviceSearchedMPFSResults = response;
                        $scope.CPTListOptions.data = EMIService.serviceSearchedMPFSResults.data;
                    });
                }
            };
    
            $scope.CPTListOptions = {
                multiSelect: false,
                enableFiltering: true,
                enableRowSelection: true,
                columnDefs: [
                    { field: 'CPT1__HCPCS', displayName: 'HCPCS/CPT' },
                    { field: 'Mod', displayName: 'Mod' },
                    { field: 'Status', displayName: 'Status' },
                    { field: 'Description', displayName: 'Description' }
                ],
        
            };
        
            $scope.CPTListOptions.onRegisterApi = function (gridApi) {
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
        }
    
    })();
    
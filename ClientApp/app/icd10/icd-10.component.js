(function () {
    
        angular.module('EmediCodesApplication')
            .controller('ICD10Ctrl', ICD10Controller);
    
        ICD10Controller.$inject = ['$scope', '$state', 'EMIService', 'authService', 'toaster'];
    
        function ICD10Controller($scope, $state, EMIService, authService, toaster) {
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
    
            $scope.ICD10ListOptions = {
                enableFiltering: true,
                enableRowSelection: true,
                columnDefs: [
                    { field: 'Name', displayName: 'Name' },
                    { field: 'CODE', displayName: 'Code' },
                    { field: 'ACTIVE', displayName: 'Is Active' },
                ],
            };
    
            EMIService.GetICD10Count(defaultRequestHeaders)
                .then(function (response) {
    
                    $scope.totalItems = response.data;
    
                    $scope.pageSize = 100;
                    $scope.totalPages = $scope.totalItems / $scope.pageSize;
                    $scope.currentPage = 1;
    
                    var obj = {};
                    obj.page = $scope.currentPage;
                    obj.pageSize = $scope.pageSize;
    
                    var data = JSON.stringify(obj);
    
                    EMIService.GetICD10Paged(data, defaultRequestHeaders)
                        .then(function (response) {
                            EMIService.serviceICD10PagedData = response;
                            $scope.ICD10ListOptions.data = EMIService.serviceICD10PagedData.data;
                        });
    
                });
    
            $scope.onPageChanged = function () {
                var obj = {};
                obj.page = $scope.currentPage;
                obj.pageSize = $scope.pageSize;
    
                var data = JSON.stringify(obj);
    
                EMIService.GetICD10Paged(data, defaultRequestHeaders)
                    .then(function (response) {
                        EMIService.serviceICD10PagedData = response;
                        $scope.ICD10ListOptions.data = EMIService.serviceICD10PagedData.data;
                    });
            };
    
            $scope.searchByICD10Code = function () {
    
                EMIService.SearchICD10($scope.SearchValue, defaultRequestHeaders)
                    .then(function (response) {
                        EMIService.servceSearchedICD10 = response;
                        $scope.ICD10ListOptions.data = EMIService.servceSearchedICD10.data;
                    });
            };
        }
    })();
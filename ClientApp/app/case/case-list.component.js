(function () {
    
        angular.module('EmediCodesApplication')
            .controller('CasesCtrl', CasesController);
    
        CasesController.$inject = ['$scope', 'EMIService', '$state', 'authService', 'toaster', '$uibModal'];
    
        function CasesController($scope, EMIService, $state, authService, toaster, $uibModal) {
            var defaultRequestHeaders = {
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": "Bearer " + localStorage.getItem('id_token')
                }
            };
    
            authService.ValidateAuthorization(defaultRequestHeaders.headers);
    
            var UserID = localStorage.getItem('UserID');
    
            $scope.ViewCase = function(CaseID) {
                EMIService.SelectedCaseID = CaseID;
                $state.go('app.case');
            };

            $scope.DeleteCase = function(CaseID){

                EMIService.deleteSelectedCaseID = CaseID;

                var modalInstance = $uibModal.open({
                    size: 'md',
                    templateUrl: 'ClientApp/app/modals/delete-case/delete-case.modal.html',
                    controller: 'DeleteCaseModalCtrl'
                });

            };

            if(UserID !== null || UserID !== undefined){
                EMIService.GetUserCaseList(UserID, defaultRequestHeaders)
                .then(function (response) {
                    EMIService.serviceUserCaseList = response;
                    $scope.UserCaseList = EMIService.serviceUserCaseList.data;
                }).catch(function (error) {
                    toaster.error(error);
                });
            }

            EMIService.GetUserCaseList(UserID, defaultRequestHeaders)
            .then(function (response) {
                EMIService.serviceUserCaseList = response;
                $scope.UserCaseList = EMIService.serviceUserCaseList.data;
            }).catch(function (error) {
                toaster.error(error);
            });

            $scope.goToCaseForm = function () {
                $state.go('app.create-case');
            };
        }
    
    })();
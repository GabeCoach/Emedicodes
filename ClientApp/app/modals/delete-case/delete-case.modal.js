(function(){

    angular.module('EmediCodesApplication')
    .controller('DeleteCaseModalCtrl', DeleteCaseModalController)

    DeleteCaseModalController.$inject = ['$scope', 'EMIService', 'authService', '$uibModalInstance', 'toaster'];

    function DeleteCaseModalController($scope, EMIService, authService, $uibModalInstance, toaster) {

        var defaultRequestHeaders = {
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('id_token')
            }
        };

        $scope.DeleteCase = function(){
            var CaseID = EMIService.deleteSelectedCaseID;

            EMIService.DeleteCase(CaseID, defaultRequestHeaders)
            .success(function (response) {
                $uibModalInstance.dismiss('cancel');
                toaster.pop('success', 'Case Successfully Deleted');
            }).catch(function (error) {
                toaster.pop('error', 'An Error Occurred On The Server.');
            });
        };

        $scope.Cancel = function(){
            $uibModalInstance.dismiss('cancel');
        };

    }

})();
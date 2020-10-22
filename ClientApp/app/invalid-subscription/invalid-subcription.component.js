(function(){

    angular.module('EmediCodesApplication')
    .controller('InvalidSubscriptionCtrl', InvalidSubscriptionController);

    InvalidSubscriptionController.$inject = ['$scope', 'authService', 'EMIService', 'toaster', '$state'];

    function InvalidSubscriptionController($scope, authService, EMIService, toaster, $state) {
        var defaultRequestHeaders = {
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('id_token')
            }
        };
    
        authService.ValidateAuthorization(defaultRequestHeaders.headers);

        $scope.goToPayment = function() {
            $state.go('payment');
        };

        $scope.contactSupport = function(){
            $state.go('app.bugs');
        };
    }

})();
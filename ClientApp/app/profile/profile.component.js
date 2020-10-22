(function(){

    angular.module('EmediCodesApplication') 
        .controller('UserProfileCtrl', UserProfileController);

    UserProfileController.$inject = ['$scope', '$state', 'EMIService', 'authService', 'toaster'];

    function UserProfileController($scope, $state, EMIService, authService, toaster) {

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
           $scope.isValidSubscription = response.data.ValidSubscription;
        });

        $scope.createSubscription = function(){
            EMIService.CheckUserPaymentMethod(UserId, defaultRequestHeaders)
            .then(function(response){
                if(response.data.PaymentMethodExists){
                    EMIService.CreateNewSubscription(UserId, defaultRequestHeaders)
                    .then(function(response){
                        toaster.success('You have successfully subscribed to e-MediCodes');
                        $state.go('app.case-list');
                    }).catch(function(error){
                        toast.error(error);
                    });
                }
                else{
                    $state.go('payment');
                }
            }).catch(function(error){
                toaster.error(error);
            });
        };

        $scope.cancelSubscription = function() {
            EMIService.CancelUserSubscription(UserId, defaultRequestHeaders)
            .then(function(response){
                toaster.success("Your subscription has been cancelled.");
            }).catch(function(error){
                toaster.error(error);
            });
        };
    }

})();
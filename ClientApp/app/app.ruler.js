(function(){

    angular.module('EmediCodesApplication')
    .controller('AppCtrl', AppController);

    AppController.$inject = ['$scope', 'EMIService', 'authService'];

    function AppController($scope, EMIService, authService) {
        var defaultRequestHeaders = {
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('id_token')
            }
        };

        authService.getProfile();

        $scope.$on('Auth0IDSet', function(){
            EMIService.GetUserProfile(EMIService.Auth0ID, defaultRequestHeaders)
            .then(function(response){
                $scope.UserProfile = response.data;
                EMIService.UserID = response.data.Id;
                localStorage.setItem('UserID', EMIService.UserID);

                EMIService.CheckUserSubscription(EMIService.UserID, defaultRequestHeaders)
                .then(function(response){
                    localStorage.setItem('ValidSubscription', response.data.ValidSubscription);
                    if(!response.data.ValidSubscription)
                        $state.go('app.invalid-subscription');
                });

                EMIService.CheckFreeTrialPeriod(EMIService.UserID, defaultRequestHeaders)
                .then(function(response){
                    localStorage.setItem('InFreeTrialPeriod', response.data.InFreeTrial);
                    if(response.data.InFreeTrial){
                        $scope.ShowFreeTrialBanner = response.data.InFreeTrial;

                        EMIService.GetFreeTrialPeriodAmount(EMIService.UserID, defaultRequestHeaders)
                        .then(function(response){
                            $scope.FreeTrialPeriodAmmount = response.data.FreeTrialDays;
                        }).catch(function(err){
                            console.log(err);
                        });
                    }  
                });

        
            });
        });

        $scope.LogOut = function(){
            authService.logout();
        };
       
    }

})();
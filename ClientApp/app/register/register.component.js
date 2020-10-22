(function(){

    angular.module('EmediCodesApplication')
    .controller('RegistrationCtrl', RegistrationController);

    RegistrationController.$inject = ['$scope', '$state', 'toaster', '$http', 'RegistrationService'];
    
        function RegistrationController($scope, $state, toaster, $http, RegistrationService) {
            $scope.States = RegistrationService.States;
            $scope.Countries = RegistrationService.Countries;
    
            $scope.Register = function () {
                var obj = {};
    
                obj.first_name = $scope.RegisterForm.FirstName.$viewValue;
                obj.last_name = $scope.RegisterForm.LastName.$viewValue;
                obj.email_address = $scope.RegisterForm.CompanyEmail.$viewValue;
                obj.city = $scope.RegisterForm.City.$viewValue;
                obj.state = $scope.RegisterForm.State.$viewValue;
                obj.country = $scope.RegisterForm.Country.$viewValue;
                obj.street_address = $scope.RegisterForm.StreetAddress.$viewValue;
                obj.phone_number = $scope.RegisterForm.PhoneNumber.$viewValue;
                obj.password = $scope.RegisterForm.Password.$viewValue;
                obj.password_confirm = $scope.RegisterForm.PasswordConfirm.$viewValue;

                var data = JSON.stringify(obj);
    
                RegistrationService.RegisterUsers(data)
                    .success(function (response) {
                        RegistrationService.UserId = response.Id;
                        RegistrationService.Auth0Identifier = response.Auth0Identifier;
                        $state.go('payment');
                    }).error(function (response) {
                        toaster.error("Error", response.Message);
                    });
            };
        }

})();
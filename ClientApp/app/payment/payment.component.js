(function(){

    angular.module('EmediCodesApplication')
    .controller('PaymentCtrl', PaymentController);

    PaymentController.$inject = ['$scope', 'toaster', 'RegistrationService', '$state'];
    
    function PaymentController($scope, toaster, RegistrationService, $state){
        var stripe = Stripe('pk_live_uJ6WdiMXisI13NekLrNDPHiv');
        
        var elements = stripe.elements();
        var style = {
                            base: {
                            color: '#32325d',
                            lineHeight: '24px',
                            fontFamily: 'Helvetica Neue',
                            fontSmoothing: 'antialiased',
                            fontSize: '16px',
                            '::placeholder': {
                                color: '#aab7c4'
                            }
                            },
                            invalid: {
                            color: '#fa755a',
                            iconColor: '#fa755a'
                            }
                        };
        $scope.card = elements.create('card', {style: style});
        $scope.card.mount('#card-element');

        // Handle real-time validation errors from the card Element.
        $scope.card.addEventListener('change', function(event) {
                if (event.error) {
                $scope.cardError = event.error.message;
                } else {
                $scope.cardError = '';
                }
        });

            $scope.submit = function() {
            $scope.cardError = '';
            $scope.submitting = true;
            createToken();
        };

        // Send data directly to stripe 
        function createToken() {
            stripe.createToken($scope.card).then(function(result) {
                if (result.error) {
                $scope.cardError = result.error.message;
                $scope.submitting = false;
                } else {
                // Send the token to your server
                stripeTokenHandler(result.token);
                }
            });
        }

        // Response Handler callback to handle the response from Stripe server
        function stripeTokenHandler(token) {
            var obj = {};

            obj.UserId = RegistrationService.UserId;
            obj.Auth0Id = RegistrationService.Auth0Identifier;
            obj.Token = token.id;
            obj.coupon_code = $scope.CouponCode;

            var data = JSON.stringify(obj);

            RegistrationService.HandleStripeToken(data)
            .then(function(response){
                toaster.success('Registration Success', 'You have been successfully registered you may now login.');
                $state.transitionTo('login');
            });

        }
    }

})();
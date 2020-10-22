(function(){

    angular.module('EmediCodesApplication')
    .controller('CallBackCtrl', CallBackController);

    CallBackController.$inject = ['$scope', '$state', 'callBackService'];

    function CallBackController($scope, $state, callBackService) {
        callBackService.handleAuthentication();
    }

})();
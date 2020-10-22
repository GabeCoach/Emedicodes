( function(){

    angular.module('EmediCodesApplication')
    .controller('BugReportCtrl', BugReportingController);

    BugReportingController.$inject = ['$scope', 'authService', 'EMIService', 'toaster'];

    function BugReportingController($scope, authService, EMIService, toaster) {
        var defaultRequestHeaders = {
            headers: {
                "Content-Type": "application/json",
                "Authorization": "Bearer " + localStorage.getItem('id_token')
            }
        };
    
        authService.ValidateAuthorization(defaultRequestHeaders.headers);

        var UserId = localStorage.getItem('UserID');
        
        $scope.submitTicket = function(){
            var obj = {};
            obj.bug_description = $scope.BugReportDescription;

            var data = JSON.stringify(obj);

            EMIService.SubmitBugReport(UserId, defaultRequestHeaders, data)
            .then(function(response){
                $scope.BugReportDescription = "";
                toaster.success('Your Ticket Has Been Submitted.');
            }).catch(function(err){
                toaster.error(err);
            });
        };
    }

})();
app.controller('SchedulerController', function ($scope, $http) {
    getAllSchedules();

    function getAllSchedules() {
        
        $http({
            method: 'GET',
            url: 'http://localhost:63989/api/EmployeeScheduler/GetSchedules'
        })
            .then(function (response) {                 
                $scope.result = response.data;           
      

            });

    }
})   

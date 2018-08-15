app.service("APIService", function ($http) {
    var apiUrl = "http://localhost:63989/";
    this.GetSchedules = function () {
        var scheduleUrl = apiUrl + "api/EmployeeScheduler/GetSchedules";
        return $http.get(scheduleUrl);
    }
});   

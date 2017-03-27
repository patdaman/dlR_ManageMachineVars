//Service to get data from service..
configApp.service('configcrudservice', function ($http) {
    this.getAllMachines = function () {
        return $http.get("/api/ConfigApi");
    };
    //save
    this.save = function (Machine) {
        var request = $http({
            method: 'post',
            url: '/api/ConfigApi/',
            data: Machine
        });
        return request;
    };
    //get single record by Id
    this.get = function (id) {
        //debugger;
        return $http.get("/api/ConfigApi/" + id);
    };
    //update Machine records
    this.update = function (UpdateId, Machine) {
        //debugger;
        var updaterequest = $http({
            method: 'put',
            url: "/api/ConfigApi/" + UpdateId,
            data: Machine
        });
        return updaterequest;
    };
    //delete record
    this.delete = function (UpdateId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            url: "/api/ConfigApi/" + UpdateId
        });
        return deleterecord;
    };
});
//# sourceMappingURL=ConfigService.js.map
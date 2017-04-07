//Service to get data from service..
machineApp.service('machinecrudservice', function ($http) {

    this.getAllMachines = function () {
        return $http.get("/api/MachineApi");
    }

    //save
    this.save = function (Machine) {
        var request = $http({
            method: 'post',
            url: '/api/MachineApi/',
            data: Machine
        });
        return request;
    }

    //get single record by Id
    this.get = function (id) {
        //debugger;
        return $http.get("/api/MachineApi/" + id);
    }

    //update Machine records
    this.update = function (UpdateId, Machine) {
        //debugger;
        var updaterequest = $http({
            method: 'put',
            url: "/api/MachineApi/" + UpdateId,
            data: Machine
        });
        return updaterequest;
    }

    //delete record
    this.delete = function (UpdateId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            url: "/api/MachineApi/" + UpdateId
        });
        return deleterecord;
    }
});
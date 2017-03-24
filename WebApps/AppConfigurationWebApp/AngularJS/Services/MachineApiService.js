//Service to CRUD data from Machine API.
var AppConfigurationWebApp;
(function (AppConfigurationWebApp) {
    var Service;
    (function (Service) {
        var MachineApiService = (function () {
            function MachineApiService($http, $q) {
                this.httpService = $http;
                this.qService = $q;
                this.machineList = null;
                this.machine = null;
                this.deleteRecord = null;
                this.GetMachinesAsync();
            }
            MachineApiService.prototype.GetMachinesAsync = function () {
                var dfo = this.qService.defer();
                var current = this;
                if (this.machineList)
                    dfo.resolve(this.machineList);
                else {
                    //var server: string = "http://devapi/api/";
                    this.httpService.get('api:/api/MachineApi/')
                        .error(function (error) {
                        dfo.reject('Failed to get Machine List');
                    });
                }
                return dfo.promise;
            };
            MachineApiService.prototype.GetMachine = function (id) {
                var dfo = this.qService.defer();
                var current = this;
                if (this.machine)
                    dfo.resolve(this.machine);
                else {
                    //var server: string = "http://devapi/api/";
                    this.httpService.get('api:/api/MachineApi/' + id)
                        .error(function (error) {
                        dfo.reject('Failed to get Machine');
                    });
                }
                return dfo.promise;
            };
            MachineApiService.prototype.CreateMachine = function (machine) {
                var dfo = this.qService.defer();
                var current = this;
                //var server: string = "http://devapi/api/";
                this.httpService.post('api:/api/MachineApi/', { data: machine })
                    .error(function (error) {
                    dfo.reject('Failed to create Machine');
                });
                return dfo.promise;
            };
            MachineApiService.prototype.UpdateMachine = function (updateId, machine) {
                var dfo = this.qService.defer();
                var current = this;
                //var server: string = "http://devapi/api/";
                this.httpService.put('api:/api/MachineApi/' + updateId, { data: machine })
                    .error(function (error) {
                    dfo.reject('Failed to update Machine');
                });
                return dfo.promise;
            };
            MachineApiService.prototype.DeleteMachine = function (id) {
                var dfo = this.qService.defer();
                var current = this;
                //var server: string = "http://devapi/api/";
                this.httpService.delete('api:/api/MachineApi/' + id)
                    .error(function (error) {
                    dfo.reject('Failed to delete Machine');
                });
                return dfo.promise;
            };
            return MachineApiService;
        }());
        Service.MachineApiService = MachineApiService;
    })(Service = AppConfigurationWebApp.Service || (AppConfigurationWebApp.Service = {}));
})(AppConfigurationWebApp || (AppConfigurationWebApp = {}));

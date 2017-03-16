//Service to CRUD data from Machine API.
module AppConfigurationWebApp.Service {
    export class MachineApiService {
        private httpService: ng.IHttpService;
        private qService: ng.IQService;
        public machine: Model.Machine;
        public machineList: Model.Machine[];
        public deleteRecord: number;

        constructor($http: ng.IHttpService, $q: ng.IQService) {
            this.httpService = $http;
            this.qService = $q;
            this.machineList = null;
            this.machine = null;
            this.deleteRecord = null;
            this.GetMachinesAsync();
        }

        public GetMachinesAsync(): ng.IPromise<Model.Machine[]> {
            var dfo = this.qService.defer<Model.Machine[]>();
            var current: any = this;

            if (this.machineList)
                dfo.resolve(this.machineList);
            else {
                //var server: string = "http://devapi/api/";
                this.httpService.get('api:/api/MachineApi/'
                    //    , { withCredentials: true }
                )
                    .error(function (error) {
                        dfo.reject('Failed to get Machine List');
                    });
            }
            return dfo.promise;
        }

        public GetMachine(id): ng.IPromise<Model.Machine> {
            var dfo = this.qService.defer<Model.Machine>();
            var current: any = this;

            if (this.machine)
                dfo.resolve(this.machine);
            else {
                //var server: string = "http://devapi/api/";
                this.httpService.get('api:/api/MachineApi/' + id
                    //    , { withCredentials: true }
                )
                    .error(function (error) {
                        dfo.reject('Failed to get Machine');
                    });
            }
            return dfo.promise;
        }

        public CreateMachine(machine): ng.IPromise<Model.Machine> {
            var dfo = this.qService.defer<Model.Machine>();
            var current: any = this;

            //var server: string = "http://devapi/api/";
            this.httpService.post('api:/api/MachineApi/',
                { data: machine }
                //, { withCredentials: true }
            )
                .error(function (error) {
                    dfo.reject('Failed to create Machine');
                });
            return dfo.promise;
        }

        public UpdateMachine(updateId, machine): ng.IPromise<Model.Machine> {
            var dfo = this.qService.defer<Model.Machine>();
            var current: any = this;

            //var server: string = "http://devapi/api/";
            this.httpService.put('api:/api/MachineApi/' + updateId,
                { data: machine }
                //, { withCredentials: true }
            )
                .error(function (error) {
                    dfo.reject('Failed to update Machine');
                });
            return dfo.promise;
        }

        public DeleteMachine(id): ng.IPromise<Model.Machine> {
            var dfo = this.qService.defer<Model.Machine>();
            var current: any = this;

            //var server: string = "http://devapi/api/";
            this.httpService.delete('api:/api/MachineApi/' + id
                //    , { withCredentials: true }
            )
                .error(function (error) {
                    dfo.reject('Failed to delete Machine');
                });
            return dfo.promise;
        }
    }
}


var app = angular.module('app', ['ui.grid', 'ui.grid.edit', 'ui.grid.pagination']);

app.controller('machinecontroller', function ($scope, machinecrudservice) {
    $scope.title = "Machine Configuration ";
    $scope.gridOptions = {
        //grid pagination
        paginationPageSizes: [10, 25, 50, 75],
        paginationPageSize: 10,
        enableSorting: true,
        //enabling filtering
        enableFiltering: true,
        enableEditing: true,
        //column definations
        //we can specify sorting mechnism also
        ColumnDefs: [
            { field: 'id' },
            { field: 'machine_name', enableEditing: true },
            { field: 'location', enableEditing: true },
            { field: 'usage' },
            { field: 'create_date', enableFiltering: false },
            { field: 'modify_date' },
            { field: 'active', enableEditing: true },
        ],

    };
    //api that is called every time
    // when data is modified on grid for sorting
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    }

        //Loads all Machine records when page loads
        loadMachines();
        function loadMachines() {
            var MachineRecords = machinecrudservice.getAllMachines();
            MachineRecords.then(function (d) {     //success
                $scope.Machines = d.data;
            },
            function () {
                swal("Oops..", "Error occured while loading", "error"); //fail
            });
        }

        //save form data
        $scope.save = function () {
            //debugger;
            var Machine = {
                id: $scope.id,
                machine_name: $scope.machine_name,
                location: $scope.location,
                usage: $scope.usage,
                create_date: $scope.create_date,
                modify_date: $scope.modify_date,
                active: $scope.active
            };

            var saverecords = machinecrudservice.save(Machine);
            saverecords.then(function (d) {
                $scope.id = d.data.id;
                loadMachines();
                swal("Reord inserted successfully");
            },
            function () {
                swal("Oops..", "Error occured while saving", 'error');
            });
        }

        //get single record by ID

        $scope.get = function (Machine) {
            //debugger;
            var singlerecord = crudservice.get(Machine.id);
            singlerecord.then(function (d) {
                // debugger;
                var record = d.data;
                $scope.Updateid = record.id;
                $scope.Updatemachine_name = record.machine_name;
                $scope.Updatelocation = record.location;
                $scope.Updateusage = record.usage;
                $scope.Updatecreate_date = record.create_date;
                $scope.Updatemodify_date = record.modify_date;
                $scope.Updateactive = record.active;
            },
            function () {
                swal("Oops...", "Error occured while getting record", "error");
            });
        }

        //update Machine data
        $scope.update = function () {
            //debugger;
            var Machine = {
                id: $scope.Updateid,
                machine_name: $scope.Updatemachine_name,
                location: $scope.Updatelocation,
                usage: $scope.Updateusage,
                create_date: $scope.Updatecreate_date,
                modify_date: $scope.Updatemodify_date,
                active: $scope.Updateactive
            };
            debugger;
            var updaterecords = crudservice.update($scope.Updateid, Machine);
            updaterecords.then(function (d) {
                loadMachines();
                swal("Record updated successfully");
            },
            function () {
                swal("Opps...", "Error occured while updating", "error");
            });
        }

        //delete Machine record
        $scope.delete = function (Updateid) {
            debugger;
            var deleterecord = crudservice.delete($scope.Updateid);
            deleterecord.then(function (d) {
                var Machine = {
                    id: '',
                    machine_name: '',
                    location: '',
                    usage: '',
                    create_date: '',
                    modify_date: '',
                    active: '',
                };
                loadMachines();
                swal("Record deleted succussfully");
        });
    }

    $scope.gridOptions.data = $scope.users;
});
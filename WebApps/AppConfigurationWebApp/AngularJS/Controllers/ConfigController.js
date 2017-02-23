var app = angular.module('app', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);
app.controller('ConfigController', function ($scope, $http) {
    $scope.title = "Application Configuration";
    var id;
    var machine_name;
    var location;
    var usage;
    var create_date;
    var modify_date;
    var active;
    //var x;
    $scope.submit = function () {
        id = $scope.id;
        machine_name = $scope.machine_name;
        location = $scope.location;
        usage = $scope.usage;
        create_date = $scope.create_date;
        modify_date = $scope.modify_date;
        active = $scope.active;
        $scope.myData.push({
            id: id,
            machine_name: machine_name,
            location: location,
            usage: usage,
            create_date: create_date,
            modify_date: modify_date,
            active: active
        });
    };
    $scope.gridOptions = {
        enablePaging: true,
        //paginationPageSizes: [10, 25, 50, 75],
        //paginationPageSize: 10,
        pagingOptions: $scope.pagingOptions,
        enablePinning: true,
        showFooter: true,
        enableSorting: true,
        enableFiltering: true,
        enableEditing: true,
        enableColumnResize: true,
        enableCellSelection: true,
        expandableRowTemplate: 'expandableRowTemplate.html',
        expandableRowHeight: 150,
        //subGridVariable will be available in subGrid scope
        expandableRowScope: {
            subGridVariable: 'subGridScopeVariable'
        },
        //column definitions
        //we can specify sorting mechnism also
        ColumnDefs: [
            { field: 'id' },
            { field: 'machine_name', enableEditing: true },
            { field: 'location', enableEditing: true },
            { field: 'usage' },
            { field: 'create_date', enableFiltering: false },
            { field: 'modify_date' },
            { field: 'active', enableEditing: true },
            {
                field: "Action",
                width: 200,
                enableCellEdit: false,
                cellTemplate: '<button id="editBtn" type="button" class="btn btn-xs btn-info"  ng-click="updateCell()" >Click a Cell for Edit </button>'
            }
        ],
    };
    $scope.selectedCell;
    $scope.selectedRow;
    $scope.selectedColumn;
    $scope.editCell = function (row, cell, column) {
        $scope.selectedCell = cell;
        $scope.selectedRow = row;
        $scope.selectedColumn = column;
    };
    $scope.updateCell = function () {
        //   alert("checking");  
        $scope.selectedRow[$scope.selectedColumn] = $scope.selectedCell;
    };
    var basicCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()" ng-click="editCell(row.entity, row.getProperty(col.field), col.field)"><span class="ui-disableSelection hover">{{row.getProperty(col.field)}}</span></div>';
    $scope.filterOptions = {
        filterText: "",
        useExternalFilter: true
    };
    $scope.gridOptions.sortInfo = {
        fields: ['machine_name', 'usage'],
        directions: ['asc'],
        columns: [0, 1]
    };
    //$scope.totalServerItems = 0;
    $scope.pagingOptions = {
        pageSizes: [5, 10, 20],
        pageSize: 5,
        currentPage: 1
    };
    $scope.changeGroupBy = function (group1, group2) {
        $scope.gridOptions.$gridScope.configGroups = [];
        $scope.gridOptions.$gridScope.configGroups.push(group1);
        $scope.gridOptions.$gridScope.configGroups.push(group2);
        $scope.gridOptions.groupBy();
    };
    $scope.clearGroupBy = function () {
        $scope.gridOptions.$gridScope.configGroups = [];
        $scope.gridOptions.groupBy();
    };
    //api that is called every time
    // when data is modified on grid for sorting
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };
    //Loads all Machine records when page loads
    loadConfigs();
    function loadConfigs() {
        var ConfigRecords = $http.get("/api/ConfigApi");
        ConfigRecords.then(function (d) {
            $scope.gridOptions.data = d.data;
        }, function () {
            //swal("Oops..", "Error occured while loading", "error"); //fail
        });
    }
    $scope.get = function () {
        return $http.get("/api/ConfigApi");
    };
    //save form data
    $scope.save = function () {
        //debugger;
        var ConfigVar = {
            id: $scope.id,
            machine_name: $scope.machine_name,
            location: $scope.location,
            usage: $scope.usage,
            create_date: $scope.create_date,
            modify_date: $scope.modify_date,
            active: $scope.active
        };
        var saverecords = $http({
            method: 'post',
            url: '/api/ConfigApi/',
            data: ConfigVar
        });
        saverecords.then(function (d) {
            $scope.id = d.data.id;
            loadConfigs();
            //swal("Reord inserted successfully");
        }, function () {
            //swal("Oops..", "Error occured while saving", 'error');
        });
    };
    //get single record by ID
    $scope.get = function (ConfigVar) {
        //debugger;
        var singlerecord = $http.get("/api/ConfigApi/"); // + Machine.id);
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
        }, function () {
            //swal("Oops...", "Error occured while getting record", "error");
        });
    };
    //update Config data
    $scope.update = function () {
        //debugger;
        var ConfigVar = {
            id: $scope.Updateid,
            machine_name: $scope.Updatemachine_name,
            location: $scope.Updatelocation,
            usage: $scope.Updateusage,
            create_date: $scope.Updatecreate_date,
            modify_date: $scope.Updatemodify_date,
            active: $scope.Updateactive
        };
        debugger;
        var updaterecords = $http({
            method: 'put',
            url: "/api/ConfigApi/",
            data: ConfigVar
        });
        updaterecords.then(function (d) {
            loadConfigs();
            //swal("Record updated successfully");
        }, function () {
            //swal("Opps...", "Error occured while updating", "error");
        });
    };
    //delete Config record
    $scope.delete = function (updateId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            url: "/api/ConfigApi/" + updateId
        });
        deleterecord.then(function (d) {
            var ConfigVar = {
                id: '',
                machine_name: '',
                location: '',
                usage: '',
                create_date: '',
                modify_date: '',
                active: '',
            };
            loadConfigs();
            //swal("Record deleted succussfully");
        });
    };
    $scope.gridOptions.data = $scope.Configs;
});

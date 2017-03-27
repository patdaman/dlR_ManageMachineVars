'use strict';
var app = angular.module('app', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable', 'ui.grid.cellNav',
    'ui.grid.grouping', 'ui.grid.selection', 'ui.grid.rowEdit',
    'ui.grid.selection', 'ui.grid.pinning', 'ui.grid.exporter']);
app.controller('MachineController', function ($scope, $http, uiGridGroupingConstants) {
    $scope.title = "Machine Configuration ";
    var vm = $scope;
    var data = [];
    var id;
    var machine_name;
    var location;
    var usage;
    var create_date;
    var modify_date;
    var active;
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
        showGridFooter: true,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        exporterMenuCsv: true,
        exporterMenuPdf: true,
        exporterCsvFilename: 'Machines.csv',
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterPdfHeader: { text: "Marcom Central Servers", style: 'headerStyle' },
        exporterPdfFooter: function (currentPage, pageCount) {
            return { text: currentPage.toString() + ' of ' + pageCount.toString(), style: 'footerStyle' };
        },
        exporterPdfCustomFormatter: function (docDefinition) {
            docDefinition.styles.headerStyle = { fontSize: 22, bold: true };
            docDefinition.styles.footerStyle = { fontSize: 10, bold: true };
            return docDefinition;
        },
        exporterPdfOrientation: 'landscape',
        exporterPdfPageSize: 'LETTER',
        exporterPdfMaxGridWidth: 500,
        exporterSuppressColumns: ['Action'],
        treeRowHeaderAlwaysVisible: false,
        enableSelectAll: true,
        enableEditing: true,
        enableColumnResize: false,
        enableCellSelection: false,
        enableRowSelection: true,
        //expandableRowTemplate: 'expandableRowTemplate.html',
        //expandableRowHeight: 150,
        ////subGridVariable will be available in subGrid scope
        //expandableRowScope: {
        //    subGridVariable: 'subGridScopeVariable'
        //},
        //column definitions
        columnDefs: [
            //{ displayName: '#', cellTemplate: '{{rowRenderIndex + 1}}' },
            { field: 'id', visible: false },
            { field: 'machine_name', cellTemplate: basicCellTemplate, width: '20%' },
            { field: 'ip_address', cellTemplate: basicCellTemplate, width: '10%' },
            { field: 'location', cellTemplate: basicCellTemplate, groupable: true, width: '15%' },
            { field: 'usage', width: '15%' },
            { field: 'create_date', enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"', width: '15%' },
            { field: 'modify_date', enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"', width: '15%' },
            { field: 'active', enableEditing: true, type: 'boolean', width: '8%' },
            {
                field: "Action",
                width: 80,
                groupable: false,
                filterable: false,
                sortable: false,
                enableCellEdit: false,
                cellTemplate: '<button id="editBtn" type="button" class="btn btn-xs btn-info"  ng-click="updateCell()" >Edit Row </button>'
            }
        ],
        data: data,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }
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
    loadMachines();
    function loadMachines() {
        var MachineRecords = $http.get("/api/MachineApi");
        MachineRecords.then(function (d) {
            $scope.gridOptions = { data: d.data };
            //$scope.gridOptions.data = d.data;
        }, function () {
            //swal("Oops..", "Error occured while loading", "error"); //fail
        });
    }
    $scope.get = function () {
        return $http.get("/api/MachineApi");
    };
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
        var saverecords = $http({
            method: 'post',
            url: '/api/MachineApi/',
            data: Machine
        });
        saverecords.then(function (d) {
            $scope.id = d.data.id;
            loadMachines();
            //swal("Reord inserted successfully");
        }, function () {
            //swal("Oops..", "Error occured while saving", 'error');
        });
    };
    //get single record by ID
    $scope.get = function (Machine) {
        //debugger;
        var singlerecord = $http.get("/api/MachineApi/" + Machine.id);
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
        var updaterecords = $http({
            method: 'put',
            url: "/api/MachineApi/" + Machine.id,
            data: Machine
        });
        updaterecords.then(function (d) {
            loadMachines();
            //swal("Record updated successfully");
        }, function () {
            //swal("Opps...", "Error occured while updating", "error");
        });
    };
    //delete Machine record
    $scope.delete = function (updateId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            url: "/api/MachineApi/" + updateId
        });
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
            //swal("Record deleted succussfully");
        });
    };
    //$scope.gridOptions.data = $scope.Machines;
});
//# sourceMappingURL=MachineController.js.map
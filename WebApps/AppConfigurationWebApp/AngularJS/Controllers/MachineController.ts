﻿////module ManageAppConfig.Controller {

////    export class MachineController {

////        machineEditorGridOptions: uiGrid.IGridOptions = undefined;
////        httpServ: ng.IHttpService;
////        qServ: ng.IQService;

////        fromDate: Date;
////        toDate: Date;
////        fromDateString: string;
////        toDateString: string;
////        dateType: string;

////        identifier: string;
////        toolbarTemplate: any;
////        detailTemplate: any;
////        // dataGridSource: uiGrid.IGridInstance;
////        filterCaseNumber: string = "";

////        getData: Function;

////        enumService: Service.EnumListService;

////        billingAggregateEnum: any;
////        billingClassificationEnum: any;
////        billStatusEnum: any;

////        programGroupSelection: string;
////        billingAggregateSelection: string;
////        billingClassificationSelection: string;
////        billStatusSelection: string;

////        enumListReceived: boolean = false;

////        usageTypeEnum: any;
////        locationEnum: any;

////        usageTypeList: any;
////        locationList: any;

////        usageTypeSelection: string;
////        locationSelection: string;

////        editRowDataModel: uiGrid.IGridRow;
////        dataModel: any;
////        exportFlag: boolean = false;

////        updateActionComment: string;
////        machineListStr: string;

////        ///-------------------------------------------------------------------------------------------------
////        /// <summary>   Constructor. </summary>
////        ///
////        /// <param name="$http">        The $http. </param>
////        /// <param name="$q">           The $q. </param>
////        /// <param name="PayorService"> The payor service. </param>
////        /// <param name="EnumService">  The enum service. </param>
////        ///-------------------------------------------------------------------------------------------------

////        constructor($rootScope, $http, $q,
////            {

////            this.gridOptions = undefined;


////            this.httpServ = $http;
////            this.qServ = $q;

////            this.enumService = EnumService;
////            this.utilService = UtilService;

////            this.identifier = $rootScope.AppBuildStatus + "Machine Editor";

////            this.dateType = "Create Date";

////            this.toDate = new Date();
////            this.fromDate = new Date();
////            this.fromDate.setDate(this.toDate.getDate() - 7);

////            this.fromDateString = DateToUSString(this.fromDate);
////            this.toDateString = DateToUSString(this.toDate);

////            this.payorListReceived = false;
////            this.enumListReceived = false;

////            this.dataGridSource = this.initDataGridSource($http);

////            if (this.enumService.EnumServiceReady) {
////                this.enumListRecdProc();
////            }
////            else {
////                var current: CaseEditorController = this;
////                this.enumService.PopulateEnumListsAsync().then(function (data) {
////                    current.enumListRecdProc();
////                }, function (reason) {
////                    console.log("Error loading enums");
////                })
////            }
////        }
'use strict'

var app = angular.module('app', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);

app.controller('MachineController', function ($scope, $http, uiGridConstants) {
    $scope.title = "Machine Configuration ";

    var vm = $scope;
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
        showFooter: true,
        enableSorting: true,
        enableFiltering: true,

        enableEditing: true,
        enableColumnResize: true,
        enableCellSelection: true,

        //expandableRowTemplate: 'expandableRowTemplate.html',
        //expandableRowHeight: 150,
        ////subGridVariable will be available in subGrid scope
        //expandableRowScope: {
        //    subGridVariable: 'subGridScopeVariable'
        //},

        //column definitions
        //we can specify sorting mechnism also
        columnDefs: [
            { field: 'id', visible: false },
            { field: 'machine_name', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'location', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'usage' },
            { field: 'create_date', enableEditing: false, enableFiltering: false },
            { field: 'modify_date', enableEditing: false },
            { field: 'active', enableEditing: true },
            {
                field: "Action",
                width: 200,
                enableCellEdit: false,
                cellTemplate: '<button id="editBtn" type="button" class="btn btn-xs btn-info"  ng-click="updateCell()" >Click a Cell for Edit </button>'
            }
        ],
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
    }
    $scope.clearGroupBy = function () {
        $scope.gridOptions.$gridScope.configGroups = [];
        $scope.gridOptions.groupBy();
    }

    //api that is called every time
    // when data is modified on grid for sorting
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    }

    //Loads all Machine records when page loads
    loadMachines();
    function loadMachines() {
        var MachineRecords = $http.get("/api/MachineApi");
        MachineRecords.then(function (d) {     //success
            $scope.gridOptions.data = d.data;
        },
            function () {
                //swal("Oops..", "Error occured while loading", "error"); //fail
            });
    }

    $scope.get = function () {
        return $http.get("/api/MachineApi");
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

        var saverecords = $http({
            method: 'post',
            url: '/api/MachineApi/',
            data: Machine
        });
        saverecords.then(function (d) {
            $scope.id = d.data.id;
            loadMachines();
            //swal("Reord inserted successfully");
        },
            function () {
                //swal("Oops..", "Error occured while saving", 'error');
            });
    }

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
        },
            function () {
                //swal("Oops...", "Error occured while getting record", "error");
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
        var updaterecords = $http({
            method: 'put',
            url: "/api/MachineApi/" + Machine.id,
            data: Machine
        });
        updaterecords.then(function (d) {
            loadMachines();
            //swal("Record updated successfully");
        },
            function () {
                //swal("Opps...", "Error occured while updating", "error");
            });
    }

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
    }

    $scope.gridOptions.data = $scope.Machines;
});
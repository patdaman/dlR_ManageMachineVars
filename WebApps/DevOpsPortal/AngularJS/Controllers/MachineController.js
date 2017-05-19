'use strict'

MachineApp.controller('MachineController', ['$scope', '$http', 'uiGridConstants',
    '$log', '$timeout', '$q', '$interval', 'ModalService',
    function ($scope, $http, uiGridConstants,
    $log, $timeout, $q, $interval, ModalService) {
    $scope.title = "Machine Configuration ";

    var vm = $scope;

    var apiRelPath = "api:/MachineApi";

    var rowIndex;
    var rowId;

    var data = [];
    var id;
    var machine_name;
    var location;
    var usage;
    var create_date;
    var modify_date;
    var active;

    var environments = [];
    var components = [];
    var applications = [];

    var componentId;
    var componentName;

    var selectedComponent;
    var component;
    var selectedApplication;
    var application;
    var selectedEnvironment;
    var environment;

    /// Display current API path and link to Help page
    $scope.ApiBaseUrl = ApiPath;
    $scope.ApiBaseUrlHelp = $scope.ApiBaseUrl.slice(0, -4) + '/Help';

    /// Grid Filters
    $scope.environment = '';
    $scope.filterEnvironment = function () {
        return $scope.environment;
    };
    $scope.application = '';
    $scope.component = '';
    $scope.dateTimeString = function () {
        date: 'yyyy-MM-dd_HH:mm(Z)'
        return date;
    }

    $scope.gridOptions = {
        enablePaging: true,
        paginationPageSizes: [10, 20, 50, 100],
        paginationPageSize: 25,
        //enableHorizontalScrollbar: 0,

        enablePinning: true,
        showGridFooter: true,
        enableSorting: true,
        enableFiltering: true,

        enableGridMenu: true,
        exporterMenuCsv: true,
        exporterMenuPdf: true,
        exporterCsvFilename: 'Machines' + $scope.dateTimeString + '.csv',
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterPdfTableStyle: { margin: [20, 10, 20, 20] },
        exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
        exporterPdfHeader: { text: "Marcom Central - Server Configuration", style: 'headerStyle' },
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

        //treeRowHeaderAlwaysVisible: false,

        enableCellSelection: true,
        enableCellEditOnFocus: true,
        enableRowSelection: true,
        enableRowHeaderSelection: true,
        enableMultiselect: true,

        //column definitions
        columnDefs: [
            { field: 'id', visible: false },
            {
                field: 'usage',
                width: '10%',
                visible: true,
                enableFiltering: false,
                filter: {
                    noTerm: true,
                    condition: function (searchTerm, cellValue) {
                        return $scope.filterEnvironment() === cellValue
                            || $scope.environment == '';
                    }
                },
                grouping: { groupPriority: 0 },
                sort: { priority: 0, direction: 'asc' },
                groupable: true,
            },
            {
                field: 'location',
                //grouping: { groupPriority: 1 },
                sort: { priority: 1, direction: 'asc' },
                groupable: true,
                width: '15%'
            },
            { field: 'machine_name', width: '20%' },
            { field: 'uri', width: '20%' },
            { field: 'ip_address', width: '10%' },
            { field: 'create_date', enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"', width: 120 },
            { field: 'modify_date', enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"', width: 120 },
            {
                field: 'active',
                type: 'boolean',
                enableFiltering: false,
                width: 80
            },
            //{ field: 'Enum_Locations', visible: false },
            //{ field: 'MachineComponentPaths', visible: false },
            //{ field: 'EnvironmentVariables', visible: true },

            {
                field: "Action",
                width: 80,
                groupable: false,
                filterable: false,
                sortable: false,
                enableCellEdit: false,
                enableFiltering: false,
                cellTemplate: '/Content/Templates/machineActionsTemplate.html'
            }
        ],
    };

    $scope.filterOptions = {
        filterText: "",
        useExternalFilter: true
    };

    $scope.gridOptions.sortInfo = {
        fields: ['machine_name', 'usage'],
        directions: ['asc'],
        columns: [0, 1]
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

    // List of environments:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //withCredentials: true,
        params: {
            type: "environment"
            //parameters: "type=environment"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.environments = result.data;
    });

    // List of components:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //withCredentials: true,
        params: {
            type: "component"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.components = result.data;
    });

    // List of applications:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //withCredentials: true,
        params: {
            type: "application"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.applications = result.data;
    });

    // Function call from Index page dropdown OnChange
    $scope.updateEnvironment = function () {
        $scope.environment = $scope.selectedEnvironment.value;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        //$scope.filterSubGrid($scope.environment);
    };
    // Function call from Index page dropdown OnChange
    $scope.updateComponent = function () {
        $scope.component = $scope.selectedComponent;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        //$scope.gridApi.grid.refresh();
    };
    // Function call from Index page dropdown OnChange
    $scope.updateApplication = function () {
        $scope.application = $scope.selectedApplication;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
    };

    $http.get('api:/MachineApi')
        .success(function (data) {
            //for (i = 0; i < data.length; i++) {
            //    data[i].subGridOptions = {
            //        enableHorizontalScrollbar: 0,
            //        enableVerticalScrollbar: 0,
            //        appScopeProvider: $scope,
            //        enableFiltering: true,
            //        enableCellSelection: true,
            //        enableCellEditOnFocus: true,
            //        enableRowSelection: false,
            //        enableRowHeaderSelection: false,
            //        enableMultiselect: false,
            //        treeRowHeaderAlwaysVisible: false,
            //        columnDefs: [
            //            { displayName: "id", field: "id", visible: false },
            //            { displayName: "Variable id", field: "configvar_id", visible: false },
            //            {
            //                field: "environment",
            //                visible: true,
            //                enableFiltering: false,
            //                filter: {
            //                    noTerm: true,
            //                    condition: function (searchTerm, cellValue) {
            //                        return $scope.filterEnvironment() === cellValue;
            //                    }
            //                },
            //                filterCellFiltered: true,
            //                enableCellEdit: false
            //            },
            //            {
            //                displayName: "Value",
            //                field: "value",
            //                visible: true,
            //                cellEditableCondition: $scope.subCanEdit,
            //                enableFiltering: false, width: "70%"
            //            },
            //            { displayName: "Create Date", field: "create_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
            //            { displayName: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, type: 'date', enableFiltering: false, cellFilter: 'date:"MM-dd-yyyy"' },
            //            { displayName: "Last Publish Date", field: "publish_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
            //            { displayName: "Is Published", field: "published", visible: false, enableCellEdit: false, type: 'boolean' },
            //            {
            //                name: "Actions",
            //                cellTemplate: '/Content/Templates/subGridActionsTemplate.html',
            //                enableCellEdit: false,
            //                width: 149,
            //                visible: true,
            //                enableFiltering: false
            //            },
            //        ],
            //        data: data[i].values,
            //        onRegisterApi: function (api) {
            //            $scope.subGridApi = api;
            //            api.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {
            //                if ($scope.bypassEditCancel === false) {
            //                    if (newRowCol.row.entity.environment !== $scope.environment || newRowCol.row.entity.configvar_id !== $scope.var_id || $scope.edit === true) {
            //                        $scope.cancelEdit();
            //                        if (oldRowCol !== null && oldRowCol !== "undefined") {
            //                            oldRowCol.row.grid.api.core.notifyDataChange(uiGridConstants.dataChange.ALL);
            //                        }
            //                    }
            //                }
            //                $scope.var_id = newRowCol.row.entity.configvar_id;
            //            });
            //            api.rowEdit.on.saveRow($scope, $scope.cancelEdit());
            //        }
            //    };
            //}
            $scope.gridOptions.data = data;
            angular.forEach(data, function (data, index) {
                data["index"] = index;
            });
        });

    //save form data
    $scope.save = function () {
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
            url: 'api:/MachineApi',
            data: Machine
        });
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
        var singlerecord = $http.get("api:/MachineApi" + Machine.id);
        singlerecord.then(function (d) {
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
            url: "api:/MachineApi" + Machine.id,
            data: Machine
        });
        updaterecords.then(function (d) {
            loadMachines();
            swal("Record updated successfully");
        },
            function () {
                swal("Opps...", "Error occured while updating", "error");
            });
    }

    //delete Machine record
    $scope.delete = function (updateId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            url: "api:/MachineApi" + updateId
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
            swal("Record deleted succussfully");
        });
    }

    //$scope.gridOptions.data = $scope.Machines;
}]);
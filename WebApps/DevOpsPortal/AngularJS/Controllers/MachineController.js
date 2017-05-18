﻿'use strict'

app.controller('MachineController', ['$scope', '$http', 'uiGridConstants',
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
            exporterPdfHeader: { text: "Marcom Central - Machine Configuration", style: 'headerStyle' },
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
                { field: 'ConfigVariableValues', visible: false },
                { field: 'Enum_Locations', visible: false },
                { field: 'MachineComponentPaths', visible: false },
                { field: 'EnvironmentVariables', visible: true },

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
            params: {
                type: "environment"
            },
        }).then(function (result) {
            $scope.environments = result.data;
        });

        // List of components:
        $http({
            method: 'GET',
            url: 'api:/ConfigValuesApi',
            params: {
                type: "component"
            },
        }).then(function (result) {
            $scope.components = result.data;
        });

        // List of applications:
        $http({
            method: 'GET',
            url: 'api:/ConfigValuesApi',
            params: {
                type: "application"
            },
        }).then(function (result) {
            $scope.applications = result.data;
        });

        // Function call from Index page dropdown OnChange
        $scope.updateEnvironment = function () {
            $scope.environment = $scope.selectedEnvironment.value;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };
        // Function call from Index page dropdown OnChange
        $scope.updateComponent = function () {
            $scope.component = $scope.selectedComponent;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };
        // Function call from Index page dropdown OnChange
        $scope.updateApplication = function () {
            $scope.application = $scope.selectedApplication;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };

        $scope.loadGrid = function () {
            var def = $q.defer();
            $http({
                method: 'GET',
                url: apiRelPath,
            })
            .success(def.resolve)
            .success(function (data) {
                $scope.gridOptions.data = data;
                angular.forEach(data, function (data, index) {
                    data["index"] = index;
                });
            })
            .error(
                function () {
                    swal("Oops..", "Error occured while loading grid", "error");
                });
        };
        $scope.loadGrid;

        //save form data
        $scope.save = function (row) {
            var machineEntity = JSON.stringify(row.entity);
            var def = $q.defer;
            $http({
                method: 'post',
                url: 'api:/MachineApi',
                //data: Machine
                data: machineEntity,
            })
            .success(def.resolve)
            .success(function (d) {
                $scope.id = d.data.id;
                loadMachines();
                swal("Reord inserted successfully");
            })
            .error(function () {
                swal("Oops..", "Error occured while saving", 'error');
            });
        };

        //get single record by ID
        $scope.get = function (machineId) {
            var def = $q.defer();
            $http({
                method: 'GET',
                url: apiRelPath + '/' + machineId,
            })
            .success(def.resolve)
            .success(function (data) {
                var record = data;
                def.resolve;
            })
            .error(function (error) {
                var Message = "Error occured while getting record";
                if (error.Message)
                    Message = Message + ":\n" + error.Message
                swal("Oops...", Message, "error");
                console.log(error);
            });
        };

        //update Machine data
        $scope.update = function (row) {
            var def = $q.defer();
            var machineEntity = JSON.stringify(row.entity);
            $http({
                method: 'put',
                url: apiRelPath,
                //url: "api:/MachineApi" + Machine.id,
                //data: Machine
                data: machineEntity,
            })
            .success(def.resolve)
            .success(function (d) {
                loadMachines();
                swal("Record updated successfully");
            })
            .error(function () {
                High("Opps...", "Error occured while updating", "error");
            });
        };

        //delete Machine record
        $scope.delete = function (deleteId) {
            var def = $q.defer;
            $http({
                method: 'delete',
                url: apiRelPath + '/' + deleteId
            })
            .success(def.resolve)
            .success(function (d) {
                loadMachines();
                swal("Record deleted succussfully");
            })
            .error(function (error) {
                var Message = "Error occured while deleting record";
                if (error.Message)
                    Message = Message + ":\n" + error.Message
                swal("Oops...", Message, "error");
                console.log(error)
            });
        };
    }]);
'use strict'

MachineApp.controller('MachineController', ['$rootScope', '$scope', '$http', 'uiGridConstants',
    '$log', '$timeout', '$q', '$interval', 'ModalService', 'getObjectService',
    function ($rootScope, $scope, $http, uiGridConstants,
    $log, $timeout, $q, $interval, ModalService, getObjectService) {
        $scope.title = "Machine Configuration ";

        var vm = $scope;

        var apiRelPath = "api:/MachineApi";

        var rowIndex;
        var rowId;

        var data = [];
        var machine_id;
        var machine_name;
        var location_name;
        var create_date;
        var modify_date;
        var active;

        var environments = [];
        var applications = [];
        var machineFilter = [];
        var applicationFilter = [];
        var locationFilter = [];
        var filteredMachines = [];
        var filteredApplications = [];
        var filteredLocations = [];

        var selectedApplication;
        var application;
        var selectedEnvironment;
        var environment;
        var selectedMachine;
        var machine;
        var selectedLocation;
        var location;

        var selectedRow;
        var canEdit;
        var bypassEditCancel;

        /// Display current API path and link to Help page
        $scope.ApiBaseUrl = ApiPath;
        $scope.ApiBaseUrlHelp = $scope.ApiBaseUrl.slice(0, -4) + '/Help';
        $scope.currentUser = $rootScope.UserName;
        $scope.displayApi = $rootScope.displayApi;
        $scope.Admin = true;
        //if ($rootScope.Admin === 'True')
        //    $scope.Admin = true;
        //else
        //    $scope.Admin = false;
        if ($rootScope.Engineer === 'True')
            $scope.Engineer = true;
        else
            $scope.Engineer = false;
        $scope.canEdit = false;

        /// Grid Filters
        $scope.environment = '';
        $scope.filterEnvironment = function () {
            return $scope.environment;
        };
        $scope.application = '';
        $scope.dateTimeString = function () {
            date: 'yyyy-MM-dd_HH:mm(Z)'
            return date;
        }
        $scope.selectedRow = "";
        $scope.bypassEditCancel = true;
        $scope.edit = false;
        $scope.canEdit = function () {
            return $scope.edit;
        };
        $scope.machineFilter = [];
        $scope.applicationFilter = [];
        $scope.locationFilter = [];
        $scope.environmentFilter = [];

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

            treeRowHeaderAlwaysVisible: false,

            enableCellSelection: true,
            enableCellEditOnFocus: true,
            enableRowSelection: true,
            enableRowHeaderSelection: false,
            enableMultiselect: false,
        }

        // List of environments:
        $scope.GetEnvironments = function () {
            getObjectService.getConfigObjects('environment')
            .then(function (result) {
                $scope.environments = result;
                if ($scope.environment && $scope.environment != '')
                    angular.forEach($scope.environments, function (environments) {
                        if (environments.value == $scope.environment)
                            $scope.selectedenvironment = environment;
                    });
            })
        };

        // List of applications:
        $scope.GetApplications = function () {
            getObjectService.getConfigObjects('application')
            .then(function (result) {
                $scope.applications = result;
                if ($scope.application && $scope.application != '')
                    angular.forEach($scope.applications, function (application) {
                        if (application.value == $scope.application)
                            $scope.selectedApplication = application;
                    });
            })
        };

        // List of locations:
        $scope.GetLocations = function () {
            getObjectService.getConfigObjects('location')
            .then(function (result) {
                $scope.locations = result;
                if ($scope.location && $scope.location != '')
                    angular.forEach($scope.locations, function (location) {
                        if (location.value == $scope.location)
                            $scope.selectedApplication = location;
                    });
            })
        };

        // List of machines:
        $scope.GetMachines = function () {
            getObjectService.getConfigObjects('machine')
            .then(function (result) {
                $scope.machines = result;
                if ($scope.machine && $scope.machine != '')
                    angular.forEach($scope.machines, function (machine) {
                        if (machine.value == $scope.machine)
                            $scope.selectedApplication = machine;
                    });
            })
        };

        // Function call from Index page dropdown OnChange
        $scope.updateEnvironment = function () {
            $scope.environment = $scope.selectedEnvironment.value;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };

        // Function call from Index page dropdown OnChange
        $scope.updateLocation = function () {
            if (!$scope.selectedLocation) {
                $scope.location = '';
            }
            else {
                $scope.location = $scope.selectedLocation.value;
            }
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };

        // Function call from Index page dropdown OnChange
        $scope.updateMachine = function () {
            if (!$scope.selectedMachine) {
                $scope.machine = '';
            }
            else {
                $scope.machine = $scope.selectedMachine.value;
            }
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };

        // Function call from Index page dropdown OnChange
        $scope.updateApplication = function () {
            if (!$scope.selectedApplication) {
                $scope.application = '';
            }
            else {
                $scope.application = $scope.selectedApplication.value;
            }
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };

        $scope.loadConfigObjects = function () {
            $scope.GetApplications();
            $scope.GetEnvironments();
            $scope.GetLocations();
            $scope.GetMachines();
        };
        $scope.loadConfigObjects();

        //column definitions
        $scope.columnDefinition = function () {
            return [
            { field: 'id', visible: false },
            {
                field: 'environment',
                width: 170,
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
                cellEditableCondition: $scope.canEdit,
            },
            {
                field: 'location',
                //grouping: { groupPriority: 1 },
                sort: { priority: 1, direction: 'asc' },
                groupable: true,
                cellEditableCondition: $scope.canEdit,
                width: 170
            },
            {
                field: 'machine_name',
                width: '20%',
                enableFiltering: true,
                groupable: false,
                //cellTemplate: '/Content/Templates/machineTemplate.html'
            },
            {
                field: 'uri',
                width: '20%',
                enableCellEdit: false,
            },
            {
                field: 'ip_address',
                width: 170,
                enableCellEdit: false,
                //cellEditableCondition: $scope.canEdit,
            },
            { field: 'create_date', enableCellEdit: false, visible: false },
            { field: 'modify_date', enableCellEdit: false, visible: false },
            {
                field: 'active',
                type: 'boolean',
                cellEditableCondition: $scope.canEdit,
                enableFiltering: true,
                width: 80
            },
            { field: 'last_modify_user', visible: false },
            {
                field: "Actions",
                width: 150,
                groupable: false,
                filterable: false,
                sortable: false,
                enableCellEdit: false,
                enableFiltering: false,
                cellTemplate: '/Content/Templates/machineActionsTemplate.html'
            },
            ];
        };

        $scope.loadGridColumns = function () {
            $scope.gridOptions.columnDefs = new Array();
            $scope.gridOptions.columnDefs = $scope.columnDefinition();
        };
        $scope.loadGridColumns();

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
            //$scope.gridApi.core.addRowHeaderColumn({ name: 'rowHeaderCol', displayName: '', width: 26, cellTemplate: '/Content/Templates/expandButtonTemplate.html' });
            gridApi.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {
                if (!oldRowCol || (oldRowCol.row !== newRowCol.row)) {
                    gridApi.expandable.expandRow(newRowCol.row.entity);
                    if (newRowCol.row.entity.environment) {
                        gridApi.selection.clearSelectedRows();
                        gridApi.selection.selectRow(newRowCol.row.entity);
                    }
                    else {
                        gridApi.treeBase.expandRow(newRowCol.row);
                    }
                    if (oldRowCol && (oldRowCol.row.entity.environment || !newRowCol.row.entity.environment)) {
                        gridApi.treeBase.collapseRow(oldRowCol.row);
                        gridApi.expandable.collapseRow(oldRowCol.row);
                    }
                }
                if ($scope.bypassEditCancel === false) {
                    if (newRowCol.row.entity.id !== $scope.machine_id) {
                        $scope.cancelEdit();
                    }
                }
                else if (newRowCol.row.entity.machine_name) {
                    var field = newRowCol.col.colDef.field;
                    if (field !== "treeBaseRowHeaderCol" && field !== "Actions")
                        $scope.machineDetail(newRowCol.row);
                }
                else {
                    gridApi.grid.cellNav.clearFocus();
                    gridApi.grid.cellNav.focusedCells = [];
                    gridApi.grid.cellNav.lastRowCol = null;
                }
                $scope.machine_id = newRowCol.row.entity.id;
            });
            gridApi.rowEdit.on.saveRow($scope, $scope.cancelEdit());
        };

        //$scope.changeGroupBy = function (group1, group2) {
        //    $scope.gridOptions.$gridScope.configGroups = [];
        //    $scope.gridOptions.$gridScope.configGroups.push(group1);
        //    $scope.gridOptions.$gridScope.configGroups.push(group2);
        //    $scope.gridOptions.groupBy();
        //}
        //$scope.clearGroupBy = function () {
        //    $scope.gridOptions.$gridScope.configGroups = [];
        //    $scope.gridOptions.groupBy();
        //}

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
        $scope.loadGrid();

        // Entered the edit row functionality of either the main grid or the expandable grid based on row entity
        $scope.editCell = function (row) {
            $scope.gridApi.grid.cellNav.clearFocus();
            $scope.gridApi.grid.cellNav.focusedCells = [];
            $scope.machine_id = row.entity.machine_id;
            row.grid.appScope.gridApi.grid.cellNav.clearFocus();
            row.grid.appScope.gridApi.grid.cellNav.focusedCells = [];
            $scope.rowId = row.entity.index;
            $scope.edit = true;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            $scope.canEdit();
            if ($scope.selectedRow !== "") {
                $scope.selectedRow.grid.appScope.gridApi.grid.cellNav.clearFocus();
                $scope.selectedRow.grid.appScope.gridApi.grid.cellNav.focusedCells = [];
                $scope.selectedRow.grid.api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            }
            $scope.gridApi.cellNav.scrollToFocus($scope.gridOptions.data[$scope.rowId], row.grid.columns[13]);
            $scope.rowIndex = row.grid.renderContainers.body.visibleRowCache.indexOf(row);
            $scope.selectedRow = row;
            $scope.bypassEditCancel = false;
        };

        // Cancel editable grid option if conditions met:
        //  - Click on a different row
        //  - Cancel button is pressed
        $scope.cancelEdit = function () {
            $scope.edit = false;
            $scope.gridApi.grid.cellNav.clearFocus();
            $scope.gridApi.grid.cellNav.focusedCells = [];
            if ($scope.selectedEntity && $scope.selectedEntity !== $scope.selectedRow.entity) {
                $scope.selectedRow.entity = $scope.selectedEntity;
            }
            var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
            var dataRows = gridRows.map(function (gridRow) {
                return gridRow.entity;
            });
            $scope.gridApi.rowEdit.setRowsClean(dataRows);
            $scope.canEdit();
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
            $scope.bypassEditCancel = true;
        };

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

        // Refresh Grid
        $scope.refreshGrid = function () {
            $scope.gridOptions.data.length = 0;
            $scope.loadConfigObjects();
            $timeout(function () {
                $scope.gridOptions.data = $scope.loadGrid();
                $scope.$apply();
            })
            .then(function () {
                if ($scope.environment !== '' && (!$scope.selectedEnvironment || ($scope.selectedEnvironment.value != $scope.environment))) {
                    angular.forEach($scope.environments, function (environments) {
                        if (environments.value == $scope.environment)
                            $scope.selectedEnvironment = environments;
                    });
                    $scope.updateEnvironment();
                }
                if ($scope.application !== '' && (!$scope.selectedApplication || ($scope.selectedApplication.value != $scope.application))) {
                    angular.forEach($scope.applications, function (applications) {
                        if (applications.value == $scope.application)
                            $scope.selectedApplication = applications;
                    });
                    $scope.updateApplication();
                }
                if ($scope.location !== '' && (!$scope.selectedLocation || ($scope.selectedLocation.value != $scope.location))) {
                    angular.forEach($scope.locations, function (locations) {
                        if (locations.value == $scope.location)
                            $scope.selectedLocation = locations;
                    });
                    $scope.updateLocation();
                }
                if ($scope.machine !== '' && (!$scope.selectedMachine || ($scope.selectedMachine.value != $scope.machine))) {
                    angular.forEach($scope.machines, function (machines) {
                        if (machines.value == $scope.machine)
                            $scope.selectedMachine = machines;
                    });
                    $scope.updateMachine();
                }
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

        // Displays modal window containing the currently selected Component's config elements
        // - Note that this only applies to the first (highest) Grouped object
        // - In our case the Component Name row 
        $scope.machineDetail = function (row) {
            var treeLevel = row.treeLevel;
            var machineName;
            var machineId;
            machineName = row.entity.machine_name;
            machineId = row.entity.id;
            var def = $q.defer();
            //angular.forEach(row.treeNode.aggregations, function (aggregation) {
            //    if (aggregation.col.field === 'componentName')
            //        componentGroupName = aggregation.groupVal;
            //});
            //var deferred = $q.defer();
            //$http({
            //    method: 'GET',
            //    //url: 'api:/IISApi',
            //    url: 'api:/MachineApi',
            //    params: {
            //        id: machineId,
            //    }
            //})
            //.success(deferred.resolve)
            //.success(function (data) {
                ModalService.showModal({
                    templateUrl: "/Content/Templates/machineDetailModal.html",
                    controller: "MachineDetail",
                    inputs: {
                        //machineData: data,
                        machineData: row.entity,
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        modal.close.then(function (result) {
                            var machine;
                        });
                    })
            //})
            //.error(function (error) {
            //    console.log(error);
            //})
            return def.promise
        };

        $scope.getNote = function (row) {
            var localRow = row;
            var machine_id = row.entity.id;
            var machineName = row.entity.machine_name;
            var noteText;
            var def = $q.defer();
            $http({
                method: 'GET',
                url: 'api:/NoteApi',
                params: {
                    noteType: 'machine',
                    id: machine_id,
                    createDate: '1900-01-01'
                }
            })
            .success(def.resolve)
            .success(function (data) {
                var singleData = data[0];
                ModalService.showModal({
                    templateUrl: "/Content/Templates/noteModal.html",
                    controller: "noteViewer",
                    inputs: {
                        header: machineName,
                        varId: machine_id,
                        createDate: singleData.createDate,
                        lastModifiedUser: singleData.userName,
                        lastModifiedDate: singleData.modifyDate,
                        noteText: singleData.noteText,
                        title: machineName,
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        modal.close.then(function (result) {
                            if (result.save) {
                                var deferred = $q.defer();
                                var data = JSON.stringify({
                                    noteId: result.machine_id,
                                    noteType: 'machine',
                                    noteText: result.noteText,
                                    userName: $rootScope.UserName,
                                });
                                $http({
                                    method: 'POST',
                                    url: 'api:/NoteApi/',
                                    data: data,
                                    headers: {
                                        'Content-Type': 'application/json'
                                    }
                                }).success(deferred.resolve)
                                  .success(function () {
                                      localRow.entity.hasNotes = true;
                                  })
                            }
                        });
                    })
            })
            .error(function (error) {
                console.log(error);
            })
            return def.promise;
        };
    }]);
'use strict'

///-------------------------------------------------------------------------------------------------
/// <summary>   ConfigController </summary>
///
/// <remarks>   Pdelosreyes, 4/21/2017. </remarks>
///
/// <param name="'ConfigController'">   The configuration controller'. </param>
/// <param name="($rootScope">          The $root scope. </param>
/// <param name="$scope">               The $scope. </param>
/// <param name="$http">                The $http. </param>
/// <param name="$log">                 The $log. </param>
/// <param name="$timeout">             The $timeout. </param>
/// <param name="uiGridConstants">      The grid constants. </param>
/// <param name="$q">                   The $q. </param>
/// <param name="$interval">            The $interval. </param>
/// <param name="ModalService">         The modal service. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('ConfigController', ['$rootScope', '$scope', '$http', '$log', '$timeout',
    'uiGridConstants', '$q', '$interval', 'ModalService', 'getObjectService',
    function ($rootScope, $scope, $http, $log, $timeout,
    uiGridConstants, $q, $interval, ModalService, getObjectService) {
        $scope.title = "Application Configuration";

        var apiRelPath = "api:/ConfigApi";
        var data = [];
        var i;

        var rowIndex;
        var rowId;

        var edit;
        var var_id;
        var key;
        var value;

        var title;
        var filePath;
        var configXml;
        var componentId;
        var componentName;
        var selectedRow;
        var bypassEditCancel;

        var environments = [];
        var components = [];
        var applications = [];

        var selectedComponent;
        var component;
        var selectedApplication;
        var application;
        var selectedEnvironment;
        var environment;
        var environmentIndex;
        var subGridHeight;

        /// Display current API path and link to Help page
        $scope.ApiBaseUrl = $rootScope.ApiPath;
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

        /// Edit Variables
        $scope.selectedRow = "";
        $scope.key = "";
        $scope.value = "";
        $scope.bypassEditCancel = true;
        $scope.edit = false;
        $scope.canEdit = function () {
            return $scope.edit;
        };

        /// Configure Config UI grid
        $scope.gridOptions = {
            enablePaging: true,
            paginationPageSizes: [10, 20, 50, 100],
            paginationPageSize: 25,
            enableHorizontalScrollbar: 0,

            enablePinning: true,
            showGridFooter: true,
            enableSorting: true,
            enableFiltering: true,
            enableExpandableRowHeader: false,

            saveScroll: true,
            saveGroupingExpandedStates: true,
            saveTreeView: true,
            saveSelection: true,
            saveFocus: true,
            saveOrder: true,
            saveVisible: true,
            saveFilter: true,

            enableGridMenu: true,
            exporterMenuCsv: true,
            exporterMenuPdf: true,
            exporterCsvFilename: 'AppConfig.csv',
            exporterPdfDefaultStyle: { fontSize: 9 },
            exporterPdfTableStyle: { margin: [20, 10, 20, 20] },
            exporterPdfTableHeaderStyle: { fontSize: 10, bold: true, italics: true, color: 'red' },
            exporterPdfHeader: { text: "Marcom Central - Component Configuration", style: 'headerStyle' },
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
            enableRowSelection: false,
            enableRowHeaderSelection: false,
            enableMultiselect: false,

            expandableRowTemplate: '/Content/Templates/expandableRowTemplate.html',
            expandableRowScope: {
                subGridVariable: 'subGridScopeVariable'
            },
        };

        /// Grid Filters
        $scope.environment = '';
        $scope.filterEnvironment = function () {
            return $scope.environment;
        };
        $scope.environmentIndex = -1;
        $scope.filterEnvironmentIndex = function () {
            return $scope.environmentIndex;
        };

        $scope.application = '';
        $scope.filterApplication = function () {
            return $scope.application;
        };

        $scope.component = '';
        $scope.filterComponent = function () {
            return $scope.component;
        };

        // List of environments:
        $scope.GetEnvironments = function () {
            getObjectService.getConfigObjects('environment')
            .then(function (result) {
                $scope.environments = result;
                $scope.subGridHeight = (result.length * 30) + 37;
                $scope.gridOptions.expandableRowHeight = (result.length * 30) + 37;
            })
        };

        // List of components:
        $scope.GetComponents = function () {
            getObjectService.getConfigObjects('component')
            .then(function (result) {
                $scope.components = result;
                if ($scope.component && $scope.component != '')
                    angular.forEach($scope.components, function (components) {
                        if (components.value == $scope.component)
                            $scope.selectedComponent = component;
                    });
            })
        };

        // List of applications:
        $scope.GetApplications = function () {
            getObjectService.getConfigObjects('application')
            .then(function (result) {
                $scope.applications = result;
            })
        };

        $scope.loadConfigObjects = function () {
            $scope.GetApplications();
            $scope.GetComponents();
            $scope.GetEnvironments();
        };
        $scope.loadConfigObjects();

        //$scope.gridOptions.columnDefs = [
        $scope.columnDefinition = function (valueOrdinal) {
            var valueColumn;
            if (valueOrdinal === -1)
                valueColumn = 'a';
            else
                valueColumn = "values[" + valueOrdinal + "].value";
            return [{
                field: 'applicationNames',
                enableCellEdit: false,
                visible: false,
                //visible: true,
                enableFiltering: false,
                filter: {
                    term: '.',
                    condition: function (searchTerm, cellValue) {
                        if ($scope.application !== '') {
                            var debug = cellValue.indexOf($scope.filterApplication()) != -1;
                            var blankCell = (cellValue == '');
                            return debug && !blankCell;
                        }
                        else
                            return true;
                    }
                },
                filterCellFiltered: true,
            },
            { field: 'componentId', visible: false, enableCellEdit: false },
            {
                field: 'componentName',
                enableCellEdit: false,
                width: 200,
                grouping: { groupPriority: 0 },
                sort: { priority: 0, direction: 'asc' },
                groupable: true,
                enableFiltering: false,
                filter: {
                    term: '.',
                    //noTerm: true,
                    condition: function (searchTerm, cellValue) {
                        if ($scope.component !== '')
                            return $scope.filterComponent() === cellValue;
                        else
                            return true;
                    }
                },
                filterCellFiltered: true,
            },
            { field: 'fileName', enableCellEdit: false, visible: false },
            { field: 'configvar_id', visible: false, enableCellEdit: false },
            { field: 'configParentElement', visible: false, enableCellEdit: false },
            { field: 'fullElement', visible: false, enableCellEdit: false },
            { field: 'configElement', visible: false, enableCellEdit: false },
            { field: 'attribute', visible: false, enableCellEdit: false },
            {
                field: 'key',
                cellEditableCondition: $scope.canEdit,
                width: '20%',
                enableFiltering: true,
                //cellToolTip: function (row) {
                //    return row.entity.configElement;
                //},
                visible: true,
                cellToolTip: true,
                cellTemplate: '/Content/Templates/keyTemplate.html'
            },
            { field: 'valueName', visible: false, enableCellEdit: 'false' },
            {
                field: valueColumn,
                displayName: 'value',
                visible: true,
                cellEditableCondition: $scope.canEdit,
                enableFiltering: true,
                cellTemplate: '/Content/Templates/valueTemplate.html'
            },
            { field: 'hasNotes', visible: false, enableCellEdit: 'false' },
            {
                name: "Actions",
                width: 150,
                cellTemplate: '/Content/Templates/actionsTemplate.html',
                enableCellEdit: false,
                visible: true,
                enableFiltering: false
            },
            { field: 'isGlobal', visible: false, enableCellEdit: 'false' },
            ];
        };

        $scope.loadGridColumns = function () {
            $scope.gridOptions.columnDefs = new Array();
            $scope.gridOptions.columnDefs = $scope.columnDefinition($scope.environmentIndex);
        };
        $scope.loadGridColumns();

        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
            $scope.gridApi.core.addRowHeaderColumn({ name: 'rowHeaderCol', displayName: '', width: 26, cellTemplate: '/Content/Templates/expandButtonTemplate.html' });
            gridApi.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {
                if ($scope.bypassEditCancel === false) {
                    if ((!newRowCol.row.entity.key) || newRowCol.row.entity.key == "" || newRowCol.row.entity.configvar_id !== $scope.var_id) {
                        $scope.cancelEdit();
                        if (oldRowCol !== null && oldRowCol !== "undefined") {
                            oldRowCol.row.grid.api.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                        }
                    }
                }
                $scope.var_id = newRowCol.row.entity.configvar_id;
            })
            gridApi.rowEdit.on.saveRow($scope, $scope.cancelEdit());
        };

        $scope.loadGrid = function () {
            var def = $q.defer();
            $http({
                method: 'GET',
                url: apiRelPath,
            })
            .success(def.resolve)
            .success(function (data) {
                for (i = 0; i < data.length; i++) {
                    data[i].subGridOptions = {
                        enableHorizontalScrollbar: 0,
                        enableVerticalScrollbar: 0,
                        appScopeProvider: $scope,
                        enableFiltering: false,
                        enableCellSelection: true,
                        enableCellEditOnFocus: true,
                        enableRowSelection: false,
                        enableRowHeaderSelection: false,
                        enableMultiselect: false,
                        treeRowHeaderAlwaysVisible: false,
                        columnDefs: [
                            { displayName: "id", field: "id", visible: false },
                            { displayName: "Variable id", field: "configvar_id", visible: false },
                            {
                                field: "environment",
                                visible: true,
                                enableFiltering: false,
                                width: 201,
                                enableCellEdit: false
                            },
                            {
                                displayName: "Value",
                                field: "value",
                                visible: true,
                                enableCellEdit: false,
                                enableFiltering: false,
                            },
                            { displayName: "Create Date", field: "create_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                            { displayName: "Modify User", field: "last_modify_user", visible: true, width: 179, enableCellEdit: false, enableFiltering: false },
                            { displayName: "Modify Date", field: "modify_date", visible: true, width: 149, enableCellEdit: false, type: 'date', enableFiltering: false, cellFilter: 'date:"MM-dd-yyyy"' },
                            { displayName: "Last Publish Date", field: "publish_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                            { displayName: "Is Published", field: "published", visible: false, enableCellEdit: false, type: 'boolean' }
                        ],
                        data: data[i].values,
                        onRegisterApi: function (api) {
                            $scope.subGridApi = api;
                        }
                    };
                }
                $scope.gridOptions.data = data;
                angular.forEach(data, function (data, index) {
                    data["index"] = index;
                });
            });
        };

        $scope.loadGrid();

        // Entered the edit row functionality of either the main grid or the expandable grid based on row entity
        $scope.editCell = function (row) {
            $scope.gridApi.grid.cellNav.clearFocus();
            $scope.gridApi.grid.cellNav.focusedCells = [];
            $scope.var_id = row.entity.configvar_id;
            $scope.key = "";
            $scope.value = "";
            row.grid.appScope.gridApi.grid.cellNav.clearFocus();
            row.grid.appScope.gridApi.grid.cellNav.focusedCells = [];
            $scope.rowId = row.entity.index;
            $scope.key = row.entity.key;
            $scope.value = row.entity.values[$scope.environmentIndex].value;
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
            if ($scope.key && $scope.key !== "") {
                $scope.selectedRow.entity.key = $scope.key;
            }
            if ($scope.value && $scope.value !== "") {
                $scope.selectedRow.entity.values[$scope.environmentIndex].value = $scope.value;
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

        // Grid save function
        $scope.saveRow = function (row) {
            var promise = $scope.saveRowFunction(row.entity);
            $scope.gridApi.rowEdit.setSavePromise(row.entity, promise);
            var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
            var dataRows = gridRows.map(function (gridRow) {
                return gridRow.entity;
            });
            $scope.gridApi.rowEdit.setRowsClean(dataRows);
            $scope.edit = false;
            $scope.canEdit();
            $scope.bypassEditCancel = true;
        };
        // Requests the key data save promise
        $scope.saveRowFunction = function (rowEntity) {
            var deferred = $q.defer();
            if (rowEntity.isGlobal)
                angular.forEach(rowEntity.values, function (value) {
                    value.value = rowEntity.values[environmentIndex].value;
                });
            var data = JSON.stringify({
                "configvar_id": rowEntity.configvar_id,
                "applicationNames": rowEntity.applicationNames,
                "componentId": rowEntity.componentId,
                "componentName": rowEntity.componentName,
                "fileName": rowEntity.fileName,
                "configParentElement": rowEntity.configParentElement,
                "fullElement": rowEntity.fullElement,
                "configElement": rowEntity.configElement,
                "attribute": rowEntity.attribute,
                "key": rowEntity.key,
                "valueName": rowEntity.valueName,
                "userName": $rootScope.UserName,
                "values": rowEntity.values,
            });
            $http({
                method: 'POST',
                url: apiRelPath,
                data: data,
                //data: rowEntity,
                headers: {
                    'Content-Type': 'application/json'
                }
            })
            .success(deferred.resolve)
            .error(deferred.reject)
            .error(function () {
                $scope.cancelEdit();
            });
            return deferred.promise;
        };

        // Function call from Index page dropdown OnChange
        $scope.updateEnvironment = function () {
            if (!$scope.selectedEnvironment) {
                $scope.environment = '';
                $scope.environmentIndex = -1;
                $scope.filterEnvironment();
                $scope.filterEnvironmentIndex();
            }
            else {
                $scope.environment = $scope.selectedEnvironment.value;
                angular.forEach($scope.environments, function (values, index) {
                    if (values.name === $scope.environment)
                        $scope.environmentIndex = index;
                });
                $scope.filterEnvironment();
                $scope.filterEnvironmentIndex();
            };
            $scope.loadGridColumns();
        };

        // Function call from Index page dropdown OnChange
        $scope.updateComponent = function () {
            if (!$scope.selectedComponent) {
                $scope.component = '';
                $scope.gridOptions.columnDefs[2].visible = true;
            }
            else {
                $scope.component = $scope.selectedComponent.value;
                $scope.gridOptions.columnDefs[2].visible = false;
                $scope.expandValues();
            }
            //$scope.filterComponent;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };
        // Function call from Index page dropdown OnChange
        $scope.updateApplication = function () {
            if (!$scope.selectedApplication)
                $scope.application = '';
            else {
                $scope.application = $scope.selectedApplication.value;
                $scope.selectedComponent = null;
                $scope.updateComponent();
            }
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        };

        $scope.refreshGrid = function () {
            $scope.gridOptions.data.length = 0;
            $scope.loadConfigObjects();
            $timeout(function () {
                $scope.gridOptions.data = $scope.loadGrid();
                $scope.$apply();
            })
            .then(function () {
                if ($scope.component !== '' && (!$scope.selectedComponent || ($scope.selectedComponent.value != $scope.component))) {
                    angular.forEach($scope.components, function (components) {
                        if (components.value == $scope.component)
                            $scope.selectedComponent = components;
                    });
                    $scope.updateComponent();
                }
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
            });
        };

        $scope.expandValues = function () {
            var groupRow;
            angular.forEach($scope.gridApi.grid.treeBase.tree, function (value, key) {
                if (value.aggregations[0].groupVal == $scope.component) {
                    groupRow = value.row;
                    $scope.gridApi.treeBase.expandRow(value.row);
                }
            });
            var temp = groupRow;
        };

        // Bring up the Add / Edit Component Modal
        //  including file upload
        $scope.addComponent = function () {
            ModalService.showModal({
                templateUrl: "/Content/Templates/addComponentModal.html",
                controller: "AddComponent",
                inputs: {
                    components: $scope.components,
                    applications: $scope.applications,
                    environments: $scope.environments,
                    environment: $scope.selectedEnvironment,
                }
            })
            .then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    if (result.save) {
                        var applications = [];
                        angular.forEach(result.componentApplications, function (app) {
                            applications.push({
                                id: app.id,
                                name: app.name,
                                last_modify_user: $rootScope.UserName,
                            });
                        });
                        var deferred = $q.defer();
                        var data = JSON.stringify({
                            "componentName": result.componentName,
                            "applications": applications,
                            "filePath": result.filePath,
                            "isGlobal": result.isGlobal,
                            "last_modify_user": $rootScope.UserName,
                            "published": result.publish,
                        });
                        $http({
                            method: 'POST',
                            url: 'api:/ComponentApi/',
                            data: data,
                            headers: {
                                'Content-Type': 'application/json'
                            }
                        }).success(deferred.resolve)
                        return deferred.promise
                            .then(function () {
                                if (result.environment)
                                    $scope.environment = result.environment.value;
                                $scope.component = result.componentName;
                                if (result.uploaded)
                                    $scope.refreshGrid();
                                else {
                                    $scope.updateComponent();
                                    $scope.updateApplication();
                                    angular.forEach($scope.gridOptions.data, function (row) {
                                        if (result.componentName == row.component) {
                                            row.applicationNames = applicationNames.join();
                                            row.isGlobal = result.isGlobal
                                        }
                                    });
                                    $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                                };
                            })

                    }
                    else {
                        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                    };
                })
            })
        };

        // Bring up the Add / Edit Application Modal
        $scope.addApplication = function () {
            ModalService.showModal({
                templateUrl: "/Content/Templates/addApplicationModal.html",
                controller: "AddApplication",
                bodyClass: "modal-body",
                inputs: {
                    components: $scope.components,
                    applications: $scope.applications,
                    environments: $scope.environments,
                }
            })
            .then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    if (result.save) {
                        var deferred = $q.defer();
                        var components = [];
                        angular.forEach(result.applicationComponents, function (comp) {
                            components.push({
                                id: comp.id,
                                componentName: comp.name,
                                last_modify_user: $rootScope.UserName
                            });
                        });
                        if (typeof result.id === 'undefined')
                            result.id = '';
                        var data = JSON.stringify({
                            "id": result.id,
                            "name": result.applicationName,
                            "components": components,
                            "release": result.release,
                            "last_modify_user": $rootScope.UserName,
                        });
                        $http({
                            method: 'POST',
                            url: 'api:/ApplicationApi/',
                            data: data,
                            headers: {
                                'Content-Type': 'application/json'
                            }
                        }).success(deferred.resolve)
                        return deferred.promise
                          .then(function () {
                              $scope.updateComponent();
                              $scope.updateApplication();
                              angular.forEach($scope.gridOptions.data, function (row) {
                                  if (~result.componentNames.indexOf(row.component))
                                      row.applicationNames += ', ' + result.applicationName
                                  else if (~row.applicationNames.indexOf(result.applicationName))
                                      row.applicationNames.replace(result.applicationName + ',', '');
                              });
                              $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                          });
                    }
                    else {
                        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                    };

                })
            }).catch(function (error) {
                console.log(error);
            })
        };

        // Displays modal to add new config variable
        $scope.addVar = function (row) {
            var componentRow = '$$' + row.uid;
            var parentRows = [];
            var rowEntity = row.entity;
            var treeLevel = row.treeLevel;
            var componentName;
            var componentFileName;
            var firstChild = row.treeNode.children[0].row.entity;
            var firstChildParentElement = firstChild.configParentElement;
            var show;
            var isNew;

            if (typeof firstChild === "undefined") {
                isNew = 1;
                show = 0;
            }
            else {
                firstChildParentElement = firstChild.configParentElement;
                isNew = 0;
                if (firstChild.attribute !== "") {
                    show = 1;
                }
                else {
                    show = 0;
                }
            }
            angular.forEach(row.treeNode.children, function (child) {
                if (parentRows.indexOf(child.row.entity.configParentElement) == -1)
                    parentRows.push(child.row.entity.configParentElement);
            });
            angular.forEach(row.treeNode.aggregations, function (aggregation) {
                if (aggregation.col.field === 'componentName')
                    componentName = aggregation.groupVal;
                if (aggregation.col.field === 'fileName')
                    componentFileName === aggregation.groupVal;
            });
            if (componentName) {
                var def = $q.defer();
                $http({
                    method: 'GET',
                    url: 'api:/ConfigApi',
                    params: {
                        componentName: componentName,
                    }
                })
                .success(def.resolve)
                .success(function (data) {
                    ModalService.showModal({
                        templateUrl: "/Content/Templates/addVariableModal.html",
                        controller: "AddVar",
                        inputs: {
                            componentName: componentName,
                            parentRows: parentRows,
                            parentElement: firstChildParentElement,
                            element: firstChild.configElement,
                            attribute: firstChild.attribute,
                            key: "",
                            valueName: firstChild.valueName,
                            show: show,
                            isNew: isNew,
                            files: data,
                        }
                    })
                        .then(function (modal) {
                            modal.element.modal();
                            modal.close.then(function (result) {
                                if (result.save) {
                                    var deferred = $q.defer();
                                    var fullElement;
                                    if (result.value_name === "")
                                        fullElement = "<" + result.element + ">"
                                                        + "{value}</"
                                                        + result.element + ">";
                                    else if (result.attribute == result.key)
                                        fullElement = "<" + result.element + " "
                                                        + result.attribute + "=\""
                                                        + "{value}\" />";
                                    else
                                        fullElement = "<" + result.element + " "
                                                        + result.attribute + "=\""
                                                        + result.key + "\" "
                                                        + result.valueName + "=\""
                                                        + "{value}\" />";
                                    var data = JSON.stringify({
                                        "componentId": firstChild.componentId,
                                        "componentName": result.componentName,
                                        "applicationNames": '',
                                        "fileName": result.fileName,
                                        "configParentElement": result.parentElement,
                                        "fullElement": fullElement,
                                        "configElement": result.element,
                                        "attribute": result.attribute,
                                        "key": result.key,
                                        "valueName": result.valueName,
                                        "values": [],
                                        "last_modify_user": $rootScope.UserName,
                                    });
                                    $http({
                                        method: 'POST',
                                        url: 'api:/ConfigApi/',
                                        data: data,
                                        headers: {
                                            'Content-Type': 'application/json'
                                        }
                                    }).success(deferred.resolve)
                                      .success(function () {
                                          $scope.refreshGrid()
                                      })
                                    return deferred.promise;
                                }
                                else {
                                    $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                                }
                            });
                            return def.promise;
                        })
                })
            }
        };

        // Displays modal window containing the currently selected Component's config elements
        // - Note that this only applies to the first (highest) Grouped object
        // - In our case the Component Name row 
        $scope.showFile = function (row) {
            var treeLevel = row.treeLevel;
            var componentGroupName;
            var componentFileName;
            angular.forEach(row.treeNode.aggregations, function (aggregation) {
                if (aggregation.col.field === 'componentName')
                    componentGroupName = aggregation.groupVal;
                if (aggregation.col.field === 'fileName')
                    componentFileName === aggregation.groupVal;
            });
            var def = $q.defer();
            $http({
                method: 'GET',
                url: 'api:/ConfigApi',
                params: {
                    componentName: componentGroupName,
                }
            })
            .success(def.resolve)
            .success(function (data) {
                ModalService.showModal({
                    templateUrl: "/Content/Templates/configFileModal.html",
                    controller: "ConfigViewer",
                    inputs: {
                        component: componentGroupName,
                        files: data,
                        environments: $scope.environments,
                        environment: $scope.environment,
                        Admin: $scope.Admin,
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        modal.close.then(function (result) {
                            var fileEnvironment;
                            if (result.environment === '')
                                fileEnvironment = $scope.environment;
                            else {
                                fileEnvironment = result.environment;
                                $scope.environment = fileEnvironment;
                            }
                            if (result.download) {
                                $scope.downloadConfig(result.component, result.fileName, fileEnvironment)
                            };
                            if (result.publish) {
                                $scope.downloadConfig(result.component, result.fileName, fileEnvironment)
                            };
                        });
                    })
            })
            .error(function (error) {
                console.log(error);
            })
            return def.promise
        };


        $scope.getNote = function (row) {
            var localRow = row;
            var configVarId = row.entity.configvar_id;
            var key = row.entity.key;
            var fullElement = row.entity.fullElement;
            var componentName = row.entity.componentName;
            var componentFileName = row.entity.fileName;
            var noteText;
            var def = $q.defer();
            $http({
                method: 'GET',
                url: 'api:/NoteApi',
                params: {
                    noteType: 'configvariables',
                    id: configVarId,
                    createDate: '1900-01-01'
                    //componentName: componentName,
                }
            })
            .success(def.resolve)
            .success(function (data) {
                var singleData = data[0];
                ModalService.showModal({
                    templateUrl: "/Content/Templates/noteModal.html",
                    controller: "noteViewer",
                    inputs: {
                        componentName: componentName,
                        key: key,
                        fullElement: fullElement,
                        configVarId: configVarId,
                        createDate: singleData.createDate,
                        lastModifiedUser: singleData.userName,
                        lastModifiedDate: singleData.modifyDate,
                        noteText: singleData.noteText,
                        //createDate: data.createDate,
                        //lastModifiedUser: data.userName,
                        //lastModifiedDate: data.modifyDate,
                        //noteText: data.noteText,
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        modal.close.then(function (result) {
                            if (result.save) {
                                var deferred = $q.defer();
                                var data = JSON.stringify({
                                    noteId: result.configVarId,
                                    noteType: 'configvariables',
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

        // Config File Download
        $scope.downloadConfig = function (componentName, fileName) {
            $scope.downloadFile(componentName, fileName, $scope.environment);
        };

        $scope.downloadFile = function (componentName, fileName, environment) {
            var def = $q.defer();
            $http({
                method: 'GET',
                url: 'api:/ConfigPublishApi',
                params: {
                    componentName: componentName,
                    environment: environment,
                    fileName: fileName,
                },
                responseType: 'arraybuffer'
            })
                .success(def.resolve)
                .success(function (data, status, headers) {
                    headers = headers();

                    var filename = headers['x-filename'];
                    var contentType = headers['content-type'];

                    var linkElement = document.createElement('a');
                    try {
                        var blob = new Blob([data], { type: contentType });
                        var url = window.URL.createObjectURL(blob);

                        linkElement.setAttribute('href', url);
                        linkElement.setAttribute('download', filename);
                        var clickEvent;

                        //This is true only for IE,firefox
                        if (document.createEvent) {
                            // To create a mouse event , first we need to create an event and then initialize it.
                            clickEvent = document.createEvent("MouseEvent");
                            clickEvent.initMouseEvent("click", true, true, window, 0, 0, 0, 0, 0, false, false, false, false, 0, null);
                        }
                        else {
                            clickEvent = new MouseEvent('click', {
                                'view': window,
                                'bubbles': true,
                                'cancelable': true
                            });
                        }
                        linkElement.dispatchEvent(clickEvent)
                    } catch (ex) {
                        console.log(ex);
                    }
                })
            return def.promise;
        };

        $scope.publishApplication = function (selectedApplication) {
            if (!selectedApplication) {
                swal({
                    title: "Publish Application",
                    text: "No Application Selected",
                    type: "error",
                    confirmButtonText: "D'oh!"
                });
            }
            else if (!$scope.selectedEnvironment) {
                swal({
                    title: "Publish Application",
                    text: "No Environment Selected",
                    type: "error",
                    confirmButtonText: "D'oh!"
                });
            }
            else {
                var componentNames = [];
                var componentNameString;
                var def = $q.defer();
                $http({
                    method: 'GET',
                    url: "api:/ApplicationApi/",
                    params: {
                        applicationName: selectedApplication.name,
                    },
                })
                    .success(def.resolve)
                return def.promise
                    .then(function (result) {
                        angular.forEach(result.Components, function (Components) {
                            var name = Components.component_name;
                            componentNames.push({
                                name: name.replace(/\[/g, '').replace(/]/g, '')
                                    .replace(/"/g, '').replace(/'/g, ''),
                            });
                        });
                        componentNames = componentNames.map(function (item) {
                            return item['name'];
                        });

                        componentNameString = ' - ' + componentNames.join().replace(/,/g, '\n - ');
                        swal({
                            title: "Are you sure?",
                            text: "Existing Component Files:\n" + componentNameString + "\nwill be overwritten in " + $scope.environment,
                            type: "warning",
                            showCancelButton: true,
                            confirmButtonColor: "#AEDEF4",
                            confirmButtonText: "Yes, upload it!",
                            closeOnConfirm: false
                        },
                        function (isConfirm) {
                            if (isConfirm) {
                                var def = $q.defer();
                                $http({
                                    method: 'POST',
                                    url: 'api:/ConfigPublishApi',
                                    params: {
                                        applicationId: selectedApplication.id,
                                        environment: $scope.environment,
                                        userName: $rootScope.UserName,
                                    },
                                })
                                .success(def.resolve)
                                .success(
                                    swal({
                                        title: "Publish" + $scope.application,
                                        text: "Application Component Files:\n" + componentNameString + "\nwere successfully published",
                                        type: "success",
                                        imageUrl: "/Assets/thumbs-up.jpg",
                                        confirmButtonText: "Cool"
                                    }))
                                .error(def.reject)
                                .error(
                                    swal({
                                        title: "Publish" + $scope.application,
                                        text: "Application ComponentFiles:\n" + componentNameString + "\nwere not published",
                                        type: "error",
                                        confirmButtonText: "D'oh!"
                                    }))
                                return def.promise;
                            };
                        })
                    })
            }
        };
    }]);

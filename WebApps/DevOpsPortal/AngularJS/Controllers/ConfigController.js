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
ConfigApp.controller('ConfigController', function ($rootScope, $scope, $http, $log, $timeout,
    uiGridConstants, $q, $interval, ModalService) {
    $scope.title = "Application Configuration";

    var apiRelPath = "api:/ConfigApi";
    var data = [];
    var i;

    var rowIndex;
    var rowId;
    var subGridRowId;
    var parentRow;

    var edit;
    var subEdit;
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

    /// Display current API path and link to Help page
    //if ($scope.APIPath.Includes('/api'))
    //if (ApiPath.Includes('/api'))
    //    $scope.ApiBaseUrl = ApiPath.slice(0, -4);
    //else
    //$scope.ApiBaseUrl = $rootScope.APIPath;
    $scope.ApiBaseUrl = ApiPath;
    //if ($scope.ApiBaseUrl.endsWith('/'))
    //    $scope.ApiBaseUrlHelp = ApiBaseUrl(0, -1) + 'Help';
    //else
    $scope.ApiBaseUrlHelp = $scope.ApiBaseUrl.slice(0, -4) + '/Help';

    /// Edit Variables
    $scope.selectedRow = "";
    $scope.key = "";
    $scope.value = "";
    $scope.bypassEditCancel = true;
    $scope.edit = false;
    $scope.canEdit = function () {
        return $scope.edit;
    };
    $scope.subEdit = false;
    $scope.subCanEdit = function () {
        return $scope.subEdit;
    };

    /// Grid Filters
    $scope.environment = 'development';
    $scope.filterEnvironment = function () {
        return $scope.environment;
    };
    $scope.application = '';
    $scope.component = '';

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
        expandableRowHeight: 64,
        expandableRowScope: {
            subGridVariable: 'subGridScopeVariable'
        },
    };

    $scope.gridOptions.columnDefs = [
        {
            field: 'applicationNames',
            enableCellEdit: false,
            filter: {
                condition: uiGridConstants.filter.CONTAINS,
                term: $scope.application
            },
            filterCellFiltered: true,
        },
        { field: 'componentId', visible: false, enableCellEdit: false },
        {
            field: 'componentName',
            enableCellEdit: false,
            grouping: { groupPriority: 0 },
            sort: { priority: 0, direction: 'asc' },
            groupable: true,
            filter: {
                condition: uiGridConstants.filter.CONTAINS,
                term: $scope.component
            },
            filterCellFiltered: true
        },
        {
            field: 'fileName',
            enableCellEdit: false,
            width: '10%',
            grouping: { groupPriority: 1 },
            sort: { priority: 1, direction: 'asc' },
            groupable: true
        },
        { field: 'configvar_id', visible: false, enableCellEdit: false },
        //{ field: 'configParentElement', visible: false, enableCellEdit: false },
        { field: 'configParentElement', visible: true, enableCellEdit: false },
        { field: 'configElement', visible: false, enableCellEdit: false },
        { field: 'attribute', visible: false, enableCellEdit: false },
        {
            field: 'key',
            cellEditableCondition: $scope.canEdit,
            width: "50%",
            filter: {
                condition: function (searchTerm, cellValue) {
                    let result = true;
                    result = '' !== cellValue;
                    return result;
                },
                noTerm: true
            },
            filterCellFiltered: true,
            cellToolTip: function (row, col) {
                if (grid.appScope.isBlank(row.entity.attribute)) {
                    return '<' + row.entity.configParentElement + '><br>&nbsp;<' + row.entity.key + '> {value} </' + row.entity.key + '><br>&nbsp;. . .<br></' + row.entity.configParentElement + '>';
                }
                else {
                    return '<' + row.entity.configParentElement + ' . . . /><br>&nbsp; <' + row.entity.configElement + '&nbsp;' + row.entity.attribute + '="{value}"&nbsp;' + row.entity.valueName + '="{value}" /><br>&nbsp;. . .<br></' + row.entity.configParentElement + ' . . .>" tooltip-placement="right" ';
                }
            }
        },
        { field: 'valueName', visible: false, enableCellEdit: 'false' },
        {
            name: "Actions",
            width: 150,
            cellTemplate: '/Content/Templates/actionsTemplate.html',
            enableCellEdit: false,
            visible: true,
            enableFiltering: false
        }
    ];

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        gridApi.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {
            if ($scope.bypassEditCancel === false) {
                if (newRowCol.row.entity.key === "undefined" || newRowCol.row.entity.configvar_id !== $scope.var_id || $scope.subEdit === true) {
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

    $http.get(ApiPath + '/ConfigApi')
    //$http.get('api:/ConfigApi')
    .success(function (data) {
        for (i = 0; i < data.length; i++) {
            data[i].subGridOptions = {
                enableHorizontalScrollbar: 0,
                enableVerticalScrollbar: 0,
                appScopeProvider: $scope,
                enableFiltering: true,
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
                        filter: {
                            noTerm: true,
                            condition: function (searchTerm, cellValue) {
                                return $scope.filterEnvironment() === cellValue;
                            }
                        },
                        filterCellFiltered: true,
                        enableCellEdit: false
                    },
                    {
                        displayName: "Value",
                        field: "value",
                        visible: true,
                        cellEditableCondition: $scope.subCanEdit,
                        enableFiltering: false, width: "70%"
                    },
                    { displayName: "Create Date", field: "create_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                    { displayName: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, type: 'date', enableFiltering: false, cellFilter: 'date:"MM-dd-yyyy"' },
                    { displayName: "Last Publish Date", field: "publish_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                    { displayName: "Is Published", field: "published", visible: false, enableCellEdit: false, type: 'boolean' },
                    {
                        name: "Actions",
                        cellTemplate: '/Content/Templates/subGridActionsTemplate.html',
                        enableCellEdit: false,
                        width: 149,
                        visible: true,
                        enableFiltering: false
                    },
                ],
                data: data[i].values,
                onRegisterApi: function (api) {
                    $scope.subGridApi = api;
                    api.cellNav.on.navigate($scope, function (newRowCol, oldRowCol) {
                        if ($scope.bypassEditCancel === false) {
                            if (newRowCol.row.entity.environment !== $scope.environment || newRowCol.row.entity.configvar_id !== $scope.var_id || $scope.edit === true) {
                                $scope.cancelEdit();
                                if (oldRowCol !== null && oldRowCol !== "undefined") {
                                    oldRowCol.row.grid.api.core.notifyDataChange(uiGridConstants.dataChange.ALL);
                                }
                            }
                        }
                        $scope.var_id = newRowCol.row.entity.configvar_id;
                    });
                    api.rowEdit.on.saveRow($scope, $scope.cancelEdit());
                }
            };
        }
        $scope.gridOptions.data = data;
        angular.forEach(data, function (data, index) {
            data["index"] = index;
        });
    });

    // Filters the subgrid based on the Selected Environment
    //  note - does not refresh currently visible sub grid rows that are not the last one expanded / selected
    $scope.filterSubGrid = function (value) {
        console.log(value);
        if (typeof $scope.gridApi.grid.appScope.subGridApi !== 'undefined') {
            $scope.gridApi.grid.appScope.subGridApi.grid.columns[2].filters[0].term = value;
            $scope.subGridApi.core.refresh();
        }
        //angular.forEach($scope.gridOptions.data, function (data) {
        //    data.values.subGridOptions;
        //});
    };

    // Entered the edit row functionality of either the main grid or the expandable grid based on row entity
    $scope.editCell = function (row) {
        $scope.gridApi.grid.cellNav.clearFocus();
        $scope.gridApi.grid.cellNav.focusedCells = [];
        $scope.var_id = row.entity.configvar_id;
        $scope.key = "";
        $scope.value = "";
        // For App Keys
        if (typeof row.entity.key !== "undefined") {
            row.grid.appScope.gridApi.grid.cellNav.clearFocus();
            row.grid.appScope.gridApi.grid.cellNav.focusedCells = [];
            $scope.rowId = row.entity.index;
            $scope.key = row.entity.key;
            $scope.edit = true;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            $scope.canEdit();
            if ($scope.selectedRow !== "") {
                $scope.selectedRow.grid.appScope.gridApi.grid.cellNav.clearFocus();
                $scope.selectedRow.grid.appScope.gridApi.grid.cellNav.focusedCells = [];
                $scope.selectedRow.grid.api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            }
            $scope.gridApi.cellNav.scrollToFocus($scope.rowId, 8);
            $scope.rowIndex = row.grid.renderContainers.body.visibleRowCache.indexOf(row);
        }
            // For Values
        else {
            row.grid.appScope.subGridApi.grid.cellNav.clearFocus();
            row.grid.appScope.subGridApi.grid.cellNav.focusedCells = [];
            angular.forEach(row.grid.rows, function (rows, index) {
                data["index"] = index;
                if (rows.entity.environment === row.entity.environment) {
                    $scope.subGridRowId = index;
                }
            });
            $scope.subGridApi.cellNav.scrollToFocus($scope.subGridRowId, 4);
            $scope.value = row.entity.value;
            $scope.subEdit = true;
            $scope.subCanEdit();
            if ($scope.selectedRow !== "") {
                $scope.selectedRow.grid.appScope.subGridApi.grid.cellNav.clearFocus();
                $scope.selectedRow.grid.appScope.subGridApi.grid.cellNav.focusedCells = [];
                $scope.selectedRow.grid.api.core.notifyDataChange(uiGridConstants.dataChange.ALL);
            }
            $scope.subGridApi.grid.appScope.subGridApi.cellNav.scrollToFocus($scope.subGridRowId, 4);
            //row.grid.appScope.subGridApi.cellNav.scrollToFocus($scope.subGridRowId, 4);
            //row.grid.api.cellNav.scrollToFocus(row.grid.rows[$scope.subGridRowId], 4);
            $scope.rowIndex = row.grid.renderContainers.body.visibleRowCache.indexOf(row);
            row.grid.appScope.subGridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            row.grid.api.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        }
        $scope.selectedRow = row;
        $scope.bypassEditCancel = false;
    };

    // Cancel editable grid option if conditions met:
    //  - Click on a different row
    //  - Cancel button is pressed
    $scope.cancelEdit = function () {
        $scope.edit = false;
        $scope.subEdit = false;
        $scope.gridApi.grid.cellNav.clearFocus();
        $scope.gridApi.grid.cellNav.focusedCells = [];
        if ($scope.key !== "") {
            $scope.selectedRow.entity.key = $scope.key;
            var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
            var dataRows = gridRows.map(function (gridRow) {
                return gridRow.entity;
            });
            $scope.gridApi.rowEdit.setRowsClean(dataRows);
            $scope.canEdit();
            $scope.selectedRow.grid.api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
        }
        else if ($scope.value !== "") {
            $scope.selectedRow.entity.value = $scope.value;
            var subGridRows = $scope.subGridApi.rowEdit.getDirtyRows();
            var subDataRows = subGridRows.map(function (gridRow) {
                return gridRow.entity;
            })
            $scope.selectedRow.grid.api.rowEdit.setRowsClean(subDataRows);
            $scope.canEdit();
            $scope.selectedRow.grid.api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
        }
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        $scope.bypassEditCancel = true;
    };

    // Grid save function
    $scope.saveRow = function (rowEntity) {
        var promise = $scope.saveRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
        var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
        var dataRows = gridRows.map(function (gridRow) {
            return gridRow.entity;
        });
        $scope.gridApi.rowEdit.setRowsClean(dataRows);
        $scope.edit = false;
        $scope.subEdit = false;
        $scope.canEdit();
        $scope.subCanEdit();
        $scope.bypassEditCancel = true;
    };
    // Requests the key data save promise
    $scope.saveRowFunction = function (rowEntity) {
        var deferred = $q.defer();
        //$http.post(ApiPath + '/api/ConfigApi/', rowEntity).success(deferred.resolve).error(deferred.reject);
        $http.post(apiRelPath, rowEntity).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    // Sub Grid save function
    $scope.saveSubGridRow = function (rowEntity) {
        var promise = $scope.saveSubGridRowFunction(rowEntity);
        $scope.selectedRow.grid.api.rowEdit.setSavePromise(rowEntity, promise);
        $scope.edit = false;
        $scope.subEdit = false;
        $scope.canEdit();
        $scope.subCanEdit();
        var gridRows = $scope.selectedRow.grid.api.rowEdit.getDirtyRows();
        var dataRows = gridRows.map(function (gridRow) {
            return gridRow.entity;
        });
        $scope.selectedRow.grid.api.rowEdit.setRowsClean(dataRows);
        $scope.gridOptions.data[$scope.rowId].subGridOptions.data[$scope.subGridRowId].editable = false;
        //rowEntity.editable = false;
        $scope.bypassEditCancel = true;
    };
    // Requests the value Save Promise
    $scope.saveSubGridRowFunction = function (rowEntity) {
        var deferred = $q.defer();
        //$http.post(ApiPath + '/api/ConfigValuesApi/', rowEntity).success(deferred.resolve).error(deferred.reject);
        $http.post('api:/ConfigValuesApi/', rowEntity).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    // List of environments:
    $http({
        method: 'GET',
        url: 'api:/ConfigValuesApi',
        //url: '/Config/GetDropDownValues',
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
        //url: '/Config/GetDropDownValues',
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
        //url: '/Config/GetDropDownValues',
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
        $scope.filterSubGrid($scope.environment);
    };
    // Function call from Index page dropdown OnChange
    $scope.updateComponent = function () {
        $scope.component = $scope.selectedComponent;
        $scope.gridApi.grid.refresh();
    };
    // Function call from Index page dropdown OnChange
    $scope.updateApplication = function () {
        $scope.application = $scope.selectedApplication;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
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
            }
        })
        .then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result.save) {
                    var deferred = $q.defer();
                    var applicationNames = result.componentApplications.map(function (item) { return item["id", "name"]; });
                    applicationNames = result.componentApplications;
                    var data = JSON.stringify({
                        "componentName": result.componentName,
                        "applications": applicationNames,
                        "filePath": result.filePath,
                    });
                    $http({
                        method: 'POST',
                        url: 'api:/ComponentApi/',
                        //withCredentials: true,
                        data: data,
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }).success(deferred.resolve)
                            .error(deferred.reject);
                    return deferred.promise;
                };
            });
        }).catch(function (error) {
            console.log(error);
        })
    };

    // Bring up the Add / Edit Component Modal
    //  including file upload
    $scope.addApplication = function () {
        ModalService.showModal({
            templateUrl: "/Content/Templates/addApplicationModal.html",
            controller: "AddApplication",
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
                    //var componentNames = result.applicationComponents.map(function (item) { return item["id", "name"]; });
                    //applicationNames = result.componentApplications;
                    var data = JSON.stringify({
                        //"id": result.id,
                        "name": result.applicationName,
                        //"components": componentNames,
                        "release": result.release,
                    });
                    $http({
                        method: 'POST',
                        //url: ApiPath + '/api/ComponentApi/',
                        url: 'api:/ApplicationApi/',
                        //withCredentials: true,
                        data: data,
                        headers: {
                            'Content-Type': 'application/json'
                        }
                    }).success(deferred.resolve)
                            .error(deferred.reject);
                    return deferred.promise;
                };
            });
        }).catch(function (error) {
            console.log(error);
        })
    };

    // Displays modal to add new config variable
    $scope.addVar = function (row) {
        var componentRow = '$$' + row.uid;
        var rowEntity = row.entity;
        var componentAggObject = row.treeNode.aggregations[0].groupVal;
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

        ModalService.showModal({
            templateUrl: "/Content/Templates/addVariableModal.html",
            controller: "AddVar",
            inputs: {
                componentName: componentAggObject,
                parentElement: firstChildParentElement,
                applicationNames: firstChild.applicationNames,
                element: "",
                keyName: "",
                key: "",
                valueName: "",
                save: false,
                show: show,
                isNew: isNew,
            }
        })
            .then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                    if (result.save) {
                        var deferred = $q.defer();
                        var data = JSON.stringify({
                            "componentId": firstChild.componentId,
                            "componentName": result.componentName,
                            "applicationNames": firstChild.applicationNames,
                            "configParentElement": result.parentElement,
                            "configElement": result.element,
                            "attribute": result.keyName,
                            "key": result.key,
                            "valueName": result.valueName,
                            "values": []
                        });
                        $http({
                            method: 'POST',
                            //url: ApiPath + '/api/ComponentApi/',
                            url: 'api:/ComponentApi/',
                            //withCredentials: true,
                            data: data,
                            headers: {
                                'Content-Type': 'application/json'
                            }
                        }).success(deferred.resolve)
                                .error(deferred.reject);
                        return deferred.promise;
                    };
                });
            }).catch(function (error) {
                console.log(error);
            })
    };

    // Displays modal window containing the currently selected Component's config elements
    // - Note that this only applies to the first (highest) Grouped object
    // - In our case the Component Name row 
    $scope.showFile = function (row) {
        var treeLevel = row.treeLevel;
        //var componentGroupName = row.entity.$$uiGrid-000A.groupVal;
        var componentGroup = row.treeNode.parentRow.entity['$$uiGrid-0009'];
        var componentGroupName = componentGroup.groupVal;
        var componentFileName = row.treeNode.aggregations[1].groupVal;
        //var def = $q.defer();
        if (typeof componentFileName !== 'undefined')
            $http.get('api:/ConfigApi?componentName=' + componentGroupName + '&environment=' + $scope.environment + '&fileName=' + componentFileName)
        //$http.get('api:/ConfigApi?componentName=' + componentGroupName + '&environment=' + $scope.environment)
            .success(function (data) {
                ModalService.showModal({
                    templateUrl: "/Content/Templates/configFileModal.html",
                    controller: "ConfigViewer",
                    inputs: {
                        title: data.componentName,
                        filePath: data.path,
                        fileName: data.fileName,
                        configXml: data.text,
                        publish: false,
                        download: false
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        modal.close.then(function (result) {
                            if (result.download) {
                                $scope.downloadConfig(result.title, result.fileName)
                            };
                            if (result.publish) {
                                $scope.downloadConfig(result.title, result.fileName)
                            };
                        });
                    })
                //def.resolve(data);
            })
        //.error(function () {
        //    def.reject("Failed to get Config File.")
        //});
        //return def.promise;
    };

    // Config File Download
    $scope.downloadConfig = function (componentName, fileName) {
        $scope.downloadFile(componentName, fileName, $scope.environment);
    };

    $scope.downloadFile = function (componentName, fileName, environment) {
        $http({
            method: 'GET',
            url: 'api:/ConfigPublishApi',
            //url: apiPath + '/ConfigPublishApi',
            //withCredentials: true,
            params: {
                componentName: componentName,
                environment: environment,
                fileName: fileName,
            },
            responseType: 'arraybuffer'
        }).success(function (data, status, headers) {
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
                linkElement.dispatchEvent(clickEvent);
            } catch (ex) {
                console.log(ex);
            }
        }).error(function (data) {
            console.log(data);
        });
    }
});


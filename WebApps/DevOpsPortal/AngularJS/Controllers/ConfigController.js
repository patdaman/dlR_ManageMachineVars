﻿'use strict'

var ConfigApp = angular.module('ConfigApp',
        ['ui.grid',
            'ui.grid.edit',
            'ui.grid.grouping',
            'ui.grid.saveState',
            'ui.grid.pagination',
            'ui.grid.expandable',
            'ui.grid.cellNav',
            'ui.grid.selection',
            'ui.grid.rowEdit',
            'ui.grid.resizeColumns',
            'ui.grid.pinning',
            'ui.grid.exporter',
            'ui.grid.moveColumns',
            'ui.grid.infiniteScroll',
            'ui.grid.importer',
            'ui.router',
            'angularModalService',
            'ngAnimate',
            'ui.bootstrap',
            'ngClickCopy'
            , 'ngFileUpload'
        ])

    .config(['$stateProvider',
    function ($stateProvider) {
        $stateProvider
            .state('Authorised', {
                url: '/Authorised',
                templateUrl: './Views/Authorised.html',
                controller: 'customerController'
            })
            .state('Restricted', {
                url: '/Restricted',
                templateUrl: './Views/Restricted.html',
                controller: 'androidController'
            })
        ;
    }]);

ConfigApp.controller('ConfigController', function ($rootScope, $scope, $http, $log, $timeout,
    uiGridConstants, $q, $interval, ModalService) {
    $scope.title = "Application Configuration";

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

    $scope.selectedRow = "";
    $scope.key = "";
    $scope.value = "";
    $scope.bypassEditCancel = true;

    $scope.environment = 'development';
    $scope.filterEnvironment = function () {
        return $scope.environment;
    };

    $scope.application = '';
    $scope.filterApplication = function () {
        return $scope.application;
    };

    $scope.component = '';
    $scope.filterComponent = function () {
        return $scope.component;
    };

    $scope.edit = false;
    $scope.canEdit = function () {
        return $scope.edit;
    };

    $scope.subEdit = false;
    $scope.subCanEdit = function () {
        return $scope.subEdit;
    };

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

        expandableRowTemplate: 'Content/Templates/expandableRowTemplate.html',
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
        { field: 'configvar_id', visible: false, enableCellEdit: false },
        { field: 'configParentElement', visible: false, enableCellEdit: false },
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
            cellTemplate: 'Content/Templates/actionsTemplate.html',
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

    $http.get('/api/ConfigApi')
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
                        cellTemplate: 'Content/Templates/subGridActionsTemplate.html',
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
        $scope.gridApi.grid.appScope.subGridApi.grid.columns[2].filters[0].term = value;
        //angular.forEach($scope.gridOptions.data, function (data) {
        //    data.values.subGridOptions;
        //});
        $scope.subGridApi.core.refresh();
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
            $scope.gridApi.cellNav.scrollToFocus($scope.rowId, 7);
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
        $http.post('/api/ConfigApi/', rowEntity).success(deferred.resolve).error(deferred.reject);
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
        $http.post('/api/ConfigValuesApi/', rowEntity).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    // List of environments:
    $http({
        method: 'GET',
        url: '/api/ConfigValuesApi/',
        //withCredentials: true,
        params: {
            type: "environment"
        },
        //responseType: 'arraybuffer'
    }).then(function (result) {
        $scope.environments = result.data;
    });

    // List of components:
    $http({
        method: 'GET',
        url: '/api/ConfigValuesApi/',
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
        url: '/api/ConfigValuesApi/',
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
            templateUrl: "Content/Templates/addComponentModal.html",
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
                    var applicationNames = result.componentApplications.map(function (item) { return item["id","name"]; });
                    var data = JSON.stringify({
                        "componentName": result.componentName,
                        "applications": applicationNames,
                        "filePath": result.filePath,
                    });
                    $http({
                        method: 'POST',
                        url: 'api/ComponentApi/',
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
            templateUrl: "Content/Templates/addVariableModal.html",
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
                            url: 'api/ConfigApi/',
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
    $scope.showFile = function (row) {
        var componentRow = '$$' + row.uid;
        var componentGroupName = row.treeNode.aggregations[0].groupVal;
        var rowEntity = row.entity;
        $http.get('/api/ConfigApi?componentName=' + componentGroupName + '&environment=' + $scope.environment)
            .success(function (data) {
                ModalService.showModal({
                    templateUrl: "Content/Templates/configFileModal.html",
                    controller: "ConfigViewer",
                    inputs: {
                        title: data.componentName,
                        filePath: data.path,
                        configXml: data.text,
                        publish: false,
                        download: false
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        modal.close.then(function (result) {
                            if (result.download) {
                                $scope.downloadConfig(result.title)
                            };
                            if (result.publish) {
                                $scope.downloadConfig(result.title)
                            };
                        });
                    })
            })
    };

    // Config File Download
    $scope.downloadConfig = function (componentName) {
        $scope.downloadFile(componentName, $scope.environment);
    };
    $scope.downloadFile = function (componentName, environment) {
        $http({
            method: 'GET',
            url: 'api/ConfigPublishApi',
            //withCredentials: true,
            params: {
                componentName: componentName,
                environment: environment
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
                linkElement.setAttribute("download", filename);
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

///-------------------------------------------------------------------------------------------------
/// <summary>   Controller for the Config File Viewer / Downloader Modal. </summary>
///
/// <remarks>   Pdelosreyes, 4/19/2017. </remarks>
///
/// <param name="'ConfigViewer'">   The configuration viewer'. </param>
/// <param name="($rootScope">      The $root scope. </param>
/// <param name="$scope">           The $scope. </param>
/// <param name="$element">         The $element. </param>
/// <param name="title">            The title. </param>
/// <param name="filePath">         Full pathname of the file. </param>
/// <param name="configXml">        The configuration XML. </param>
/// <param name="close">            The close. </param>
/// <param name="publish">          The publish. </param>
/// <param name="download">         The download. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('ConfigViewer',
    function ($rootScope, $scope, $element, title, filePath, configXml, close, publish, download) {
        //var vm = this;
        var vm = $scope;
        vm.filePath = filePath;
        vm.configXml = configXml;
        vm.title = title;
        vm.close = function () {
            $element.modal('hide');
            close({
                publish: false,
                download: false,
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({
                publish: false,
                download: false,
            }, 500);
        };
        vm.publish = function () {
            $element.modal('hide');
            close({
                publish: true,
                download: false,
                title: vm.title
            }, 500);
        }
        vm.download = function () {
            $element.modal('hide');
            close({
                publish: false,
                download: true,
                title: vm.title
            }, 500);
        }
        vm.ngClickCopy = function () {
            vm.ngClickCopy;
            //$rootscope.ngClickCopy;
        }
    });

///-------------------------------------------------------------------------------------------------
/// <summary>   Controllers. </summary>
///
/// <remarks>   Pdelosreyes, 4/19/2017. </remarks>
///
/// <param name="'AddComponent'">       The add component'. </param>
/// <param name="($rootScope">          The $root scope. </param>
/// <param name="$scope">               The $scope. </param>
/// <param name="$element">             The $element. </param>
/// <param name="close">                The close. </param>
/// <param name="filePath">             Full pathname of the file. </param>
/// <param name="components">           The components. </param>
/// <param name="componentComponents">  The component components. </param>
/// <param name="componentName">        Name of the component. </param>
/// <param name="fileName">             Filename of the file. </param>
/// <param name="componentEnvironment"> The component environment. </param>
/// <param name="environments">         The environments. </param>
/// <param name="file">                 The file. </param>
/// <param name="publish">              The publish. </param>
/// <param name="upload">               The upload. </param>
///
/// <returns>   . </returns>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('AddComponent',
    function ($rootScope, $scope, $element, $http, $timeout, Upload, close, components, applications, environments) {
        //var vm = this;
        var vm = $scope;
        var componentData;
        var componentComponents;
        var filePath = filePath;
        var availableApplications = [];
        var componentApplications = [];
        var componentName;
        var componentEnvironment;
        var fileName;
        var localComponent;

        vm.availableApplications = [];
        vm.componentApplications = [];
        vm.componentName = "";
        vm.componentEnvironment = "";
        vm.fileName = "";
        vm.applications = applications;
        vm.components = components;
        vm.componentEnvironment = componentEnvironment;
        vm.environments = environments;
        vm.localComponent = {
            componentName: vm.componentName,
            filePath: vm.filePath,
            applications: vm.componentApplications,
        }

        vm.availableApplications = applications.map(function (item) { return item["name"]; });

        vm.selectComponent = function (component) {
            $http({
                method: 'GET',
                url: '/api/ComponentApi',
                //withCredentials: true,
                params: {
                    componentName: component.name,
                },
                //responseType: 'arraybuffer'
            }).then(function (result) {
                vm.componentData = result.data;
                if (typeof vm.componentData.ConfigFile !== "undefined")
                    vm.fileName = vm.componentData.ConfigFiles.file_name;
                vm.filePath = vm.componentData.relative_path;
                vm.componentName = vm.componentComponents.name;
                vm.componentApplications = [];
                angular.forEach(result.data.Applications, function (Applications) {
                    var name = Applications.application_name;
                    var id = Applications.id;
                    var value = Applications.application_name;
                    vm.componentApplications.push({ id: id, name: name, value: value });
                });
                vm.localComponent = {
                    componentName: vm.componentName,
                    filePath: vm.filePath,
                    applications: vm.componentApplications,
                }
            });
        };

        vm.$watch('files', function () {
            $scope.upload($scope.files);
        });
        vm.$watch('file', function () {
            if ($scope.file != null) {
                $scope.files = [$scope.file];
            }
        });
        vm.log = '';

        vm.upload = function (files) {
            if (files) {
                var file = files;
                if (!file.$error) {
                    Upload.upload({
                        url: 'api/ConfigPublishApi',
                        params: {
                            componentName: vm.componentName,
                            environment: vm.componentEnvironment.name,
                            applications: vm.componentApplications.join(','),
                        },
                        data: {
                            file: file
                        }
                    }).then(function (resp) {
                        $timeout(function () {
                            $scope.log = 'file: ' +
                            resp.config.data.file.name +
                            ', Response: ' + JSON.stringify(resp.data) +
                            '\n' + $scope.log;
                        });
                    }, null, function (evt) {
                        var progressPercentage = parseInt(100.0 *
                                evt.loaded / evt.total);
                        $scope.log = 'progress: ' + progressPercentage +
                            '% ' + evt.config.data.file.name + '\n' +
                          $scope.log;
                    });
                }
            }
        }

        vm.close = function () {
            close({
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({
                publish: false,
                save: false,
            }, 500);
        };
        vm.publish = function () {
            $element.modal('hide');
            close({
                save: true,
                publish: true,
                componentApplications: vm.componentApplications,
                componentName: vm.componentName,
                filePath: vm.filePath,
            }, 500);
        }
        vm.save = function () {
            $element.modal('hide');
            close({
                save: true,
                publish: false,
                componentApplications: vm.componentApplications,
                componentName: vm.componentName,
                filePath: vm.filePath,
            }, 500);
        }
    });

///-------------------------------------------------------------------------------------------------
/// <summary>   AddVar Controller </summary>
///
/// <remarks>   Pdelosreyes, 4/19/2017. </remarks>
///
/// <param name="close">            The close. </param>
/// <param name="componentName">    Name of the component. </param>
/// <param name="parentElement">    The parent element. </param>
/// <param name="element">          The element. </param>
/// <param name="keyName">          Name of the key. </param>
/// <param name="key">              The key. </param>
/// <param name="valueName">        Name of the value. </param>
/// <param name="save">             The save. </param>
/// <param name="publish">          The publish. </param>
///-------------------------------------------------------------------------------------------------
ConfigApp.controller('AddVar',
    function ($rootScope, $scope, $element, close, componentName, parentElement, element, keyName, key, valueName, show, isNew, save) {
        var vm = $scope;
        var additionalParentElements = [];
        vm.componentName = componentName;
        vm.element = element;
        vm.parentElement = parentElement;
        vm.keyName = keyName;
        vm.key = key;
        vm.valueName = valueName;
        vm.show = show;
        vm.isNew = isNew;
        vm.close = function () {
            close({
                save: false,
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        vm.cancel = function () {
            $element.modal('hide');
            close({
                save: false,
            }, 500);
        };
        vm.save = function () {
            $element.modal('hide');
            close({
                save: true,
                componentName: vm.componentName,
                element: vm.element,
                parentElement: vm.parentElement,
                keyName: vm.keyName,
                key: vm.key,
                valueName: valueName,
                additionalParentElements: vm.additionalParentElements
            }, 500);
        };
    });

ConfigApp.directive('multiSelect', function ($q) {
    return {
        restrict: 'E',
        require: 'ngModel',
        scope: {
            selectedLabel: "@",
            availableLabel: "@",
            displayAttr: "@",
            available: "=",
            model: "=ngModel"
        },
        template: '<div class="multiSelect">' +
                    '<div class="select">' +
                      '<label class="control-label" for="multiSelectSelected">{{ selectedLabel }} ' +
                          '({{ model.length }})</label>' +
                      '<select id="currentRoles" ng-model="selected.current" multiple ' +
                          'class="pull-left" ng-options="e as e[displayAttr] for e in model">' +
                          '</select>' +
                    '</div>' +
                    '<div class="select buttons">' +
                      '<button class="btn mover left" ng-click="add()" title="Add selected" ' +
                          'ng-disabled="selected.available.length == 0">' +
                        '<i class="icon-arrow-left"></i>' +
                      '</button>' +
                      '<button class="btn mover right" ng-click="remove()" title="Remove selected" ' +
                          'ng-disabled="selected.current.length == 0">' +
                        '<i class="icon-arrow-right"></i>' +
                      '</button>' +
                    '</div>' +
                    '<div class="select">' +
                      '<label class="control-label" for="multiSelectAvailable">{{ availableLabel }} ' +
                          '({{ available.length }})</label>' +
                      '<select id="multiSelectAvailable" ng-model="selected.available" multiple ' +
                          'ng-options="e as e[displayAttr] for e in available"></select>' +
                    '</div>' +
                  '</div>',
        link: function (scope, elm, attrs) {
            scope.selected = {
                available: [],
                current: []
            };

            /* Handles cases where scope data hasn't been initialized yet */
            var dataLoading = function (scopeAttr) {
                var loading = $q.defer();
                if (scope[scopeAttr]) {
                    loading.resolve(scope[scopeAttr]);
                } else {
                    scope.$watch(scopeAttr, function (newValue, oldValue) {
                        if (newValue !== undefined)
                            loading.resolve(newValue);
                    });
                }
                return loading.promise;
            };

            /* Filters out items in original that are also in toFilter. Compares by reference. */
            var filterOut = function (original, toFilter) {
                var filtered = [];
                angular.forEach(original, function (entity) {
                    var match = false;
                    for (var i = 0; i < toFilter.length; i++) {
                        if (toFilter[i][attrs.displayAttr] == entity[attrs.displayAttr]) {
                            match = true;
                            break;
                        }
                    }
                    if (!match) {
                        filtered.push(entity);
                    }
                });
                return filtered;
            };

            scope.refreshAvailable = function () {
                scope.available = filterOut(scope.available, scope.model);
                scope.selected.available = [];
                scope.selected.current = [];
            };

            scope.add = function () {
                scope.model = scope.model.concat(scope.selected.available);
                scope.refreshAvailable();
            };
            scope.remove = function () {
                scope.available = scope.available.concat(scope.selected.current);
                scope.model = filterOut(scope.model, scope.selected.current);
                scope.refreshAvailable();
            };

            $q.all([dataLoading("model"), dataLoading("available")]).then(function (results) {
                scope.refreshAvailable();
            });
        }
    };
})
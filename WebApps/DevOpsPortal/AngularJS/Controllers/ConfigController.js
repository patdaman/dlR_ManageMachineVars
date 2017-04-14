'use strict'

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
    uiGridConstants, $q, $interval, $templateCache, ModalService) {
    $scope.title = "Application Configuration";
    //var vm = $scope;
    var data = [];
    var i;

    var rowIndex;
    var rowId;
    var subGridRowId;

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
        appScopeProvider: $scope,

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

        rowEquality: function (entityA, entityB) {
            var aId = angular.isObject(entityA) ? entityA.configvar_id : entityA;
            var bId = angular.isObject(entityB) ? entityB.configvar_id : entityB;

            return aId === bId;
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
        gridApi.cellNav.on.navigate($scope, function (selected) {
            if ('.ui-grid-cell-focus ') {
                if ($scope.bypassEditCancel === false) {
                    if ($scope.rowId !== (selected.row.entity.index)) {
                        if (selected.row.entity.index === 'undefined') {
                            $scope.cancelEdit(selected);
                        }
                        else
                            if ($scope.gridOptions.data[(selected.row.entity.index)].editable !== true) {
                                $scope.cancelEdit(selected);
                            }
                    }
                }
            }
        })
        gridApi.rowEdit.on.saveRow($scope, $scope.cancelEdit());
    };

    var editableTemplate = '<div ng-if="!row.entity.editable">{{COL_FIELD}}</div><div ng-if="row.entity.editable"><input ng-model="MODEL_COL_FIELD"</div>';

    $scope.isBlank = function (str) {
        return (!str || /^\s*$/.test(str));
    }

    $scope.editCell = function (row) {
        $scope.selectedRow = row;
        $scope.var_id = row.entity.configvar_id;
        $scope.rowIndex = row.grid.renderContainers.body.visibleRowCache.indexOf($scope.selectedRow);
        // For App Keys
        if (typeof row.entity.key !== "undefined") {
            $scope.rowId = row.entity.index;
            $scope.gridApi.grid.cellNav.clearFocus();
            $scope.gridApi.grid.cellNav.focusedCells = [];
            $scope.key = row.entity.key;
            $scope.gridOptions.data[$scope.rowId].editable = true;
            $scope.edit = true;
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            $scope.scrollToFocus($scope.rowId, 7);
        }
            // For Values
        else {
            $scope.rowId = row.grid.parentRow.treeNode.row.entity.index;
            $scope.selectedRow.grid.cellNav.clearFocus();
            $scope.selectedRow.grid.cellNav.focusedCells = [];
            angular.forEach($scope.gridOptions.data[$scope.rowId].subGridOptions.data, function (data, index) {
                data["index"] = index;
                if (data.environment === row.entity.environment) {
                    $scope.subGridRowId = index;
                }
            });
            $scope.value = row.entity.value;
            $scope.gridOptions.data[$scope.rowId].subGridOptions.data[$scope.subGridRowId].editable = true;
            $scope.subEdit = true;
            $scope.selectedRow.grid.api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
            $scope.subCanEdit();
            $scope.selectedRow.grid.api.cellNav.scrollToFocus($scope.gridOptions.data[$scope.rowId].values[$scope.subGridRowId], 4);
        }
        $scope.bypassEditCancel = false;
    };

    $scope.cancelEdit = function () {
        $scope.edit = false;
        $scope.subEdit = false;
        if ($scope.selectedRow.entity !== "undefined") {
            if (typeof $scope.selectedRow.entity.key !== "undefined") {
                $scope.selectedRow.entity.key = $scope.key;
                var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
                var dataRows = gridRows.map(function (gridRow) {
                    return gridRow.entity;
                });
                $scope.gridApi.rowEdit.setRowsClean(dataRows);
                $scope.gridOptions.data[$scope.rowId].editable = false;

            }
            else {
                $scope.selectedRow.entity.value = $scope.value;
                var subGridRows = $scope.selectedRow.grid.api.rowEdit.getDirtyRows();
                var subDataRows = subGridRows.map(function (gridRow) {
                    return gridRow.entity;
                })
                $scope.selectedRow.grid.api.rowEdit.setRowsClean(subDataRows);
                if ($scope.gridOptions.data[$scope.rowId].subGridOptions.data[$scope.subGridRowId].editable === true) {
                    $scope.gridOptions.data[$scope.rowId].subGridOptions.data[$scope.subGridRowId].editable = false;
                    $scope.selectedRow.grid.api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
                }
            }
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        }
        $scope.bypassEditCancel = true;
    };

    $scope.scrollToFocus = function (rowIndex, colIndex) {
        $scope.canEdit();
        $scope.gridApi.cellNav.scrollToFocus($scope.gridOptions.data[rowIndex], $scope.gridOptions.columnDefs[colIndex]);
    };

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
        $scope.gridOptions.data[rowEntity.index].editable = false;
        $scope.bypassEditCancel = true;
    };

    $scope.saveRowFunction = function (rowEntity) {
        var deferred = $q.defer();
        $http.post('/api/ConfigApi/', rowEntity).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

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

    $scope.updateEnvironment = function () {
        $scope.environment = $scope.selectedEnvironment;
        //$scope.gridOptions.appScopeProvider.gridApi.grid.api.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
        //$scope.gridOptions.appScopeProvider.gridApi.grid.api.core.notifyDataChange(uiGridConstants.dataChange.ALL);
        //$scope.gridApi.grid.refresh();
        //$scope.gridOptions.appScopeProvider.gridApi.grid.api.core.refresh();
        $scope.filterSubGrid($scope.environment);
    };

    $scope.updateComponent = function () {
        $scope.component = $scope.selectedComponent;
        $scope.gridApi.grid.refresh();
    };

    $scope.updateApplication = function () {
        $scope.application = $scope.selectedApplication;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
    };

    $scope.addComponent = function () {
        ModalService.showModal({
            templateUrl: "Content/Templates/addComponentModal.html",
            controller: "AddComponent",
            inputs: {
                filePath: "",
                applications: [],
                components: $scope.components,
                componentComponents: "",
                componentName: "",
                componentEnvironment: "",
                environments: $scope.environments,
                fileName: "",
                file: null,
                publish: false,
                upload: false
            }
        })
        .then(function (modal) {
            modal.element.modal();
            modal.close.then(function (result) {
                if (result.upload === true) {
                    if (result.file !== 'undefined' && result.file !== null) {
                        $scope.uploadFile(result.file, result.fileName, result.filePath, result.componentEnvironment, result.componentName, result.componentId, result.applications);
                    }
                } else if (result.publish === true) {
                    if (result.file !== 'undefined' && result.file !== null) {
                        $scope.publishFile(result.file, result.fileName, result.filePath, result.componentEnvironment, result.componentName, result.componentId, result.applications);
                    }
                } else if (result.componentId !== 'undefined' && result.componentId !== null) {
                    $scope.updateComponent(result.fileName, result.filePath, result.componentEnvironment, result.componentName, result.componentId, result.applications)
                } else {
                    $scope.updateComponent(result.fileName, result.filePath, result.componentEnvironment, result.componentName, result.componentId, result.applications)
                }
            });
        }).catch(function (error) {
            console.log(error);
        })
    };

    $scope.addVar = function (row) {
        var componentRow = '$$' + row.uid;
        var rowEntity = row.entity;
        var componentAggObject = rowEntity['$$uiGrid-000A'];
        var firstChildParentElement = row.treeNode.children[0].row.entity.configParentElement;
        ModalService.showModal({
            templateUrl: "Content/Templates/addVariableModal.html",
            controller: "AddVar",
            inputs: {
                componentName: componentAggObject.groupVal,
                parentElement: firstChildParentElement,
                element: "",
                keyName: "",
                key: "",
                valueName: "",
                save: false,
                publish: false
            }
        })
            .then(function (modal) {
                modal.element.modal();
                modal.close.then(function (result) {
                });
            }).catch(function (error) {
                console.log(error);
            })
    };

    $scope.showFile = function (row) {
        var componentRow = '$$' + row.uid;
        var rowEntity = row.entity;
        var componentAggObject = rowEntity['$$uiGrid-000A'];
        $http.get('/api/ConfigApi?componentName=' + componentAggObject.groupVal + '&environment=' + $scope.environment)
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

    $scope.downloadConfig = function (componentName) {
        $scope.downloadFile(componentName, $scope.environment);
    };

    $scope.uploadConfig = function (componentName, environment) {
        $scope.uploadFile(componentName, environment);
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
                    columnDefs: [
                    { displayName: "id", field: "id", visible: false },
                    {
                        displayName: "Variable id", field: "configvar_id", visible: false
                    },
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
                    {
                        displayName: "Create Date", field: "create_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"'
                    },
                    {
                        displayName: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, type: 'date', enableFiltering: false, cellFilter: 'date:"MM-dd-yyyy"'
                    },
                    {
                        displayName: "Last Publish Date", field: "publish_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"'
                    },
                    {
                        displayName: "Is Published", field: "published", visible: false, enableCellEdit: false, type: 'boolean'
                    },
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
                    onRegisterApi: function (gridApi) {
                        $scope.gridApi = gridApi;
                        gridApi.cellNav.on.navigate($scope, function (selected) {
                            if ('.ui-grid-cell-focus ') {
                                if ($scope.bypassEditCancel === false) {
                                    if (selected.row.grid.parentRow === 'undefined') {
                                        $scope.cancelEdit();
                                    }
                                    else
                                        if ($scope.rowId !== selected.row.grid.parentRow.treeNode.row.entity.index || selected.row.entity.environment !== $scope.environment) {
                                            $scope.cancelEdit();
                                        }
                                }
                            }
                        })
                        gridApi.rowEdit.on.saveRow($scope, $scope.cancelEdit());
                        //gridApi.grid.registerRowsProcessor($scope.singleFilter, 200);
                    }
                };
            }
            $scope.gridOptions.data = data;
            angular.forEach(data, function (data, index) {
                data["index"] = index;
            });
        });

    $scope.filterSubGrid = function (value) {
        console.log(value);
        $scope.gridOptions.appScopeProvider.gridApi.grid.api.grid.columns[2].filters[0].term = value;

        angular.forEach($scope.gridOptions.data, function(data) {
            data.values.subGridOptions;
        });
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
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        vm.cancel = function () {
            $element.modal('hide');
            close(null, 500);
        };
        vm.publish = function () {
            $element.modal('hide');
            close({
                publish: true,
                title: vm.title
            }, 500);
        }
        vm.download = function () {
            $element.modal('hide');
            close({
                download: true,
                title: vm.title
            }, 500);
        }
        vm.ngClickCopy = function () {
            vm.ngClickCopy;
            //$rootscope.ngClickCopy;
        }
    });

ConfigApp.controller('AddComponent',
    function ($rootScope, $scope, $element, close, filePath, components, componentComponents, componentName, fileName, componentEnvironment, environments, file, publish, upload) {
        //var vm = this;
        var vm = $scope;
        vm.filePath = filePath;
        vm.componentComponents = componentComponents;
        vm.components = components;
        vm.componentName = componentName;
        vm.componentEnvironment = componentEnvironment;
        vm.environments = environments;
        vm.fileName = fileName;
        vm.file = file;
        vm.close = function () {
            close({
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        vm.cancel = function () {
            $element.modal('hide');
            close(null, 500);
        };
        vm.publish = function () {
            $element.modal('hide');
            close({
                publish: true,
                componentName: vm.componentName
            }, 500);
        }
        vm.upload = function () {
            $element.modal('hide');
            close({
                upload: true,
                componentName: vm.componentName
            }, 500);
        }
    });

ConfigApp.controller('AddVar',
    function ($rootScope, $scope, $element, close, componentName, parentElement, element, keyName, key, valueName, save, publish) {
        var vm = $scope;
        vm.componentName = componentName;
        vm.element = element;
        vm.parentElement = parentElement;
        vm.keyName = keyName;
        vm.key = key;
        vm.valueName = valueName;
        vm.close = function () {
            close({
                componentName: vm.componentName
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        vm.cancel = function () {
            $element.modal('hide');
            close(null, 500);
        };
        vm.save = function () {
            $element.modal('hide');
            close(null, 500);
        };
        vm.publish = function () {
            $element.modal('hide');
            close({
                publish: true,
                componentName: vm.componentName
            }, 500);
        }
    });


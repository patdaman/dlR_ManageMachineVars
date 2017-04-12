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
    var var_id;
    var keyName;
    var title;
    var filePath;
    var configXml;
    var componentId;
    var componentName;
    var value;
    var currentRow;
    var focusedCell;

    var selectedComponent;

    $scope.environment = 'development';
    $scope.filterEnvironment = function () {
        return $scope.environment;
    };

    $scope.component = '';
    $scope.filterComponent = function () {
        return $scope.component;
    };

    $scope.edit = false;
    $scope.canEdit = function () {
        return $scope.edit;
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
        multiselect: false,

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
        { field: 'applicationNames', enableCellEdit: false },
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
                //term: $scope.filterComponent
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
        //$scope.gridApi.selection.on.rowSelectionChanged($scope, function (row) {
        //    if ($scope.gridOptions.data[rowId].editable === true) {
        //        $scope.cancelEdit(row);
        //    }
        //});
    };

    var editableTemplate = '<div ng-if="!row.entity.editable">{{COL_FIELD}}</div><div ng-if="row.entity.editable"><input ng-model="MODEL_COL_FIELD"</div>';

    $scope.isBlank = function (str) {
        return (!str || /^\s*$/.test(str));
    }

    $scope.findMatch = function (rowIndex) {
        $scope.gridOptions.enableRowSelection = true;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.OPTIONS)
        $scope.gridApi.selection.clearSelectedRows();
        var row = $scope.gridApi.selection.selectRow(rowIndex);
        $scope.gridOptions.enableRowSelection = false;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.OPTIONS)
        return row;
    }

    $scope.editCell = function (row) {
        $scope.currentRow = row;
        $scope.gridApi.grid.cellNav.clearFocus();
        $scope.gridApi.grid.cellNav.focusedCells = [];
        $scope.rowIndex = row.grid.renderContainers.body.visibleRowCache.indexOf(row);

        var rowCol = $scope.gridApi.cellNav.getFocusedCell();
        if (typeof row.entity.key !== "undefined") {
            $scope.var_id = row.entity.configvar_id;
            $scope.keyName = row.entity.key;
            $scope.rowId = (row.entity.index) - 1;
        }
        else {
            $scope.parentVar_id = row.entity.configvar_id;
            $scope.value = row.entity.value;
            var parentRow = row.grid.parentRow.treeNode.row;
            $scope.rowId = (parentRow.entity.index) - 1;
        }
        $scope.gridOptions.data[$scope.rowId].editable = true;
        $scope.edit = true;
        $scope.scrollToFocus(1, 7);
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
        if ($scope.rowIndex !== "undefined") {
            $scope.scrollToFocus($scope.rowId, 7);
            //$scope.scrollToFocus($scope.rowId, 7);
        }
        else {
            $scope.scrollToFocus($scope.rowId, 7);
            $scope.scrollToFocus($scope.rowId, 7);
        }
    };

    $scope.cancelEdit = function (rowEntity) {
        if (typeof rowEntity.keyName !== "undefined") {
            rowEntity.key = $scope.keyName;
        }
        else {
            rowEntity.value = $scope.value;
        }
        $scope.edit = false;
        var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
        var dataRows = gridRows.map(function (gridRow) {
            return gridRow.entity;
        });
        $scope.gridApi.rowEdit.setRowsClean(dataRows);
        var rowId = rowEntity.index;
        $scope.gridOptions.data[rowId].editable = false;

        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
    };

    $scope.scrollToFocus = function (rowIndex, colIndex) {
        $scope.canEdit();
        $scope.gridApi.cellNav.scrollToFocus($scope.gridOptions.data[rowIndex], $scope.gridOptions.columnDefs[colIndex]);
    };

    $scope.saveRow = function (rowEntity) {
        var promise = $scope.saveRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
        $scope.edit = false;
        var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
        var dataRows = gridRows.map(function (gridRow) {
            return gridRow.entity;
        });
        $scope.gridApi.rowEdit.setRowsClean(dataRows);
        var rowId = rowEntity.index;
        $scope.gridOptions.data[rowId].editable = false;
    };

    $scope.saveRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    $scope.saveSubGridRow = function (rowEntity) {
        var promise = $scope.saveSubGridRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
        $scope.edit = false;
        var gridRows = $scope.gridApi.rowEdit.getDirtyRows();
        var dataRows = gridRows.map(function (gridRow) {
            return gridRow.entity;
        });
        $scope.gridApi.rowEdit.setRowsClean(dataRows);
        var rowId = rowEntity.index;
        $scope.gridOptions.data[rowId].editable = false;
    };

    $scope.saveSubGridRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigValuesApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    // These need to come from Api:
    $scope.environments = ["development", "qa", "production"];
    $scope.components = ["Commerce", "DAL", "ManagerI18N", "Services"];
    // End Todo

    $scope.updateEnvironment = function () {
        $scope.environment = $scope.selectedEnvironment;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.ALL);
    };

    $scope.filterComponent = function () {
        $scope.component = $scope.selectedComponent;
        $scope.gridApi.grid.refresh();
    };

    $scope.addComponent = function () {
        ModalService.showModal({
            templateUrl: "Content/Templates/configFileModal.html",
            controller: "AddComponent"
        })
        .then(function (modal) {

        }).catch(function (error) {
            console.log(error);
        })
    };

    $scope.addVar = function (row) {
        ModalService.showModal({
            templateUrl: "Content/Templates/configFileModal.html",
            controller: "AddVar"
        })
            .then(function (modal) {

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
                    enableCellSelection: true,
                    enableCellEditOnFocus: true,
                    enableRowSelection: true,
                    enableRowHeaderSelection: false,
                    multiselect: false,
                    enableFiltering: true,
                    columnDefs: [
                        { displayName: "id", field: "id", visible: false, resizable: true },
                        { displayName: "Variable id", field: "configvar_id", visible: false },
                        {
                            field: "environment",
                            visible: true,
                            enableFiltering: false,
                            filter: {
                                condition: function (searchTerm, cellValue) {
                                    let result = true;
                                    result = $scope.environment === cellValue;
                                    return result;
                                },
                                noTerm: true
                            },
                            filterCellFiltered: true,
                            enableCellEdit: false
                        },
                        { displayName: "Value", field: "value", visible: true, cellEditableCondition: $scope.canEdit, enableFiltering: false, width: "70%" },
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
                    onRegisterApi: function (gridApi) {
                        $scope.gridApi = gridApi;
                        //$scope.gridApi.selection.on.rowSelectionChanged($scope, function (row) {
                        //    if ($scope.gridOptions.data[rowId].editable === true) {
                        //        $scope.cancelEdit(row);

                        //    }});
                    }
                };
            }
            $scope.gridOptions.data = data;
            angular.forEach(data, function (data, index) {
                data["index"] = index + 1;
            });
        });

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
            close({
                filePath: $scope.filePath,
                configXml: $scope.configXml
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
    function ($rootScope, $scope, $element, filePath, componentName, close, publish, upload) {
        //var vm = this;
        var vm = $scope;
        vm.filePath = filePath;
        vm.configXml = configXml;
        vm.title = title;
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
    function ($rootScope, $scope, $element, componentName, close, publish) {
        var vm = $scope;
        vm.componentName = componentName;
        vm.close = function () {
            close({
                componentName: vm.componentName
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
    });


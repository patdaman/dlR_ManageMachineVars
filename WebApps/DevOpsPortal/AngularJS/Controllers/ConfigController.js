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
            'angularModalService',
            'ngAnimate',
            'ui.bootstrap'
        ]);

ConfigApp.controller('ConfigController', function ($rootScope, $scope, $http, $log, $timeout,
    uiGridConstants, $q, $interval, $templateCache, ModalService) {
    $scope.title = "Application Configuration";
    //var vm = $scope;
    var data = [];
    var i;
    var editMode = false;

    var rowIndex;
    var configXml;
    var filePath;

    var environment = 'development';
    var componentId;
    var componentName;

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
        appScopeProvider: {
            showRow: function (row) {
                return row.key !== '';
            }
        },

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

        //expandableRowTemplate: '<div ui-grid="row.entity.subGridOptions" ui-grid-edit ui-grid-row-edit ui-grid-cellNav ui-grid-selection style="width:100%; float:right"></div>',
        //expandableRowTemplate: '<div style="padding:5px;"><div ui-grid="row.entity.subGridOptions[0]" ui-grid-edit  ui-grid-row-edit ui-grid-selection style="display:inline-block;"></div><div ui-grid="row.entity.subGridOptions[1]" ui-grid-edit  ui-grid-row-edit ui-grid-selection style="height:340px;width:48%;display:inline-block;margin-left:5px"></div></div>',
        expandableRowTemplate: 'Content/Templates/expandableRowTemplate.html',
        expandableRowHeight: 125,
        expandableRowScope: {
            subGridVariable: 'subGridScopeVariable'
        },
    };

    $scope.gridOptions.columnDefs = [
        { field: 'applicationNames', enableCellEdit: false, cellTemplate: basicCellTemplate },
        { field: 'componentId', visible: false, enableCellEdit: false },
        { field: 'componentName', enableCellEdit: false, grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, groupable: true },
        { field: 'configvar_id', visible: false, enableCellEdit: false },
        { field: 'configParentElement', visible: false, enableCellEdit: false },
        { field: 'configElement', visible: false, enableCellEdit: false },
        { field: 'attribute', visible: false, enableCellEdit: false },
        {
            field: 'key',
            cellEditableCondition: $scope.canEdit,
            //enableCellEdit: true,
            width: "50%",
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
            enableFiltering: false
        },
                {
                    name: "Actions",
                    cellTemplate: '<div ng-if="!row.groupHeader"><div class="ui-grid-cell-contents">' +
                    '<button value="Edit" class="btn btn-xs btn-info" ng-if="!row.inlineEdit.isEditModeOn" ng-click="grid.appScope.editCell(row.entity)">Edit</button>' +
                    //'<button value="Edit" class="btn btn-xs btn-info" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
                    '<button value="Edit" class="btn btn-xs btn-danger" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Delete</button>' +
                    '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
                    '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
                    '</div></div>' +
                    '<div ng-if="row.groupHeader"><div class="ui-grid-cell-contents"><button class="btn btn-xs btn-default" ng-click="grid.appScope.showFile(row.entity)">View File</button></div>',
                    enableCellEdit: false
                },

    ];

    //var basicCellTemplate = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div>';
    var basicCellTemplate = 'Content/Templates/basicCellTemplate.html';

    $scope.editCell = function (rowEntity) {
        var row = $scope.gridApi.cellNav.getFocusedCell();
        $scope.edit = true;
        $scope.scrollToFocus(row.row.entity.id, row.col.colDef.key);
    };

    $scope.isBlank = function (str) {
        return (!str || /^\s*$/.test(str));
    }

    $templateCache.put('ui-grid/uiGridViewport',
        "<div class=\"ui-grid-viewport\" ng-style=\"colContainer.getViewportStyle()\"><div class=\"ui-grid-canvas\"><div ng-repeat=\"(rowRenderIndex, row) in rowContainer.renderedRows track by $index\" ng-if=\"grid.appScope.showRow(row.entity)\" class=\"ui-grid-row\" ng-style=\"Viewport.rowStyle(rowRenderIndex)\"><div ui-grid-row=\"row\" row-render-index=\"rowRenderIndex\"></div></div></div></div>"
    );

    $scope.scrollToFocus = function (rowIndex, colIndex) {
        $scope.canEdit();
        $scope.gridApi.cellNav.scrollToFocus($scope.gridOptions.data[rowIndex], $scope.gridOptions.columnDefs[colIndex]);
    };

    //$scope.selectedCell;
    //$scope.selectedRow;
    //$scope.selectedColumn;

    //$scope.editCell = function (row, cell, column) {
    //    $scope.selectedCell = cell;
    //    $scope.selectedRow = row;
    //    $scope.selectedColumn = column;
    //};

    //$scope.updateCell = function () {
    //    //   alert("checking");  
    //    $scope.selectedRow[$scope.selectedColumn] = $scope.selectedCell;
    //};

    //$scope.editable = function (row) {
    //    $scope.editMode = !editMode;
    //};

    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        //gridApi.cellNav.on.navigate(null, function (newRowCol, oldRowCol) {
        //    $scope.gridApi.selection.selectRow(newRowCol.row.entity);
        //    $scope.gridApi.core.notifyDataChange($scope.gridApi.grid, uiGridConstants.dataChange.COLUMN);
        //});

        //gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
        //    var selectedRows = $scope.gridApi.selection.getSelectedRows();

        //    if (newValue !== oldValue) {

        //        rowEntity.state = "Changed";
        //        //Get column
        //        var rowCol = $scope.gridApi.cellNav.getFocusedCell().col.colDef.name;

        //        angular.forEach(selectedRows, function (item) {
        //            item[rowCol] = rowEntity[rowCol];
        //            item.state = "Changed";
        //            item.isDirty = false;
        //            item.isError = false;
        //        });

        //    }
        //});

        gridApi.rowEdit.on.saveRow($scope, $scope.saveRow);
    };

    $scope.saveRow = function (rowEntity) {
        var promise = $scope.saveRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
        $scope.edit = false;
    };

    $scope.saveRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    $scope.toggleEdit = function (rowNum) {
        $scope.gridOptions.data[rowNum].editable = !$scope.gridOptions1.data[rowNum].editable;
        $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
    };

    $scope.saveSubGridRow = function (rowEntity) {
        var promise = $scope.saveSubGridRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
        $scope.edit = false;
    };

    $scope.saveSubGridRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigValuesApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };

    // These need to come from Api:
    $scope.environments = ["development", "qa", "production"];
    $scope.machines = ["sdsvc01.dc.pti.com", "hqdev07.dev.corp.printable.com", "hqdev08.dev.corp.printable.com"];
    $scope.components = ["Commerce", "DAL", "ManagerI18N", "Services"];
    // End Todo

    //$scope.showFile = function (rowEntity) {
    //    var configFile = $http.get('/api/ConfigApi?componentName=' + rowEntity.componentName + '&environment=development' );
    $scope.showFile = function (row) {

        $http.get('/api/ConfigApi?componentName=' + 'Commerce' + '&environment=development')
            .success(function (data) {
                ModalService.showModal({
                    templateUrl: "Content/Templates/configFileModal.html",
                    controller: "ConfigViewer",
                    inputs: {
                        title: data.componentName,
                        filePath: data.path,
                        configXml: data.text
                    }
                })
                    .then(function (modal) {
                        modal.element.modal();
                        //modal.close.then(function (result) {
                        //    $scope.complexResult = "Name: " + result.name + ", age: " + result.age;
                        //});
                    });
            }
    )
    };

    $scope.addVar = function (row) {
        ModalService.showModal({
            templateUrl: "",
            controller: "",
            inputs: {
                title: "",
                componentName: "",
            }
        })
    };

    $http.get('/api/ConfigApi')
        .success(function (data) {
            for (i = 0; i < data.length; i++) {
                data[i].subGridOptions = {
                    enableHorizontalScrollbar: 0,
                    //appScopeProvider: $scope,
                    appScopeProvider: {
                        showRow: function (row) {
                            return row.environment !== 'development';
                        }
                    },
                    columnDefs: [
                        { displayName: "id", field: "id", visible: false, resizable: true },
                        { displayName: "Variable id", field: "configvar_id", visible: false },
                        { displayName: "Environment", field: "environment", visible: true, enableCellEdit: false, cellTemplate: basicCellTemplate },
                        { displayName: "Value", field: "value", visible: true, enableCellEdit: true, width: "50%" },
                        { displayName: "Create Date", field: "create_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { displayName: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { displayName: "Last Publish Date", field: "publish_date", visible: false, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                        { displayName: "Is Published", field: "published", visible: false, enableCellEdit: false, type: 'boolean' },
                        {
                            name: "Actions",
                            cellTemplate: '<div class="ui-grid-cell-contents" >' +
                            '<button value="Edit" class="btn btn-xs btn-info" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
                            //'<button value="Edit" class="btn btn-xs btn-warning" ng-if="!row.inlineEdit.isEditModeOn" ng-click="publishValue(row.entity)">Publish</button>' +
                            //'<button value="Edit" class="btn btn-xs btn-warning" ng-if="!row.inlineEdit.isEditModeOn" ng-click="appScopeProvider.publishValue(row.entity)">Publish</button>' +
                            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
                            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
                            '</div>',
                            enableCellEdit: false
                        },
                    ],
                    data: data[i].values,
                    onRegisterApi: function (gridApi) {
                        //set gridApi on scope
                        $scope.gridApi = gridApi;

                        //gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
                        //    //var selectedRows = $scope.gridApi.selection.getSelectedRows();
                        //    //var parentRow = rowEntity.grid.appScope.row;
                        //    var index = data.indexOf(rowEntity.entity);
                        //    if (newValue !== oldValue) {

                        //        rowEntity.state = "Changed";
                        //        //Get column
                        //        var rowCol = $scope.gridApi.cellNav.getFocusedCell().col.colDef.name;

                        //        angular.forEach(selectedRows, function (item) {
                        //            item[rowCol] = rowEntity[rowCol];
                        //            item.state = "Changed";
                        //            item.isDirty = false;
                        //            item.isError = false;
                        //        });
                        //        $scope.subGridOptions.data.splice(index, 1);

                        //        $scope.subGridOptions.data.push({
                        //            "id": rowEntity.id,
                        //            "configvar_id": rowEntity.configvar_id,
                        //            "environment": rowEntity.environment,
                        //            "value": rowEntity.value,
                        //            "create_date": rowEntity.create_date,
                        //            "modify_date": rowEntity.modify_date,
                        //            "publish_date": rowEntity.publish_date,
                        //            "published": rowEntity.published,
                        //        });
                        //    }
                        //});

                        gridApi.rowEdit.on.saveRow($scope, $scope.saveSubGridRow);
                    }
                };
            }
            $scope.gridOptions.data = data;
        });
}
);

ConfigApp.controller('ConfigViewer',
    function ($scope, $element, title, filePath, configXml, close) {

        $scope.filePath = filePath;
        $scope.configXml = configXml;
        $scope.title = title;
        $scope.close = function () {
            close({
                filePath: $scope.filePath,
                configXml: $scope.configXml
            }, 500); // close, but give 500ms for bootstrap to animate
        };
        $scope.cancel = function () {
            $element.modal('hide');
            close({
                filePath: $scope.filePath,
                configXml: $scope.configXml
            }, 500); // close, but give 500ms for bootstrap to animate
        };
    });


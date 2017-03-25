'use strict';
var ConfigApp = angular.module('ConfigApp', ['ui.grid', 'ui.grid.edit', 'ui.grid.grouping', 'ui.grid.saveState',
    'ui.grid.pagination', 'ui.grid.expandable', 'ui.grid.cellNav',
    'ui.grid.selection', 'ui.grid.rowEdit', 'ui.grid.resizeColumns',
    'ui.grid.pinning', 'ui.grid.exporter', 'ui.grid.moveColumns',
    'ui.grid.infiniteScroll', 'ui.grid.importer'
]);
ConfigApp.value('configUrl', 'http://localhost:41999');
var MachineApp = angular.module('MachineApp', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning', 'ui.grid.exporter']);
var LogApp = angular.module('LogApp', ['ui.grid',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning', 'ui.grid.exporter']);
LogApp.value('apiUrl', 'http://localhost:41999');
var ManagerApp = angular.module('ManagerApp', ['ConfigApp', 'LogApp', 'MachineApp']);
ConfigApp.controller('ConfigController', function ($scope, $http, $log, $timeout, uiGridConstants, 
    //uiGridExporterConstants,
    //uiGridGroupingConstants,
    $q, $interval) {
    $scope.title = "Application Configuration";
    var vm = $scope;
    //var data = [];
    var i;
    $scope.gridOptions = {
        enablePaging: true,
        paginationPageSizes: [10, 20, 50, 100],
        paginationPageSize: 25,
        enablePinning: true,
        showGridFooter: true,
        enableSorting: true,
        enableFiltering: true,
        enableHorizontalScrollbar: 0,
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
        enableSelectAll: true,
        enableEditing: true,
        enableColumnResize: false,
        enableCellSelection: false,
        enableRowSelection: true,
        expandableRowTemplate: '<div ui-grid="row.entity.subGridOptions" ui-grid-edit ui-grid-row-edit ui-grid-selection style="height:150px;width:88%; float:right"></div>',
        expandableRowHeight: 150,
        //subGridVariable will be available in subGrid scope
        expandableRowScope: {
            subGridVariable: 'subGridScopeVariable'
        },
        //column definitions
        columnDefs: [
            { field: 'applicationNames', enableCellEdit: false, cellTemplate: basicCellTemplate },
            { field: 'componentId', visible: false, enableCellEdit: false },
            { field: 'componentName', enableCellEdit: false, grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, groupable: true, cellTemplate: basicCellTemplate },
            { field: 'configvar_id', visible: false, enableCellEdit: false },
            { field: 'configParentElement', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'configElement', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            //{ field: 'configAttribute', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'keyName', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'key', groupable: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'valueName', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            //{
            //    field: "Action",
            //    width: 150,
            //    enableCellEdit: false,
            //    enableFiltering: false,
            //    cellTemplate: '<div class="inline-block"><div ng-if="!row.groupHeader"><button id="editBtn" type="button" class="btn btn-xs btn-info"  ng-click="editCell()" >Edit </button>&nbsp<button id="delBtn" type="button" class="btn btn-xs btn-danger"  ng-click="saveRow()" >Save </button>&nbsp<button id="delBtn" type="button" class="btn btn-xs btn-danger"  ng-click="editCell()" >Remove </button></div></div>'
            //    //cellTemplate: '<div><div ng-if="!row.groupHeader"><button id="pubBtn" type="button" class="btn btn-xs btn-primary"  ng-click="publishCell()" >Publish </button></div><div ng-if="!row.groupHeader"><button id="delBtn" type="button" class="btn btn-xs btn-danger"  ng-click="editCell()" >Remove </button></div></div>'
            //}
            {
                name: "Actions",
                cellTemplate: '<div ng-if="!row.groupHeader"><div class="ui-grid-cell-contents" >' +
                    '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Delete</button>' +
                    '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
                    '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
                    '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
                    '</div></div>',
                enableCellEdit: false
            },
        ],
    };
    //var basicCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()" ng-click="editCell(row.entity, row.getProperty(col.field), col.field)"><span class="ui-disableSelection hover">{{row.getProperty(col.field)}}</span></div>';
    //var basicCellTemplate = '<div ng-if="!row.entity.editable">{{COL_FIELD}}</div><div ng-if="row.entity.editable"><input ng-model="MODEL_COL_FIELD"</div>';
    var basicCellTemplate = '<div ng-if="!col.grouping || col.grouping.groupPriority === undefined || col.grouping.groupPriority === null || ( row.groupHeader && col.grouping.groupPriority === row.treeLevel )" class="ui-grid-cell-contents" title="TOOLTIP">{{COL_FIELD CUSTOM_FILTERS}}</div>';
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
    $scope.changeGroupBy = function (group1, group2) {
        $scope.gridOptions.$gridScope.configGroups = [];
        $scope.gridOptions.$gridScope.configGroups.push(group1);
        $scope.gridOptions.$gridScope.configGroups.push(group2);
        $scope.gridOptions.groupBy();
    };
    $scope.clearGroupBy = function () {
        $scope.gridOptions.$gridScope.configGroups = [];
        $scope.gridOptions.groupBy();
    };
    ////api that is called every time
    //// when data is modified on grid for sorting
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        gridApi.rowEdit.on.saveRow($scope, $scope.saveRow);
    };
    //$scope.saveRow = function (rowEntity) {
    //    var promise = $scope.saveRowFunction(rowEntity);
    //    $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
    //};
    //$scope.info = {};
    //$scope.gridOptions.onRegisterApi = function (gridApi) {
    //    //set gridApi on scope
    //    $scope.gridApi = gridApi;
    //    gridApi.selection.on.rowSelectionChanged($scope, function (row) {
    //        var msg = 'row selected ' + row.isSelected;
    //        $log.log(msg);
    //    });
    //    gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
    //        var msg = 'rows changed ' + rows.length;
    //        $log.log(msg);
    //    });
    //    gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
    //        var selectedRows = $scope.gridApi.selection.getSelectedRows();
    //        if (newValue != oldValue) {
    //            rowEntity.state = "Changed";
    //            //Get column
    //            var rowCol = $scope.gridApi.cellNav.getFocusedCell().col.colDef.name;
    //            angular.forEach(selectedRows, function (item) {
    //                item[rowCol] = rowEntity[rowCol];
    //                item.state = "Changed";
    //                item.isDirty = false;
    //                item.isError = false;
    //            });
    //        }
    //    });
    //    gridApi.rowEdit.on.saveRow($scope, $scope.saveRow);
    //};
    //gridApi.expandable.on.rowExpandedStateChanged($scope, function (row) {
    //    if (row.isExpanded) {
    //        row.entity.subGridOptions = [{
    //            virtualizationThreshold: 60,
    //            columnDefs: [{
    //                name: "Actions",
    //                cellTemplate: '<div class="ui-grid-cell-contents" >' +
    //                '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
    //                '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="$scope.publishValue(row.entity)">Publish</button>' +
    //                '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
    //                '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
    //                '</div>',
    //                enableCellEdit: false
    //            },
    //                { name: "id", field: "id", visible: false },
    //                { name: "Variable id", field: "configvar_id", visible: false },
    //                { name: "Environment", field: "environment", visible: true, cellTemplate: basicCellTemplate },
    //                { name: "Value", field: "value", visible: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
    //                { name: "Create Date", field: "create_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
    //                { name: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
    //                { name: "Last Publish Date", field: "publish_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
    //                { name: "Is Published", field: "published", visible: true, enableCellEdit: false, type: 'boolean', width: 50 },
    //            ],
    //            onRegisterApi: function (gridApi) {
    //                $scope.text = gridApi;
    //                gridApi.selection.on.rowSelectionChanged($scope, function (row) {
    //                    console.log(row);
    //                });
    //                gridApi.rowEdit.on.saveRow($scope, $scope.saveRow)
    //            }
    //        }, {
    //                virtualizationThreshold: 60,
    //                columnDefs: [{
    //                    name: "Actions",
    //                    cellTemplate: '<div class="ui-grid-cell-contents" >' +
    //                    '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
    //                    '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="$scope.publishValue(row.entity)">Publish</button>' +
    //                    '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
    //                    '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
    //                    '</div>',
    //                    enableCellEdit: false
    //                },
    //                    { name: "id", field: "id", visible: false },
    //                    { name: "Variable id", field: "configvar_id", visible: false },
    //                    { name: "Environment", field: "environment", visible: true, cellTemplate: basicCellTemplate },
    //                    { name: "Value", field: "value", visible: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
    //                    { name: "Create Date", field: "create_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
    //                    { name: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
    //                    { name: "Last Publish Date", field: "publish_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
    //                    { name: "Is Published", field: "published", visible: true, enableCellEdit: false, type: 'boolean', width: 50 },
    //                ]
    //            }],
    //            $http.get('/api/ConfigValues')
    //                .success(function (data) {
    //                    row.entity.subGridOptions.data = data
    //                });
    //    }
    $scope.saveRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };
    $scope.toggleEdit = function (rowNum) {
        $scope.gridOptions1.data[rowNum].editable = !$scope.gridOptions1.data[rowNum].editable;
        $scope.grid1Api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
    };
    $scope.environments = ["development", "qa", "production"];
    $scope.expandAllRows = function () {
        $scope.gridApi.expandable.expandAllRows();
    };
    $scope.collapseAllRows = function () {
        $scope.gridApi.expandable.collapseAllRows();
    };
    //loadConfigs();
    //function loadConfigs() {
    //    var ConfigRecords = $http.get('/api/ConfigApi');
    //    ConfigRecords.then(function (data) {
    //        $scope.gridOptions = { data: data.data };
    //    })
    //}
    //function loadConfigs() {
    //    var ConfigRecords = $http.get('/api/ConfigApi');
    //    ConfigRecords.then(function (data) {
    $http.get('/api/ConfigApi')
        .success(function (data) {
        for (i = 0; i < data.length; i++) {
            data[i].subGridOptions = {
                appScopeProvider: $scope,
                //virtualizationThreshold: 60,
                columnDefs: [{
                        name: "Actions",
                        cellTemplate: '<div class="ui-grid-cell-contents" >' +
                            '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
                            '<button value="Edit" ng-if="!row.inlineEdit.isEditModeOn" ng-click="$scope.publishValue(row.entity)">Publish</button>' +
                            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
                            '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
                            '</div>',
                        enableCellEdit: false
                    },
                    { name: "id", field: "id", visible: false },
                    { name: "Variable id", field: "configvar_id", visible: false },
                    { name: "Environment", field: "environment", visible: true, cellTemplate: basicCellTemplate },
                    { name: "Value", field: "value", visible: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
                    { name: "Create Date", field: "create_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
                    { name: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
                    { name: "Last Publish Date", field: "publish_date", visible: true, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
                    { name: "Is Published", field: "published", visible: true, enableCellEdit: false, type: 'boolean', width: 50 },
                ],
                data: data[i].values
            };
        }
        $scope.gridOptions.data = data;
    });
});
angular.module('ui.grid').factory('InlineEdit', ['$interval', '$rootScope', 'uiGridRowEditService',
    function ($interval, $rootScope, uiGridRowEditService) {
        function InlineEdit(entity, index, grid) {
            this.grid = grid;
            this.index = index;
            this.entity = {};
            this.isEditModeOn = false;
            this.init(entity);
        }
        InlineEdit.prototype = {
            init: function (rawEntity) {
                var self = this;
                for (var prop in rawEntity) {
                    self.entity[prop] = {
                        value: rawEntity[prop],
                        isValueChanged: false,
                        isSave: false,
                        isCancel: false,
                        isEdit: false
                    };
                }
            },
            enterEditMode: function (event) {
                event && event.stopPropagation();
                var self = this;
                self.isEditModeOn = true;
                // cancel all rows which are in edit mode
                self.grid.rows.forEach(function (row) {
                    if (row.inlineEdit && row.inlineEdit.isEditModeOn && row.uid !== self.grid.rows[self.index].uid) {
                        row.inlineEdit.cancelEdit();
                    }
                });
                // Reset all the values
                for (var prop in self.entity) {
                    self.entity[prop].isSave = false;
                    self.entity[prop].isCancel = false;
                    self.entity[prop].isEdit = true;
                }
            },
            saveEdit: function (event) {
                event && event.stopPropagation();
                var self = this;
                self.isEditModeOn = false;
                for (var prop in self.entity) {
                    self.entity[prop].isSave = true;
                    self.entity[prop].isEdit = false;
                }
                uiGridRowEditService.saveRow(self.grid, self.grid.rows[self.index])();
            },
            cancelEdit: function (event) {
                event && event.stopPropagation();
                var self = this;
                self.isEditModeOn = false;
                for (var prop in self.entity) {
                    self.entity[prop].isCancel = true;
                    self.entity[prop].isEdit = false;
                }
            }
        };
        return InlineEdit;
    }]);

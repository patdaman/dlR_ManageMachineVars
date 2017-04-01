'use strict';
var ConfigApp = angular.module('ConfigApp', ['ui.grid', 'ui.grid.edit', 'ui.grid.grouping', 'ui.grid.saveState',
    'ui.grid.pagination', 'ui.grid.expandable', 'ui.grid.cellNav',
    'ui.grid.selection', 'ui.grid.rowEdit', 'ui.grid.resizeColumns',
    'ui.grid.pinning', 'ui.grid.exporter', 'ui.grid.moveColumns',
    'ui.grid.infiniteScroll', 'ui.grid.importer'
]);
ConfigApp.controller('ConfigController', function ($scope, $http, $log, $timeout, uiGridConstants, $q, $interval) {
    $scope.title = "Application Configuration";
    //var vm = $scope;
    var data = [];
    var i;
    var editMode = false;
    var rowIndex;
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
        enableSelectAll: false,
        enableEditing: false,
        enableColumnResize: false,
        enableCellSelection: false,
        enableRowSelection: true,
        expandableRowTemplate: '<div ui-grid="row.entity.subGridOptions" ui-grid-edit ui-grid-row-edit style="width:100%; float:right"></div>',
        expandableRowHeight: 125,
        expandableRowScope: {
            subGridVariable: 'subGridScopeVariable'
        },
    };
    $scope.gridOptions.columnDefs = [
        { field: 'applicationNames', enableCellEdit: false, cellTemplate: basicCellTemplate },
        { field: 'componentId', visible: false, enableCellEdit: false },
        { field: 'componentName', enableCellEdit: false, grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, groupable: true, cellTemplate: basicCellTemplate },
        { field: 'configvar_id', visible: false, enableCellEdit: false },
        { field: 'configParentElement', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        { field: 'configElement', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        { field: 'attribute', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        { field: 'key', groupable: true, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        { field: 'valueName', visible: false, cellTemplate: basicCellTemplate, cellEditableCondition: 'false' },
        //{
        //    field: "Action",
        //    width: 150,
        //    enableCellEdit: false,
        //    enableFiltering: false,
        //    cellTemplate: '<div class="inline-block"><div ng-if="!row.groupHeader">' +
        //    '<button id="editBtn" ng-if="!row.inlineEdit.isEditModeOn" type= "button" class="btn btn-xs btn-info"  ng-click="toggleEdit()" >Edit </button>' +
        //    '<button id="editBtn" ng-if="!row.inlineEdit.isEditModeOn" type= "button" class="btn btn-xs btn-waring"  ng-click="publish()" >Publish </button>' +
        //    '&nbsp <button id="delBtn" ng-if="row.inlineEdit.isEditModeOn" type="button" class="btn btn-xs btn-danger"  ng-click="saveRow()">Save </button>' +
        //    '&nbsp <button id="delBtn" ng-if="row.inlineEdit.isEditModeOn" type="button" class="btn btn-xs btn-danger"  ng-click="editCell()">Remove </button>' + 
        //    '</div></div>'
        //    //cellTemplate: '<div><div ng-if="!row.groupHeader"><button id="pubBtn" type="button" class="btn btn-xs btn-primary"  ng-click="publishCell()" >Publish </button></div><div ng-if="!row.groupHeader"><button id="delBtn" type="button" class="btn btn-xs btn-danger"  ng-click="editCell()" >Remove </button></div></div>'
        //}
        {
            name: "Actions",
            cellTemplate: '<div ng-if="!row.groupHeader"><div class="ui-grid-cell-contents" >' +
                '<button value="Edit" class="btn btn-xs btn-info" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
                '<button value="Edit" class="btn btn-xs btn-danger" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Delete</button>' +
                '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.saveEdit($event)">Update</button>' +
                '<button value="Edit" ng-if="row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.cancelEdit($event)">Cancel</button>' +
                '</div></div>',
            enableCellEdit: false
        },
    ];
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
    $scope.editable = function (row) {
        $scope.editMode = !editMode;
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
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
        gridApi.selection.on.rowSelectionChanged($scope, function (row) {
            var msg = 'row selected ' + row.isSelected;
            $log.log(msg);
        });
        gridApi.selection.on.rowSelectionChangedBatch($scope, function (rows) {
            var msg = 'rows changed ' + rows.length;
            $log.log(msg);
        });
        gridApi.edit.on.afterCellEdit($scope, function (rowEntity, colDef, newValue, oldValue) {
            var selectedRows = $scope.gridApi.selection.getSelectedRows();
            if (newValue != oldValue) {
                rowEntity.state = "Changed";
                //Get column
                var rowCol = $scope.gridApi.cellNav.getFocusedCell().col.colDef.name;
                angular.forEach(selectedRows, function (item) {
                    item[rowCol] = rowEntity[rowCol];
                    item.state = "Changed";
                    item.isDirty = false;
                    item.isError = false;
                });
            }
        });
        gridApi.rowEdit.on.saveRow($scope, $scope.saveRow);
    };
    $scope.saveRow = function (rowEntity) {
        var promise = $scope.saveRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
    };
    $scope.saveRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigApi/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };
    $scope.toggleEdit = function (rowNum) {
        $scope.gridOptions1.data[rowNum].editable = !$scope.gridOptions1.data[rowNum].editable;
        $scope.grid1Api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
    };
    $scope.saveSubGridRow = function (rowEntity) {
        var promise = $scope.saveSubGridRowFunction(rowEntity);
        $scope.gridApi.rowEdit.setSavePromise(rowEntity, promise);
    };
    $scope.saveSubGridRowFunction = function (row) {
        var deferred = $q.defer();
        $http.post('/api/ConfigValues/', row).success(deferred.resolve).error(deferred.reject);
        return deferred.promise;
    };
    $scope.environments = ["development", "qa", "production"];
    $scope.machines = ["sdsvc01.dc.pti.com", "hqdev07.dev.corp.printable.com", "hqdev08.dev.corp.printable.com"];
    $scope.components = ["Commerce", "DAL", "ManagerI18N", "Services"];
    $scope.expandAllRows = function () {
        $scope.gridApi.expandable.expandAllRows();
    };
    $scope.collapseAllRows = function () {
        $scope.gridApi.expandable.collapseAllRows();
    };
    //$http({
    //    method: "GET",
    //    url: '/api/ConfigApi'
    //}).then(function (data) {
    $http.get('/api/ConfigApi')
        .success(function (data) {
        for (i = 0; i < data.length; i++) {
            data[i].subGridOptions = {
                enableHorizontalScrollbar: 0,
                appScopeProvider: $scope,
                columnDefs: [
                    { name: "id", field: "id", visible: false },
                    { name: "Variable id", field: "configvar_id", visible: false },
                    { name: "Environment", field: "environment", visible: true, cellTemplate: basicCellTemplate },
                    //{ name: "Value", field: "value", visible: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
                    { name: "Value", field: "value", visible: true, cellEditableContition: false, cellTemplate: basicCellTemplate },
                    { name: "Create Date", field: "create_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                    { name: "Modify Date", field: "modify_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                    { name: "Last Publish Date", field: "publish_date", visible: true, enableCellEdit: false, type: 'date', cellFilter: 'date:"MM-dd-yyyy"' },
                    { name: "Is Published", field: "published", visible: true, enableCellEdit: false, type: 'boolean' },
                    {
                        name: "Actions",
                        cellTemplate: '<div class="ui-grid-cell-contents" >' +
                            '<button value="Edit" class="btn btn-xs btn-info" ng-if="!row.inlineEdit.isEditModeOn" ng-click="row.inlineEdit.enterEditMode($event)">Edit</button>' +
                            '<button value="Edit" class="btn btn-xs btn-warning" ng-if="!row.inlineEdit.isEditModeOn" ng-click="appScopeProvider.publishValue(row.entity)">Publish</button>' +
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
                    //    var selectedRows = $scope.gridApi.selection.getSelectedRows();
                    //    var parentRow = rowEntity.grid.appScope.row;
                    //    var index = $scope.subGridOptions.data.indexOf(rowEntity.entity);
                    //    if (newValue != oldValue) {
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
//# sourceMappingURL=ConfigController.js.map
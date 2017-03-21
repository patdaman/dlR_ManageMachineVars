'use strict';
var ConfigApp = angular.module('ConfigApp', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable', 'ui.grid.cellNav',
    'ui.grid.grouping', 'ui.grid.selection', 'ui.grid.rowEdit',
    'ui.grid.selection', 'ui.grid.pinning', 'ui.grid.exporter',
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
ConfigApp.controller('ConfigController', function ($scope, $http, uiGridConstants, uiGridGroupingConstants, uiGridExporterConstants) {
    $scope.title = "Application Configuration";
    var vm = $scope;
    var data = [];
    var machineId;
    var machine_name;
    var location;
    var usage;
    var machineCreate_date;
    var machineModify_date;
    var machineActive;
    var applicationId;
    var applicationName;
    var applicationRelease;
    var componentId;
    var componentName;
    var varId;
    var varType;
    var configParentElement;
    var configElement;
    var configAttribute;
    var keyName;
    var key;
    var configValue_name;
    var valueName;
    var value;
    var varPath;
    var varActive;
    var envType;
    var varCreate_date;
    var varModify_date;
    $scope.gridOptions = {
        enablePaging: true,
        paginationPageSizes: [10, 25, 50, 100],
        paginationPageSize: 25,
        enablePinning: true,
        showGridFooter: true,
        enableSorting: true,
        enableFiltering: true,
        enableGridMenu: true,
        exporterMenuCsv: true,
        exporterMenuPdf: true,
        exporterCsvFilename: 'AppConfig.csv',
        exporterPdfDefaultStyle: { fontSize: 9 },
        exporterPdfTableStyle: { margin: [30, 30, 30, 30] },
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
        //expandableRowTemplate: 'expandableRowTemplate.html',
        //expandableRowHeight: 150,
        ////subGridVariable will be available in subGrid scope
        //expandableRowScope: {
        //    subGridVariable: 'subGridScopeVariable'
        //},
        //column definitions
        columnDefs: [
            //{ displayName: '#', cellTemplate: '{{rowRenderIndex + 1}}' },
            //{ displayName: '#', cellTemplate: '<div>{{$parent.$index + 1}}</div>' },
            { field: 'machineId', visible: false },
            { field: 'machine_name', grouping: { groupPriority: 2 }, sort: { priority: 2, direction: 'asc' }, groupable: true, cellTemplate: basicCellTemplate },
            { field: 'location', groupable: true, cellTemplate: basicCellTemplate },
            { field: 'usage', grouping: { groupPriority: 3 }, sort: { priority: 3, direction: 'asc' }, groupable: true, cellTemplate: basicCellTemplate },
            { field: 'machineCreate_date', visible: false },
            { field: 'machineModify_date', visible: false },
            { field: 'machineActive', visible: false },
            { field: 'applicationId', visible: false },
            { field: 'applicationName', grouping: { groupPriority: 0 }, sort: { priority: 0, direction: 'asc' }, groupable: true, cellTemplate: basicCellTemplate },
            { field: 'applicationRelease', groupable: true, cellTemplate: basicCellTemplate },
            { field: 'componentId', visible: false },
            { field: 'componentName', grouping: { groupPriority: 1 }, sort: { priority: 1, direction: 'asc' }, groupable: true, cellTemplate: basicCellTemplate },
            { field: 'varId', visible: false },
            { field: 'varType', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'configParentElement', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'configElement', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'configAttribute', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'keyName', enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'key', groupable: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'configValue_name', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'valueName', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'value', enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'varPath', visible: false, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'varActive', visible: false, enableCellEdit: true, type: 'boolean', width: '8%' },
            { field: 'envType', visible: false, groupable: true, enableCellEdit: true, cellTemplate: basicCellTemplate },
            { field: 'varCreate_date', visible: false, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
            { field: 'varModify_date', visible: false, enableCellEdit: false, cellFilter: 'date:"MM-dd-yyyy"' },
            {
                field: "Action",
                width: 100,
                enableCellEdit: false,
                enableFiltering: false,
                cellTemplate: '<div><div ng-if="!row.groupHeader"><button id="editBtn" type="button" class="btn btn-xs btn-info"  ng-click="editCell()" >Edit Row </button></div></div>'
            }
        ],
        data: data,
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }
    };
    //var basicCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()" ng-click="editCell(row.entity, row.getProperty(col.field), col.field)"><span class="ui-disableSelection hover">{{row.getProperty(col.field)}}</span></div>';
    var basicCellTemplate = '<div ng-if="!row.entity.editable">{{COL_FIELD}}</div><div ng-if="row.entity.editable"><input ng-model="MODEL_COL_FIELD"</div>';
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
    $scope.filterOptions = {
        filterText: "",
        useExternalFilter: true
    };
    //$scope.gridOptions.sortInfo = {
    //    fields: ['machine_name', 'usage'],
    //    directions: ['asc'],
    //    columns: [0, 1]
    //};
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
    //api that is called every time
    // when data is modified on grid for sorting
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };
    $scope.toggleEdit = function (rowNum) {
        $scope.gridOptions1.data[rowNum].editable = !$scope.gridOptions1.data[rowNum].editable;
        $scope.grid1Api.core.notifyDataChange(uiGridConstants.dataChange.EDIT);
    };
    //Loads all Config records when page loads
    loadConfigs();
    function loadConfigs() {
        var ConfigRecords = $http.get("/api/ConfigApi");
        ConfigRecords.then(function (d) {
            $scope.gridOptions = { data: d.data };
        }, function () {
            //swal("Oops..", "Error occured while loading", "error"); //fail
        });
    }
    $scope.get = loadConfigs();
});
//# sourceMappingURL=ConfigController.js.map
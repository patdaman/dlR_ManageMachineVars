'use strict';
var configApp = angular.module('configApp', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);
configApp.value('configUrl', 'http://localhost:41999');
var machineApp = angular.module('machineApp', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);
var logApp = angular.module('logApp', ['ui.grid',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);
logApp.value('apiUrl', 'http://localhost:41999');
var managerApp = angular.module('managerApp', ['configApp', 'logApp', 'machineApp']);
//configApp.controller('ConfigController', ['$scope', '$http', 'configcrudservice', 'uiGridConstants',
//    function ($scope, $http, configcrudservice, uiGridConstants) {
configApp.controller('ConfigController', function ($scope, $http, uiGridConstants) {
    $scope.title = "Application Configuration";
    var vm = $scope;
    var configVars;
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
        pagingOptions: $scope.pagingOptions,
        enablePinning: true,
        showFooter: true,
        enableSorting: true,
        enableFiltering: true,
        enableEditing: true,
        enableColumnResize: true,
        enableCellSelection: true,
        //expandableRowTemplate: 'expandableRowTemplate.html',
        //expandableRowHeight: 150,
        ////subGridVariable will be available in subGrid scope
        //expandableRowScope: {
        //    subGridVariable: 'subGridScopeVariable'
        //},
        //column definitions
        //we can specify sorting mechnism also
        ColumnDefs: [
            { displayName: '#', cellTemplate: '{{rowRenderIndex + 1}}' },
            //{ displayName: '#', cellTemplate: '<div>{{$parent.$index + 1}}</div>' },
            { field: 'machineId', visible: false },
            { field: 'machine_name', groupable: true, cellTemplate: basicCellTemplate },
            { field: 'location', groupable: true, cellTemplate: basicCellTemplate },
            { field: 'usage', groupable: true, cellTemplate: basicCellTemplate },
            { field: 'machineCreate_date', visible: false },
            { field: 'machineModify_date', visible: false },
            { field: 'machineActive', visible: false },
            { field: 'applicationId', visible: false },
            { field: 'applicationName', groupable: true, cellTemplate: basicCellTemplate },
            { field: 'applicationRelease', cellTemplate: basicCellTemplate },
            { field: 'componentId', visible: false },
            { field: 'componentName', groupable: true, cellTemplate: basicCellTemplate },
            { field: 'varId', visible: false },
            { field: 'varType', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'configParentElement', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'configElement', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'configAttribute', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'keyName', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'key', groupable: true, enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'configValue_name', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'valueName', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'value', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'varPath', enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'varActive', enableEditing: true },
            { field: 'envType', groupable: true, enableEditing: true, cellTemplate: basicCellTemplate },
            { field: 'varCreate_date' },
            { field: 'varModify_date' },
            {
                field: "Action",
                width: 200,
                enableCellEdit: false,
                cellTemplate: '<button id="editBtn" type="button" class="btn btn-xs btn-info"  ng-click="updateCell()" >Click a Cell for Edit </button>'
            }
        ],
        onRegisterApi: function (gridApi) {
            $scope.gridApi = gridApi;
        }
    };
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
    var basicCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()" ng-click="editCell(row.entity, row.getProperty(col.field), col.field)"><span class="ui-disableSelection hover">{{row.getProperty(col.field)}}</span></div>';
    $scope.filterOptions = {
        filterText: "",
        useExternalFilter: true
    };
    //$scope.gridOptions.sortInfo = {
    //    fields: ['machine_name', 'usage'],
    //    directions: ['asc'],
    //    columns: [0, 1]
    //};
    $scope.pagingOptions = {
        pageSizes: [5, 10, 20],
        pageSize: 5,
        currentPage: 1
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
    //api that is called every time
    // when data is modified on grid for sorting
    $scope.gridOptions.onRegisterApi = function (gridApi) {
        $scope.gridApi = gridApi;
    };
    //Loads all Config records when page loads
    loadConfigs();
    function loadConfigs() {
        var ConfigRecords = $http.get("/api/ConfigApi");
        ConfigRecords.then(function (d) {
            $scope.gridOptions.data = d.data;
        }, function () {
            //swal("Oops..", "Error occured while loading", "error"); //fail
        });
    }
    //$scope.get = function () {
    //    return $http.get("/api/ConfigApi");
    //}
});

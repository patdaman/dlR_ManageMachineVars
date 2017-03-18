
'use strict' 

configApp.controller('ConfigController', ['$scope', '$http', 'configcrudservice',
    function ($scope, $http, configcrudservice) {
        $scope.title = "Application Configuration";
        var configVars

        var machineId
        var machine_name
        var location
        var usage
        var machineCreate_date
        var machineModify_date
        var machineActive
        var applicationId
        var applicationName
        var applicationRelease
        var componentId
        var componentName
        var varId
        var varType
        var configParentElement
        var configElement
        var configAttribute
        var keyName
        var key
        var configValue_name
        var valueName
        var value
        var varPath
        var varActive
        var envType
        var varCreate_date
        var varModify_date

        $scope.configGridOptions = {
            enablePaging: true,
            //paginationPageSizes: [10, 25, 50, 75],
            //paginationPageSize: 10,
            pagingOptions: $scope.pagingOptions,

            enablePinning: true,
            showFooter: true,
            enableSorting: true,
            enableFiltering: true,

            enableEditing: true,
            enableColumnResize: true,
            enableCellSelection: true,

            expandableRowTemplate: 'expandableRowTemplate.html',
            expandableRowHeight: 150,
            //subGridVariable will be available in subGrid scope
            expandableRowScope: {
                subGridVariable: 'subGridScopeVariable'
            },

            //column definitions
            //we can specify sorting mechnism also
            ColumnDefs: [
	            { field: 'machineId' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'machine_name' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'location' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'usage' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'machineCreate_date' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'machineModify_date' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'machineActive' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'applicationId' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'applicationName' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'applicationRelease' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'componentId' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'componentName' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'varId' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'varType' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'configParentElement' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'configElement' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'configAttribute' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'keyName' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'key' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'configValue_name' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'valueName' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'value' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'varPath' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'varActive' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'envType' , enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'varCreate_date', enableEditing: true, cellTemplate: basicCellTemplate },
	            { field: 'varModify_date' },
                {
                    field: "Action",
                    width: 200,
                    enableCellEdit: false,
                    cellTemplate: '<button id="editBtn" type="button" class="btn btn-xs btn-info"  ng-click="updateCell()" >Click a Cell for Edit </button>'
                }
            ],

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

        $scope.configGridOptions.sortInfo = {
            fields: ['machine_name', 'usage'],
            directions: ['asc'],
            columns: [0, 1]
        };

        //$scope.totalServerItems = 0;

        $scope.pagingOptions = {
            pageSizes: [5, 10, 20],
            pageSize: 5,
            currentPage: 1
        };

        $scope.changeGroupBy = function (group1, group2) {
            $scope.configGridOptions.$gridScope.configGroups = [];
            $scope.configGridOptions.$gridScope.configGroups.push(group1);
            $scope.configGridOptions.$gridScope.configGroups.push(group2);
            $scope.configGridOptions.groupBy();
        }
        $scope.clearGroupBy = function () {
            $scope.configGridOptions.$gridScope.configGroups = [];
            $scope.configGridOptions.groupBy();
        }

        //api that is called every time
        // when data is modified on grid for sorting
        $scope.configGridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        }

        //Loads all Machine records when page loads
        loadConfigs();
        function loadConfigs() {
            var ConfigRecords = $http.get("/api/ConfigApi");
            ConfigRecords.then(function (d) {     //success
                $scope.configGridOptions.data = d.data;
            },
                function () {
                    //swal("Oops..", "Error occured while loading", "error"); //fail
                });
        }

        $scope.get = function () {
            return $http.get("/api/ConfigApi");
        }

        loadVars();
        function loadVars() {
            $scope.configVars = $http.get($scope.apiUrl + "/api/LogApi");
            $scope.configVars.then(function (d) {     //success
                $scope.configGridOptions.data = d.data;
            },
                function () {
                    //swal("Oops..", "Error occured while loading", "error"); //fail
                });
        }

        $scope.get = function (ConfigVar) {
            //debugger;
            var singlerecord = $http.get("/api/ConfigApi/"); // + Machine.id);
            var singlerecord = $http.get("/api/ConfigApi/" + Machine.id);
            singlerecord.then(function (d) {
                // debugger;
                var record = d.data;
                $scope.Updateid = record.id;
                $scope.Updatemachine_name = record.machine_name;
                $scope.Updatelocation = record.location;
                $scope.Updateusage = record.usage;
                $scope.Updatecreate_date = record.create_date;
                $scope.Updatemodify_date = record.modify_date;
                $scope.Updateactive = record.active;
            },
                function () {
                    //swal("Oops...", "Error occured while getting record", "error");
                });
        }
        $scope.configGridOptions.data = $scope.Configs;
    });
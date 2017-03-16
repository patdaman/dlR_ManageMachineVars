'use strict'

logApp.controller('LogController', ['$scope', '$http', 'uiGridConstants', 'logBackendHubProxy',
    function ($scope, $http, uiGridConstants, logBackendHubProxy) {
        $scope.title = "RollingLogs ";
        var loggingDataHub = logBackendHubProxy(logBackendHubProxy.defaultServer, 'EventHub');
        var entry = [];

        var vm = $scope;
        var id;
        var device_id;
        var Hostname;
        var Date;
        var Time;
        var Priority;
        var Message;
        var Device;

        //loggingDataHub.on('broadcastEvents', function (data) {
        //    var logEntry = [];
        //    data.forEach(function (dataItem) {
        //        $scope.id = dataItem.id;
        //        $scope.device_id = dataItem.device_id;
        //        $scope.Hostname = dataItem.Hostname;
        //        $scope.Date = dataItem.Date;
        //        $scope.Time = dataItem.Time;
        //        $scope.Priority = dataItem.Priority;
        //        $scope.Message = dataItem.Message;
        //    });
        //});

        $scope.submit = function () {
            id = $scope.id;
            device_id = $scope.device_id;
            Hostname = $scope.Hostname;
            Date = $scope.Date;
            Time = $scope.Time;
            Priority = $scope.Priority;
            Message = $scope.Message;
            $scope.myData.push({
                id: id,
                device_id: device_id,
                Hostname: Hostname,
                Date: Date,
                Time: Time,
                Priority: Priority,
                Message: Message
            });
        };

        $scope.gridOptions = {
            enablePaging: true,
            pagingOptions: $scope.pagingOptions,

            enablePinning: true,
            showFooter: true,
            enableSorting: true,
            enableFiltering: true,

            enableColumnResize: true,
            enableCellSelection: true,

            expandableRowTemplate: 'expandableRowTemplate.html',
            expandableRowHeight: 150,
            expandableRowScope: {
                subGridVariable: 'subGridScopeVariable'
            },

            //column definitions
            //we can specify sorting mechnism also
            columnDefs: [
                { field: 'id', visible: false },
                { field: 'device_id', visible: false },
                { field: 'Hostname', cellTemplate: basicCellTemplate },
                { field: 'Date' },
                { field: 'Time' },
                { field: 'Priority' },
                { field: 'Message' },
            ],
            onRegisterApi: function (gridApi) {
                $scope.gridApi = gridApi;
            }
        };

        function hideIdColumn(columns) {
            columns.forEach(function (column) {
                if (column.field === '_id') {
                    column.visible = false;
                }
            });
            return columns;
        }

        $scope.toggleVisible = function () {
            $scope.columns[0].visible = !($scope.columns[0].visible || $scope.columns[0].visible === undefined);
            $scope.gridApi.core.notifyDataChange(uiGridConstants.dataChange.COLUMN);
        }

        $scope.selectedCell;
        $scope.selectedRow;
        $scope.selectedColumn;

        var basicCellTemplate = '<div class="ngCellText" ng-class="col.colIndex()" ><span class="ui-disableSelection hover">{{row.getProperty(col.field)}}</span></div>';

        $scope.filterOptions = {
            filterText: "",
            useExternalFilter: true
        };

        $scope.gridOptions.sortInfo = {
            fields: ['Date', 'Time'],
            directions: ['desc'],
            columns: [4, 5]
        };

        $scope.pagingOptions = {
            pageSizes: [5, 10, 20],
            pageSize: 5,
            currentPage: 1
        };

        $scope.changeGroupBy = function (group1, group2) {
            $scope.gridOptions.$gridScope.configGroups = [];
            $scope.gridOptions.$gridScope.configGroups.push(group1);
            $scope.gridOptions.$gridScope.configGroups.push(group2);
            $scope.gridOptions.groupBy();
        }
        $scope.clearGroupBy = function () {
            $scope.gridOptions.$gridScope.configGroups = [];
            $scope.gridOptions.groupBy();
        }

        //api that is called every time
        // when data is modified on grid for sorting
        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        }
        $scope.refresh = loadLogs();
        loadLogs();
        function loadLogs() {
            //var EventRecords = $http.get(apiUrl + "/api/LogApi");
            var EventRecords = $http.get("http://localhost:4999/api/LogApi");
            EventRecords.then(function (d) {     //success
                $scope.gridOptions.data = d.data;
            },
                function () {
                    //swal("Oops..", "Error occured while loading", "error"); //fail
                });
        }

        $scope.get = function () {
            return $http.get("/api/LogApi");
        }

        $scope.gridOptions.data = $scope.EventRecords;
    }
    ]);
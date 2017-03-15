'use strict';

app.controller('ServerTimeController', ['$scope', 'backendHubProxy',
    function ServerTimeController($scope, backendHubProxy) {
        var clientPushHubProxy = backendHubProxy(backendHubProxy.defaultServer, 'performanceHub', { logging: true });
        var serverTimeHubProxy = backendHubProxy(backendHubProxy.defaultServer, 'performanceHub');

        clientPushHubProxy.on('serverTime', function (data) {
            $scope.currentServerTime = data;
            var x = clientPushHubProxy.connection.id;
        });

        $scope.getServerTime = function () {
            serverTimeHubProxy.invoke('getServerTime', function (data) {
                $scope.currentServerTimeManually = data;
            });
        };
    }
]);


app.controller('PerformanceDataController', ['$scope', 'backendHubProxy',
    function ($scope, backendHubProxy) {
        var performanceDataHub = backendHubProxy(backendHubProxy.defaultServer, 'performanceHub');
        var entry = [];

        $scope.currentRamNumber = 0;
        $scope.realtimeLine = generateLineData();
        $scope.realtimeBar = generateLineData();
        $scope.realtimeArea = generateLineData();
        $scope.options = { thickness: 10, mode: 'gauge', total: 100 };
        $scope.data = [
            //{ label: 'CPU', value: 78, color: '#d62728', suffix: '%' }
            { label: 'CPU', value: 0, color: '#d62728', suffix: '%' }
        ];

        $scope.ramGaugeoptions = { thickness: 10, mode: 'gauge', total: 100 };
        $scope.ramGaugeData = [
            { label: 'RAM', value: 0, color: '#1f77b4', suffix: '%' }
        ];
        $scope.currentRamNumber = 0;
        //$scope.realtimeLineFeed = entry;


        performanceDataHub.on('broadcastPerformance', function (data) {
            var timestamp = ((new Date()).getTime() / 1000) | 0;
            var chartEntry = [];
            data.forEach(function (dataItem) {

                switch(dataItem.counterName) {
                    case '% Processor Time':
                        $scope.cpuData = dataItem.value;
                        chartEntry.push({ time: timestamp, y: dataItem.value });
                        $scope.data = [
                            { label: 'CPU', value: dataItem.value, color: '#d62728', suffix: '%' }
                        ];
                        break;
                    case 'Available MBytes':
                        $scope.memData = dataItem.value;
                        chartEntry.push({ time: timestamp, y: dataItem.value });
                        $scope.ramGaugeData = [
                            { label: 'RAM', value: dataItem.value, color: '#1f77b4', suffix: '%' }
                        ];
                        $scope.currentRamNumber = dataItem.value;
                        break;
                    case 'Bytes Received/sec':
                        $scope.netInData = dataItem.value.toFixed(2);
                        chartEntry.push({ time: timestamp, y: dataItem.value });
                        break;
                    case 'Bytes Sent/sec':
                        $scope.netOutData = dataItem.value.toFixed(2);
                        chartEntry.push({ time: timestamp, y: dataItem.value });
                        break;
                    case 'Disk Reads/sec':
                        $scope.diskReaddData = dataItem.value.toFixed(3);
                        chartEntry.push({ time: timestamp, y: dataItem.value });
                        break;
                    case 'Disk Writes/sec':
                        $scope.diskWriteData = dataItem.value.toFixed(3);
                        chartEntry.push({ time: timestamp, y: dataItem.value });
                        break;
                    default:
                        break;
                    //default code block
                }
            });
            $scope.realtimeLineFeed = chartEntry;
            $scope.realtimeBarFeed = chartEntry;
            $scope.realtimeAreaFeed = chartEntry;
       
        });

        function generateLineData() {
            var data1 = [{ label: 'Layer 1', values: [] }];
            for (var i = 0; i <= 128; i++) {
                var x = 20 * (i / 128) - 10,
                    y = Math.cos(x) * x;
                data1[0].values.push({ x: x, y: y });
            }
            var data2 = [
                { label: 'Layer 1', values: [] },
                { label: 'Layer 2', values: [] },
                { label: 'Layer 3', values: [] }
            ];
            for (var i = 0; i < 256; i++) {
                var x = 40 * (i / 256) - 20;
                data2[0].values.push({ x: x, y: Math.sin(x) * (x / 4) });
                data2[1].values.push({ x: x, y: Math.cos(x) * (x / Math.PI) });
                data2[2].values.push({ x: x, y: Math.sin(x) * (x / 2) });
            }
            return data2;
        }

        $scope.areaAxes = ['left','right','bottom'];
        $scope.lineAxes = ['right','bottom'];
        $scope.scatterAxes = ['left','right','top','bottom'];
    }
]);


app.controller('LogController', ['$scope', 'logBackendHubProxy',
    function ($scope, $http, uiGridConstants, logBackendHubProxy) {
        $scope.title = "RollingLogs ";
        var loggingDataHub = logBackendHubProxy(logBackendHubProxy.defaultServer, 'loggingHub');
        var entry = [];

        var id;
        var device_id;
        var Hostname;
        var Date;
        var Time;
        var Priority;
        var Message;
        var Device;

        loggingDataHub.on('broadcastLogging', function (data) {
            var logEntry = [];
            data.forEach(function (dataItem) {
                $scope.id = dataItem.id;
                $scope.device_id = dataItem.device_id;
                $scope.Hostname = dataItem.Hostname;
                $scope.Date = dataItem.Date;
                $scope.Time = dataItem.Time;
                $scope.Priority = dataItem.Priority;
                $scope.Message = dataItem.Message;
            });
        });

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

        $scope.get = function () {
            return $http.get("/api/LogApi");
        }

        loadLogs();
        function loadLogs() {
            var EventRecords = $http.get("/api/LogApi");
            EventRecords.then(function (d) {     //success
                $scope.gridOptions.data = d.data;
            },
                function () {
                    //swal("Oops..", "Error occured while loading", "error"); //fail
                });
        }

        $scope.gridOptions.data = $scope.Machines;
    }]);
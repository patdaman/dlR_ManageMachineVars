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


app.controller('PowerShellScriptController', ['$scope', 'backendHubProxy',
    function ($scope, backendHubProxy) {
        var performanceDataHub = backendHubProxy(backendHubProxy.defaultServer, 'logHub');
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

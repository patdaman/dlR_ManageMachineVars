'use strict';

var app = angular.module('angularServiceDashboard',
    ['ng.epoch', 'n3-pie-chart',
    'ui.grid',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);

app.value('backendServerUrl', 'http://localhost:41999/signalr/performance');
app.value('logBackendServerUrl', 'http://localhost:41999/signalr/logging');
//app.value('apiUrl', 'http://localhost:29452');
app.value('apiUrl', 'http://localhost:41999');
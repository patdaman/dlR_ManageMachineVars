'use strict';

var dashboardApp = angular.module('dashboardApp',
    ['ng.epoch', 'n3-pie-chart']);

dashboardApp.value('backendServerUrl', 'http://localhost:41998');

var logApp = angular.module('logApp',
                            ['ui.grid',
                            'ui.grid.pagination', 'ui.grid.expandable',
                            'ui.grid.selection', 'ui.grid.pinning']);

logApp.value('logBackendServerUrl', 'http://localhost:41999/signalr/logging');
//logApp.value('apiUrl', 'http://localhost:29452');
logApp.value('apiUrl', 'http://localhost:41999');

var app = angular.module('app', ['dashboardApp', 'logApp']);
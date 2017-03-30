'use strict';

var configApp = angular.module('configApp',
    ['ui.grid', 'ui.grid.edit',
        'ui.grid.pagination', 'ui.grid.expandable',
        'ui.grid.selection', 'ui.grid.pinning']);

configApp.value('configUrl', 'http://localhost:41999');

var machineApp = angular.module('machineApp', ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);

var logApp = angular.module('logApp',
    ['ui.grid',
        'ui.grid.pagination', 'ui.grid.expandable',
        'ui.grid.selection', 'ui.grid.pinning']);

logApp.value('apiUrl', 'http://localhost:41999');


var managerApp = angular.module('managerApp', ['configApp', 'logApp']);


var dashboardApp = angular.module('dashboardApp',
    ['ng.epoch', 'n3-pie-chart']);
dashboardApp.value('backendServerUrl', 'http://localhost:41999/signalr/performance');

var app = angular.module('app', ['dashboardApp', 'logApp']);
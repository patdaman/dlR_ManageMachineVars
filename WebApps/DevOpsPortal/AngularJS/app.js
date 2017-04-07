'use strict'

var ConfigApp = angular.module('ConfigApp',
        ['ui.grid',
            'ui.grid.edit',
            'ui.grid.grouping',
            'ui.grid.saveState',
            'ui.grid.pagination',
            'ui.grid.expandable',
            'ui.grid.cellNav',
            'ui.grid.selection',
            'ui.grid.rowEdit',
            'ui.grid.resizeColumns',
            'ui.grid.pinning',
            'ui.grid.exporter',
            'ui.grid.moveColumns',
            'ui.grid.infiniteScroll',
            'ui.grid.importer',
            'angularModalService',
            'ngAnimate',
            'ui.bootstrap'
        ]);

//ConfigApp.value('configUrl', 'http://localhost:41999');
ConfigApp.value('configUrl', '');

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
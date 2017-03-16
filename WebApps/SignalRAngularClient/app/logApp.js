'use strict'

var logApp = angular.module('angularServiceDashboardLogs',
                            ['ui.grid',
                            'ui.grid.pagination', 'ui.grid.expandable',
                            'ui.grid.selection', 'ui.grid.pinning']);

logApp.value('logBackendServerUrl', 'http://localhost:41999/signalr/logging');
//logApp.value('apiUrl', 'http://localhost:29452');
logApp.value('apiUrl', 'http://localhost:41999');
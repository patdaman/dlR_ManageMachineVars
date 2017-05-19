'use strict'

var DevOpsWebApp;
var ApiPath = DevOpsWebApp.ApiPath;
var DashboardSignalRPath = DevOpsWebApp.DashboardSignalRPath;
var LogSignalRPath = DevOpsWebApp.LogSignalRPath;
var UserName = DevOpsWebApp.UserName;
var displayApi = DevOpsWebApp.DisplayApi;

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
            'ui.router',
            'ngFileUpload',
            'ngclipboard',
            'ui.bootstrap',
            'ngAnimate',
            'angularModalService'
        ]);

var MachineApp = angular.module('MachineApp',
        [
            'ui.grid',
            'ui.grid.edit',
            'ui.grid.pagination',
            'ui.grid.expandable',
            'ui.grid.cellNav',
            'ui.grid.grouping',
            'ui.grid.selection',
            'ui.grid.rowEdit',
            'ui.grid.pinning',
            'ui.grid.exporter',
            //'ngclipboard',
            'ui.bootstrap',
            'ngAnimate',
            'angularModalService'
        ]);

var LogApp = angular.module('LogApp',
        [
            'ui.grid',
            'ui.grid.pagination',
            'ui.grid.expandable',
            'ui.grid.selection',
            'ui.grid.pinning'
        ]);

var PowershellApp = angular.module('PowershellApp',
        [
            'ngFileUpload',
            'ui.codemirror',
            //'ngclipboard',
            'ui.bootstrap',
            'ngAnimate',
            'angularModalService'
        ]);

var DashboardApp = angular.module('DashboardApp',
      [
          'ng.epoch',
          'n3-pie-chart'
      ]);

// Todo:
// Get rid of these with
// new values from $rootScope
DashboardApp.value('backendServerUrl', 'http://localhost:41999/signalr/performance');
LogApp.value('logBackendServerUrl', 'http://localhost:41999/signalr/logging');

var appHandler = angular.module('appHandler', []);

///  ----------------------------------------------------------- ///
/// <summary>   The application. </summary>
///
///  ----------------------------------------------------------- ///
var app = angular.module('app',
    ['appHandler', 'ConfigApp', 'MachineApp', 'PowershellApp']);

app.run(['$rootScope', function ($rootScope) {
    $rootScope.APIPath = ApiPath;
    $rootScope.UserName = DevOpsWebApp.UserName;
    $rootScope.displayApi = DevOpsWebApp.DisplayApi;
}]);

///  ----------------------------------------------------------- ///
/// <summary>   The signal r application. </summary>
///
///  ----------------------------------------------------------- ///
var signalRApp = angular.module('signalRApp',
  ['appHandler', 'LogApp', 'DashboardApp']);

signalRApp.run(['$rootScope', function ($rootScope) {
    $rootScope.APIPath = ApiPath;
    $rootScope.DashboardSignalRPATH = DashboardSignalRPath;
    $rootScope.LogSignalRPath = DevOpsWebApp.LogSignalRPath;
    $rootScope.UserName = DevOpsWebApp.UserName;
    $rootScope.displayApi = DevOpsWebApp.DisplayApi;
}])


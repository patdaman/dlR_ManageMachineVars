'use strict'

var DevOpsWebApp;
var ApiPath = DevOpsWebApp.ApiPath;
var SignalRPath = DevOpsWebApp.SignalRPath;

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
            'angularModalService',
            'ngAnimate',
            'ui.bootstrap',
            //'ngclipboard',
            'ngFileUpload'
        ])

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
            'ui.grid.exporter'
        ]);

var LogApp = angular.module('LogApp',
        [
            'ui.grid',
            'ui.grid.pagination',
            'ui.grid.expandable',
            'ui.grid.selection',
            'ui.bootstrap',
            'ui.grid.pinning'
        ]);

var PowershellApp = angular.module('PowershellApp',
        [
            'angularModalService',
            'ui.bootstrap',
            'ngAnimate',
            'ngFileUpload',
            'ui.codemirror'
        ]);

//var dashboardApp = angular.module('dashboardApp',
//      [
//          'ng.epoch',
//          'n3-pie-chart'
//      ]);


///  ----------------------------------------------------------- ///
/// <summary>   The application. </summary>
///
///  ----------------------------------------------------------- ///
var app = angular.module('app', ['ConfigApp', 'LogApp', 'MachineApp', 'PowershellApp', 'ngclipboard']);
//var app = angular.module('app', ['ConfigApp', 'logApp', 'machineApp', 'PowershellApp', 'dashboardApp']);

app.run(['$rootScope', function ($rootScope) {
    $rootScope.APIPath = ApiPath;
    $rootScope.SignalRPATH = SignalRPath;
}]);
///  ----------------------------------------------------------- ///
/// <summary>   The application. </summary>
///
///  ----------------------------------------------------------- ///


// http interceptor to add hostname [] are for minification safety
app.factory('httpAPIPathAdder', [function () {
    return {
        request: function (config) {
            if (config.url.search("api:") === 0)
                config.url = ApiPath + config.url.slice(4);
            return config;
        },
        responseError: function (rejection) {
            return $q.reject(rejection);
        }
    }
}]);

// for CORS
app.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.interceptors.push('httpAPIPathAdder');
}]);

// error handling [] are for minification safety
app.factory('$exceptionHandler', [function () {
    return function (exception, cause) {
        if (exception) {
            if (exception.message) {
                var showstr = exception.message;
                if (cause)
                    showstr = showstr + "\nCause: " + cause;
                swal({
                    title: "Application Error",
                    text: showstr,
                    type: "error",
                    confirmButtonText: "Cool"
                });
                console.log(showstr);
                //alert(showstr);
            }
            else {
                console.log(exception);
                alert(exception);
            }
        }
        else {
            if (cause) {
                console.log(cause);
                swal({
                    title: "Application Error",
                    text: cause,
                    type: "error",
                    confirmButtonText: "Cool"
                });
                //alert(cause);
            }
            else {
                console.log("Unknown Exception");
                swal({
                    title: "Application Error",
                    text: "Unknown Exception",
                    type: "error",
                    confirmButtonText: "Cool"
                });
                //alert("Unknown Exception");
            }
            alert("Unknown Exception")
        }
    }
}]);
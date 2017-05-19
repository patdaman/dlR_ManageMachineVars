'use strict'

var DevOpsWebApp;
var ApiPath = DevOpsWebApp.ApiPath;
var EnumPath = DevOpsWebApp.EnumPath;
var SignalRPath = DevOpsWebApp.SignalRPath;
var UserName = DevOpsWebApp.UserName;
var displayApi = DevOpsWebApp.DisplayApi;

var EnumService = angular.module('EnumService', []);

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
            'angularModalService',
            'EnumService'
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
            'ui.grid.exporter',
            'ngclipboard',
            'ui.bootstrap',
            'ngAnimate',
            'angularModalService',
            'EnumService'
        ]);

var LogApp = angular.module('LogApp',
        [
            'ui.grid',
            'ui.grid.pagination',
            'ui.grid.expandable',
            'ui.grid.selection',
            'ui.grid.pinning',
            'EnumService'
        ]);

var PowershellApp = angular.module('PowershellApp',
        [
            'ngFileUpload',
            'ui.codemirror',
            'ngclipboard',
            'ui.bootstrap',
            'ngAnimate',
            'angularModalService',
            'EnumService'
        ]);

//var dashboardApp = angular.module('dashboardApp',
//      [
//          'ng.epoch',
//          'n3-pie-chart'
//          'EnumService'
//      ]);


///  ----------------------------------------------------------- ///
/// <summary>   The application. </summary>
///
///  ----------------------------------------------------------- ///
var app = angular.module('app',
    ['ConfigApp', 'LogApp', 'MachineApp', 'PowershellApp']);
//var app = angular.module('app', ['ConfigApp', 'logApp', 'machineApp', 'PowershellApp', 'dashboardApp']);

app.run(['$rootScope', function ($rootScope) {
    $rootScope.APIPath = ApiPath;
    $rootScope.EnumPath = EnumPath;
    $rootScope.SignalRPath = SignalRPath;
}]);
///  ----------------------------------------------------------- ///
/// <summary>   The application. </summary>
///
///  ----------------------------------------------------------- ///


// http interceptor to add hostname [] are for minification safety
app.factory('httpAPIPathAdder', ['$q', '$location', function ($q, $location) {
    return {
        request: function (config) {
            if (config.url.search("api:") === 0)
                config.url = ApiPath + config.url.slice(4);
            var endInteger = config.url.match(/\d+$/);
            if (endInteger) {
                var integerLength = config.url.length - endInteger[0].length;
                if (config.url.charAt(integerLength) != '/')
                    var endUrl = config.url.slice(-(integerLength)) + '/' + endInteger[0];
                };
            return config;
        },
        requestError: function (config) {
            if (config.status === 401) {
                $location.path('/home');
            }
            else {
                swal({
                    title: "Application Error",
                    text: config,
                    type: "error",
                    confirmButtonText: "Cool"
                });
                console.log(config);
            }
            return config;
        },
        response: function (res) {
            return res;
        },
        responseError: function (res) {
            if (res.status === 401) {
                $location.path('/home');
            }
            return res;
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
                if (exception.stack)
                    showstr = showstr + "\nStack: " + exception.stack;
                //if (cause)
                //    showstr = showstr + "\nCause: " + cause;
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
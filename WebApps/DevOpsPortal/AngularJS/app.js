'use strict'

var DevOpsWebApp;
var AppBuildTag = DevOpsWebApp.AppBuildTag;
var ApiPath = DevOpsWebApp.ApiPath;
var SignalRPath = DevOpsWebApp.SignalRPath;
var TitleTag = DevOpsWebApp.TitleTag;

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
            //'ui.router',
            'angularModalService',
            'ngAnimate',
            'ui.bootstrap',
            'ngClickCopy',
            'ngFileUpload'
        ])

var machineApp = angular.module('machineApp',
    ['ui.grid', 'ui.grid.edit',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);

var logApp = angular.module('logApp',
    ['ui.grid',
    'ui.grid.pagination', 'ui.grid.expandable',
    'ui.grid.selection', 'ui.grid.pinning']);

//var dashboardApp = angular.module('dashboardApp',
//    ['ng.epoch', 'n3-pie-chart']);


///  ----------------------------------------------------------- ///
/// <summary>   The application. </summary>
///
///  ----------------------------------------------------------- ///
var app = angular.module('app', ['ConfigApp', 'logApp', 'machineApp']);
//var app = angular.module('app', ['ConfigApp', 'logApp', 'machineApp', 'dashboardApp']);

app.run(['$rootScope', function ($rootScope) {
    $rootScope.AppBuildStatus = TitleTag;
    $rootScope.APIPath = ApiPath;
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
                console.log(showstr);
                alert(showstr);
            }
            else if (exception.Message) {
                var Showstr = exception.Message;
                if (cause)
                    Showstr = Showstr + "\nCause: " + cause;
                console.log(Showstr);
                alert(Showstr);
            }
            else {
                console.log(exception);
                alert(exception);
            }
        }
        else {
            if (cause) {
                console.log(cause);
                alert(cause);
            }
            else {
                console.log("Unknown Exception");
                alert("Unknown Exception");
            }
            alert("Unknown Exception")
        }
    }
}]);
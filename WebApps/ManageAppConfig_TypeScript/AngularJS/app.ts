///-------------------------------------------------------------------------------------------------
// <summary>application class</summary>
///-------------------------------------------------------------------------------------------------
'use strict';

declare var ManageAppConfig_Var: any;

(function () {

    var ApiPath: string = ManageAppConfig_Var.ApiPath;
    var TitleTag: string = ManageAppConfig_Var.TitleTag;

    var mainApp = angular.module('ManageAppConfig_Typescript', ['ngRoute'])
        .run(['$rootScope', function ($rootScope) {
            $rootScope.APIPath = ApiPath;
        }]);

    // error handling [] are for minification safety
    mainApp.factory('$exceptionHandler', [function () {
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
                    var showstr = exception.Message;
                    if (cause)
                        showstr = showstr + "\nCause: " + cause;
                    console.log(showstr);
                    alert(showstr);
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


    // routing
    mainApp.config(['$routeProvider', function ($routeProvider) {
        $routeProvider
            .when('/', {
                templateUrl: '/Home/Home',
                caseInsensitiveMatch: true
            })
            .when('/home',
            {
                templateUrl: '/Home/Home',
                caseInsensitiveMatch: true
            })
            .when('/about',
            {
                templateUrl: '/Home/About',
                caseInsensitiveMatch: true
            })
            .when('/Machine',
            {
                templateUrl: 'Machine/Machine',
                controller: 'MachineController',
                controllerAs: 'vm',
                caseInsensitiveMatch: true
            })
            .when('/help',
            {
                templateUrl: '/help',
                caseInsensitiveMatch: true
            })
            .otherwise(
            {
                templateUrl: '/Home/Home',
                caseInsensitiveMatch: true
            });
    }]);


    // http interceptor to add hostname [] are for minification safety
    mainApp.factory('httpAPIPathAdder', [function () {
        return {
            request: function (config) {
                if (config.url.search("api:") == 0)
                    config.url = ApiPath + config.url.slice(4);
                return config;
            }
        }
    }]);



    // for CORS
    mainApp.config(['$httpProvider', function ($httpProvider) {
        $httpProvider.defaults.useXDomain = true;
        delete $httpProvider.defaults.headers.common['X-Requested-With'];
        $httpProvider.interceptors.push('httpAPIPathAdder');
    }
    ]);



    //Services
//    mainApp.service('UtilService', ["$http", "$q", "$location", "$window", ManageAppConfig_Var.Service.UtilService]);
    mainApp.service('EnumListService', ["$http", "$q", ManageAppConfig_Var.Service.EnumListService]);

    //Controllers
    mainApp.controller('MachineController', ["$rootScope", "$http", "EnumListService", "UtilService", ManageAppConfig_Var.Controller.MachineController]);
//    mainApp.controller('ApplicationController', ["$rootScope", "$http", "$q", "EnumListService", "UtilService", ManageAppConfig_Var.Controller.ApplicationController]);
})();
console.log('Angular registration complete.');

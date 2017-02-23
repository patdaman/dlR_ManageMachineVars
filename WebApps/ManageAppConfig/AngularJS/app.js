/////-------------------------------------------------------------------------------------------------
//// <copyright file="app.ts" company="Signal Genetics Inc.">
//// Copyright (c) 2015 Signal Genetics Inc.. All rights reserved.
//// </copyright>
//// <author>Ssur</author>
//// <date>20150916</date>
//// <summary>application class</summary>
/////-------------------------------------------------------------------------------------------------
//'use strict';
//declare var BillingSuiteWebApp: any;
//(function () {
//    console.log("Set build type to:" + BillingSuiteWebApp.AppBuildTag);
//    var AppBuildTag: string = BillingSuiteWebApp.AppBuildTag; // this is provided by _PartialPageCommonDependencies.cshtml
//    var ApiPath: string = BillingSuiteWebApp.ApiPath;
//    var TitleTag: string = BillingSuiteWebApp.TitleTag;
//    console.log("Set build type to:" + AppBuildTag + " = " + BillingSuiteWebApp.AppBuildTag);
//    var mainApp = angular.module('BillingSuiteApp', ['kendo.directives', 'ngRoute'])
//        .run(['$rootScope', function ($rootScope) {
//            $rootScope.AppBuildStatus = TitleTag;
//            $rootScope.APIPath = ApiPath;
//        }]);
//    // error handling [] are for minification safety
//    mainApp.factory('$exceptionHandler', [function () {
//        return function (exception, cause) {
//            if (exception) {
//                if (exception.message) {
//                    var showstr = exception.message;
//                    if (cause)
//                        showstr = showstr + "\nCause: " + cause;
//                    console.log(showstr);
//                    alert(showstr);
//                }
//                else if (exception.Message) {
//                    var showstr = exception.Message;
//                    if (cause)
//                        showstr = showstr + "\nCause: " + cause;
//                    console.log(showstr);
//                    alert(showstr);
//                }
//                else {
//                    console.log(exception);
//                    alert(exception);
//                }
//            }
//            else {
//                if (cause) {
//                    console.log(cause);
//                    alert(cause);
//                }
//                else {
//                    console.log("Unknown Exception");
//                    alert("Unknown Exception");
//                }
//                alert("Unknown Exception")
//            }
//        }
//    }]);
//    // routing
//    mainApp.config(['$routeProvider', function ($routeProvider) {
//        $routeProvider
//            .when('/', {
//                templateUrl: '/Home/Home',
//                caseInsensitiveMatch: true
//            })
//            .when('/home',
//            {
//                templateUrl: '/Home/Home',
//                caseInsensitiveMatch: true
//            })
//            .when('/about',
//            {
//                templateUrl: '/Home/About',
//                caseInsensitiveMatch: true
//            })
//            .when('/PayorEditor',
//            {
//                templateUrl: 'PayorEditor/PayorEditor',
//                controller: 'PayorEditorController',
//                controllerAs: 'vm',
//                caseInsensitiveMatch: true
//            })
//            .when('/PhysicianEditor',
//            {
//                templateUrl: 'PhysicianEditor/PhysicianEditor',
//                controller: 'PhysicianEditorController',
//                controllerAs: 'vm',
//                caseInsensitiveMatch: true
//            })
//            .when('/BillReporter',
//            {
//                templateUrl: 'BillReporter/BillReporter',
//                controller: 'BillReporterController',
//                controllerAs: 'vm',
//                caseInsensitiveMatch: true
//            })
//            .when('/DailyStatusReport',
//            {
//                templateUrl: 'DailyStatusReport/DailyStatusReport',
//                controller: 'DailyStatusReportController',
//                controllerAs: 'vm',
//                caseInsensitiveMatch: true
//            })
//            .when('/CaseEditor',
//            {
//                templateUrl: 'CaseEditor/CaseEditor',
//                controller: 'CaseEditorController',
//                controllerAs: 'vm',
//                caseInsensitiveMatch: true
//            })
//            .when('/Reconciliation',
//            {
//                templateUrl: 'Reconciliation/Reconciliation',
//                controller: 'ReconciliationController',
//                controllerAs: 'vm',
//                caseInsensitiveMatch: true
//            })
//            .when('/AccessionTracking',
//            {
//                templateUrl: 'AccessionTracking/AccessionTracking',
//                controller: 'AccessionTrackingController',
//                controllerAs: 'vm',
//                caseInsensitiveMatch: true
//            })
//            .when('/help',
//            {
//                templateUrl: '/help',
//                caseInsensitiveMatch: true
//            })
//            .otherwise(
//            {
//                templateUrl: '/Home/Home',
//                caseInsensitiveMatch: true
//            });
//    }]);
//    // http interceptor to add hostname [] are for minification safety
//    mainApp.factory('httpAPIPathAdder', [function () {
//        return {
//            request: function (config) {
//                if (config.url.search("api:") == 0)
//                    config.url = ApiPath + config.url.slice(4);
//                return config;
//            }
//        }
//    }]);
//    // for CORS
//    mainApp.config(['$httpProvider', function ($httpProvider) {
//        $httpProvider.defaults.useXDomain = true;
//        delete $httpProvider.defaults.headers.common['X-Requested-With'];
//        $httpProvider.interceptors.push('httpAPIPathAdder');
//    }
//    ]);
//    //Services
//    mainApp.service('UtilService', ["$http", "$q", "$location", "$window", BillingSuiteApp.Service.UtilService]);
//    mainApp.service('PayorService', ["$http", "$q", BillingSuiteApp.Service.PayorService]);
//    mainApp.service('PayorGroupService', ["$http", "$q", BillingSuiteApp.Service.PayorGroupService]);
//    mainApp.service('EnumListService', ["$http", "$q", BillingSuiteApp.Service.EnumListService]);
//    //Controllers
//    mainApp.controller('DailyStatusReportController', ["$rootScope", "$http", "PayorService", "UtilService", BillingSuiteApp.Controller.DailyStatusReportController]);
//    mainApp.controller('BillReporterController', ["$rootScope", "$http", "$q", "PayorService", "EnumListService", "UtilService", BillingSuiteApp.Controller.BillReporterController]);
//    mainApp.controller('PhysicianEditorController', ["$rootScope", "$http", BillingSuiteApp.Controller.PhysicianEditorController]);
//    mainApp.controller('CaseEditorController', ["$rootScope", "$http", "$q", "PayorService", "EnumListService", "UtilService", BillingSuiteApp.Controller.CaseEditorController]);
//    mainApp.controller('PayorEditorController', ["$rootScope", "$http", "$q", "PayorGroupService", "EnumListService", BillingSuiteApp.Controller.PayorEditorController]);
//    mainApp.controller('ReconciliationController', ["$rootScope", "$http", "$q", "PayorService", "PayorGroupService", "EnumListService", "UtilService", BillingSuiteApp.Controller.ReconciliationController]);
//    mainApp.controller('AccessionTrackingController', ["$rootScope", "$http", "$q", "EnumListService", BillingSuiteApp.Controller.AccessionTrackingController]);
//})();
//console.log('Angular registration complete.');
//# sourceMappingURL=app.js.map
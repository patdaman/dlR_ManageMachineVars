// register the interceptor as a service
$provide.factory('myHttpInterceptor', function ($q, dependency1, dependency2) {
    return {
        // optional method
        'request': function (config) {
            // do something on success
            return config;
        },

        // optional method
        'requestError': function (rejection) {
            // do something on error
            if (canRecover(rejection)) {
                return responseOrNewPromise
            }
            return $q.reject(rejection);
        },

        // optional method
        'response': function (response) {
            // do something on success
            return response;
        },

        // optional method
        'responseError': function (rejection) {
            // do something on error
            if (canRecover(rejection)) {
                return responseOrNewPromise
            }
            return $q.reject(rejection);
        }
    };
});

$httpProvider.interceptors.push('myHttpInterceptor');


// alternatively, register the interceptor via an anonymous factory
$httpProvider.interceptors.push(function ($q, dependency1, dependency2) {
    return {
        'request': function (config) {
            // same as above
        },

        'response': function (response) {
            // same as above
        }
    };
});



//var app = angular.module('dataSource', function(httpserv: ng.IHttpService, relapipath)
//    pagesize: number,
//    kmodel: typeof kendo.data.Model,
//    transportOptions,
//    onChange?: (e: kendo.data.DataSourceChangeEvent) => void): kendo.data.DataSource {

//        var dst: kendo.data.DataSourceTransport = {};

//        if (transportOptions.read) {
//            dst.read = function (e) {
//                var pars = transportOptions.read();
//                httpserv.get(pars, { withCredentials: true })
//                    .success(function (data, status, headers, config) {
//                        console.log("GridDataManager: Got Data");
//                        e.success(data);
//                    })
//                    .error(function (data, status, headers, config) {
//                        errorHandler("read", data, status, headers, config);
//                    })
//            }
//        }


//        if (transportOptions.update) {
//            dst.update = function (e) {
//                var pars = transportOptions.update();
//                httpserv.put(pars, e.data, { withCredentials: true })
//                    .success(function (data, status, headers, config) {
//                        e.success(data);
//                    })
//                    .error(function (data, status, headers, config) {
//                        errorHandler("update", data, status, headers, config);
//                    })
//            }
//        }

//        if (transportOptions.create) {
//            dst.create = function (e) {
//                var pars = transportOptions.create();
//                httpserv.post(pars, e.data, { withCredentials: true })
//                    .success(function (data, status, headers, config) {
//                        e.success(data);
//                    })
//                    .error(function (data, status, headers, config) {
//                        errorHandler("create", data, status, headers, config);
//                    })
//            }
//        }

//        if (transportOptions.destroy) {
//            dst.destroy = function (e) {
//                var pars = transportOptions.destroy();
//                httpserv.delete(pars, { withCredentials: true })
//                    .success(function (data, status, headers, config) {
//                        e.success(data);
//                    })
//                    .error(function (data, status, headers, config) {
//                        errorHandler("destroy", data, status, headers, config);
//                    })
//            }
//        }


//        function errorHandler(verbName:string, data: any, status: any, headers: any, config: any): void {
//            console.log("DataGrid " + verbName + " error with status: " + status);
//            console.log("Exception details: " + data.Message);
//            console.log("Stacktrace: " + data.StackTraceString);
//            throw { message: "Error doing " + verbName + ". Got Status " + status + ".\n" + data.Message, cause: status };
//        }
//    }
'use strict'

// http interceptor to add hostname [] are for minification safety
appHandler.factory('httpAPIPathAdder', ['$q', '$location', function ($q, $location) {
    return {
        'request': function (config) {
            if (config.url.search("api:") === 0)
                config.url = ApiPath + config.url.slice(4);
            return config;
        },
        'requestError': function (rejection) {
            if (rejection.status === 401) {
                swal({
                    title: "Access Denied",
                    text: rejection.data.Message,
                    type: "error",
                    configButtonText: "OK"
                });
                $location.path('/home');
            }
            else {
                var message;
                if (rejection.data) {
                    if (rejection.data.Message)
                        message = rejection.status + ': ' + rejection.statusText + '\n' + rejection.data.Message;
                }
                swal({
                    title: "Application Error",
                    text: message,
                    type: "error",
                    confirmButtonText: "Cool"
                });
            }
            return $q.reject(rejection);
        },
        'response': function (response) {
            return response;
        },
        'responseError': function (rejection) {
            if (rejection.status === 401) {
                swal({
                    title: "Access Denied",
                    text: rejection.data.Message,
                    type: "error",
                    configButtonText: "OK"
                });
                $location.path('/home');
            }
            else {
                var message;
                if (rejection.data) {
                    if (rejection.data.Message)
                        message = rejection.status + ': ' + rejection.statusText + '\n' + rejection.data.Message;
                }
                else
                    rejection.status + ': ' + rejection.statusText;
                swal({
                    title: "Application Error",
                    text: message,
                    type: "error",
                    confirmButtonText: "Cool"
                });
                return $q.reject(rejection);
            }
        }
    }
}])

// for CORS
.config(['$httpProvider', function ($httpProvider) {
    $httpProvider.defaults.useXDomain = true;
    delete $httpProvider.defaults.headers.common['X-Requested-With'];
    $httpProvider.interceptors.push('httpAPIPathAdder');
}])

// error handling [] are for minification safety
.factory('$exceptionHandler', [function () {
    return function (exception, stack) {
        if (exception) {
            if (exception.message) {
                var showstr = exception.message;
                if (stack)
                    showstr = showstr + "\nCause: " + stack;
                swal({
                    title: "Application Error",
                    text: showstr,
                    type: "error",
                    confirmButtonText: "OK"
                });
                console.log(showstr);
            }
            else {
                console.log(exception);
                alert(exception);
            }
        }
        else {
            if (stack) {
                console.log(cause);
                swal({
                    title: "Application Error",
                    text: stack,
                    type: "error",
                    confirmButtonText: "OK"
                });
            }
            else {
                console.log("Unknown Exception");
                swal({
                    title: "Application Error",
                    text: "Unknown Exception",
                    type: "error",
                    confirmButtonText: "OK"
                });
            }
            alert("Unknown Exception")
        }
    }
}]);
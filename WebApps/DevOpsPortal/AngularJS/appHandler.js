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
                $location.path('/home');
            }
                //else if (canRecover(rejection)) {
                //    return responseOrNewPromise;
                //}
            else {
                swal({
                    title: "Application Error",
                    text: rejection,
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
                $location.path('/home');
            }
                //else if (canRecover(rejection)) {
                //    return responseOrNewPromise;
                //}
            else {
                swal({
                    title: "Application Error",
                    text: rejection,
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
            }
            else {
                console.log("Unknown Exception");
                swal({
                    title: "Application Error",
                    text: "Unknown Exception",
                    type: "error",
                    confirmButtonText: "Cool"
                });
            }
            alert("Unknown Exception")
        }
    }
}]);
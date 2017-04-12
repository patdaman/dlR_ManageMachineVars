
//Service to get data from service..
ConfigApp.service('configcrudservice', function ($http) {

    this.getAllConfigVars = function () {
        return $http.get("/api/ConfigApi");
    }

    //save
    this.save = function (AppVar) {
        var request = $http({
            method: 'post',
            withCredentials: true,
            url: '/api/ConfigApi/',
            data: AppVar
        });
        return request;
    }

    //get single record by Id
    this.get = function (id) {
        //debugger;
        var request = $http({
            method: 'post',
            withCredentials: true,
            url: ('/api/ConfigApi/' + id),
            data: AppVar
        });
        return request;
    }

    //update Config record
    this.update = function (UpdateId, AppVar) {
        //debugger;
        var updaterequest = $http({
            method: 'put',
            withCredentials: true,
            url: "/api/ConfigApi/" + UpdateId,
            data: AppVar
        });
        return updaterequest;
    }

    //delete record
    this.delete = function (UpdateId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            withCredentials: true,
            url: "/api/ConfigApi/" + UpdateId
        });
        return deleterecord;
    }
});

angular.module('ngClickCopy',  [])
    .service('ngCopy', ['$window', function ($window) {
    var body = angular.element($window.document.body);
    var textarea = angular.element('<textarea/>');
    textarea.css({
        position: 'fixed',
        opacity: '0'
    });

    return function (toCopy) {
        textarea.val(toCopy);
        body.append(textarea);
        textarea[0].select();

        try {
            var successful = document.execCommand('copy');
            if (!successful) throw successful;
        } catch (err) {
            window.prompt("Copy to clipboard: Ctrl+C, Enter", toCopy);
        }

        textarea.remove();
    }
}])

.directive('ngClickCopy', ['ngCopy', function (ngCopy) {
    return {
        restrict: 'A',
        link: function (scope, element, attrs) {
            element.bind('click', function (e) {
                ngCopy(attrs.ngClickCopy);
            });
        }
    }
}])

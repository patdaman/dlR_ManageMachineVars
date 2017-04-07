﻿//Service to get data from service..
ConfigApp.service('configcrudservice', function ($http) {

    this.getAllMachines = function () {
        return $http.get("/api/ConfigApi");
    }

    //save
    this.save = function (Machine) {
        var request = $http({
            method: 'post',
            url: '/api/ConfigApi/',
            data: Machine
        });
        return request;
    }

    //get single record by Id
    this.get = function (id) {
        //debugger;
        return $http.get("/api/ConfigApi/" + id);
    }

    //update Machine records
    this.update = function (UpdateId, Machine) {
        //debugger;
        var updaterequest = $http({
            method: 'put',
            url: "/api/ConfigApi/" + UpdateId,
            data: Machine
        });
        return updaterequest;
    }

    //delete record
    this.delete = function (UpdateId) {
        debugger;
        var deleterecord = $http({
            method: 'delete',
            url: "/api/ConfigApi/" + UpdateId
        });
        return deleterecord;
    }
});

ConfigApp.service('ngCopy', ['$window', function ($window) {
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

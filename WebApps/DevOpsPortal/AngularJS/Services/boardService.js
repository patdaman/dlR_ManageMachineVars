boardApp.service('boardService', ['$http', '$q', '$rootScope', 'backendHubProxy',
    function ($http, $q, $rootScope, backendHubProxy) {
        var proxy = null;
        var proxyService = null;

        var getColumns = function () {
            return $http.get("api:/BoardApi").then(function (response) {
                return response.data;
            }, function (error) {
                return $q.reject(error.data.Message);
            });
        };

        var canMoveTask = function (sourceColIdVal, targetColIdVal) {
            return $http.get("api:/BoardApi/CanMove", { params: { sourceColId: sourceColIdVal, targetColId: targetColIdVal } })
                .then(function (response) {
                    return response.data.canMove;
                }, function (error) {
                    return $q.reject(error.data.Message);
                });
        };

        var moveTask = function (taskIdVal, targetColIdVal) {
            return $http.post("api:/BoardApi/MoveTask", { taskId: taskIdVal, targetColId: targetColIdVal })
                .then(function (response) {
                    return response.status == 200;
                }, function (error) {
                    return $q.reject(error.data.Message);
                });
        };

        var initialize = function () {

            this.proxyService = backendHubProxy(backendHubProxy.defaultServer, 'boardHub');

            connection = jQuery.hubConnection($rootScope.signalRPath);
            this.proxy = connection.createHubProxy('boardHub');

            // Listen to the 'BoardUpdated' event that will be pushed from SignalR server
            this.proxyService.on('BoardUpdated', function () {
                $rootScope.$emit("refreshBoardService");
            });

            // Listen to the 'BoardUpdated' event that will be pushed from SignalR server
            this.proxy.on('BoardUpdated', function () {
                $rootScope.$emit("refreshBoard");
            });

            // Connecting to SignalR server        
            //return this.proxyService.connection.start()
            return connection.start()
            .then(function (connectionObj) {
                return connectionObj;
            }, function (error) {
                return error.message;
            });

            //return this.proxyService;
        };

        // Call 'NotifyBoardUpdated' on SignalR server
        var sendRequest = function () {
            this.proxy.invoke('NotifyBoardUpdated');
        };

        return {
            initialize: initialize,
            sendRequest: sendRequest,
            getColumns: getColumns,
            canMoveTask: canMoveTask,
            moveTask: moveTask
        };
    }]);
var app = angular.module('app', ['ngTouch', 'ui.grid', 'ui.grid.expandable', 'ui.grid.selection', 'ui.grid.pinning']);
app.controller('MainCtrl', ['$scope', '$http', '$log', function ($scope, $http, $log) {
        $scope.gridOptions = {
            expandableRowTemplate: 'expandableRowTemplate.html',
            expandableRowHeight: 150,
            //subGridVariable will be available in subGrid scope
            expandableRowScope: {
                subGridVariable: 'subGridScopeVariable'
            }
        };
        $scope.gridOptions.columnDefs = [
            { name: 'id' },
            { name: 'name' },
            { name: 'age' },
            { name: 'address.city' }
        ];
        var i = 0;
        $http.get('/data/500_complex.json')
            .success(function (data) {
            for (i = 0; i < data.length; i++) {
                data[i].subGridOptions = {
                    columnDefs: [{ name: "Id", field: "id" }, { name: "Name", field: "name" }],
                    data: data[i].friends
                };
            }
            $scope.gridOptions.data = data;
        });
        $scope.gridOptions.onRegisterApi = function (gridApi) {
            $scope.gridApi = gridApi;
        };
        $scope.expandAllRows = function () {
            $scope.gridApi.expandable.expandAllRows();
        };
        $scope.collapseAllRows = function () {
            $scope.gridApi.expandable.collapseAllRows();
        };
    }]);
app.controller('SecondCtrl', ['$scope', '$http', '$log', function ($scope, $http, $log) {
        $scope.gridOptions = {
            enableRowSelection: true,
            expandableRowTemplate: 'expandableRowTemplate.html',
            expandableRowHeight: 150
        };
        $scope.gridOptions.columnDefs = [
            { name: 'id', pinnedLeft: true },
            { name: 'name' },
            { name: 'age' },
            { name: 'address.city' }
        ];
        var i = 0;
        $http.get('/data/500_complex.json')
            .success(function (data) {
            for (i = 0; i < data.length; i++) {
                data[i].subGridOptions = {
                    columnDefs: [{ name: "Id", field: "id" }, { name: "Name", field: "name" }],
                    data: data[i].friends
                };
            }
            $scope.gridOptions.data = data;
        });
    }]);
app.controller('ThirdCtrl', ['$scope', '$http', '$log', function ($scope, $http, $log) {
        $scope.gridOptions = {
            expandableRowTemplate: 'expandableRowTemplate.html',
            expandableRowHeight: 150,
            onRegisterApi: function (gridApi) {
                gridApi.expandable.on.rowExpandedStateChanged($scope, function (row) {
                    if (row.isExpanded) {
                        row.entity.subGridOptions = {
                            columnDefs: [
                                { name: 'name' },
                                { name: 'gender' },
                                { name: 'company' }
                            ]
                        };
                        $http.get('/data/100.json')
                            .success(function (data) {
                            row.entity.subGridOptions.data = data;
                        });
                    }
                });
            }
        };
        $scope.gridOptions.columnDefs = [
            { name: 'id', pinnedLeft: true },
            { name: 'name' },
            { name: 'age' },
            { name: 'address.city' }
        ];
        $http.get('/data/500_complex.json')
            .success(function (data) {
            $scope.gridOptions.data = data;
        });
    }]);

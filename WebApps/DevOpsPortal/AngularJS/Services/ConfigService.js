//get object lists
ConfigApp.service('getObjectService', ['$http', function ($http) {
    this.getConfigObjects = function (type) {
        return $http({
            method: 'get',
            url: ('api:/ConfigValuesApi'),
            params: {
                type: type,
            },
        }).then(function (result) {
            return result.data;
        })
    };
}]);



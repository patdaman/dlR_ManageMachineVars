//get object lists
EnumService.service('getObjectService', ['$http', '$rootScope', function ($http, $rootScope) {
    this.getConfigObjects = function (type) {
        return $http({
            method: 'get',
            //url: ('api:/ConfigValuesApi'),
            url: ($rootScope.EnumPath),
            params: {
                type: type,
            },
        }).then(function (result) {
            return result.data;
        })
    };
}]);



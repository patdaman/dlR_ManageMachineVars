'use strict'

ConfigApp.directive('multiSelect', ['$q', function ($q) {
    return {
        restrict: 'E',
        require: 'ngModel',
        scope: {
            selectedLabel: "@",
            availableLabel: "@",
            displayAttr: "@",
            available: "=",
            model: "=ngModel"
        },
        template: '<div class="multiSelect">' +
                    '<div class="form-group">' +
                     '<div class="select">' +
                      '<label class="control-label" for="multiSelectSelected">{{ selectedLabel }} ' +
                          '({{ model.length }})</label>' + '<br />' +
                      '<select class="multiSelectBox" id="currentRoles" ng-model="selected.current" multiple ' +
                          'class="pull-left" ng-options="e as e[displayAttr] for e in model">' +
                      '</select>' +
                      '</div>' +
                    '</div>' +
                    '<div class="form-group">' +
                    '<div class="select buttons">' +
                      '<button class="btn mover down" ng-click="remove()" title="Add selected" ' +
                          'ng-disabled="selected.current.length == 0">' +
                        '<span class="glyphicon glyphicon-menu-down"> Remove </span>' +
                      '</button>' + '&nbsp;' +
                      '<button class="btn mover up" ng-click="add()" title="Remove selected" ' +
                          'ng-disabled="selected.available.length == 0">' +
                        '<span class="glyphicon glyphicon-menu-up"> Add &nbsp;</span>' +
                      '</button>' +
                    '</div>' +
                    '</div>' +
                    '<div class="form-group">' +
                    '<div class="select">' +
                      '<label class="control-label" for="multiSelectAvailable">{{ availableLabel }} ' +
                          '({{ available.length }})</label>' + '<br />' +
                      '<select class="multiSelectBox" id="multiSelectAvailable" ng-model="selected.available" multiple ' +
                          'ng-options="e as e[displayAttr] for e in available"></select>' +
                    '</div>' +
                    '</div>' +
                  '</div>',
        link: function (scope, elm, attrs) {
            scope.selected = {
                available: [],
                current: []
            };

            /* Handles cases where scope data hasn't been initialized yet */
            var dataLoading = function (scopeAttr) {
                var loading = $q.defer();
                if (scope[scopeAttr]) {
                    loading.resolve(scope[scopeAttr]);
                } else {
                    scope.$watch(scopeAttr, function (newValue, oldValue) {
                        if (newValue !== undefined)
                            loading.resolve(newValue);
                    });
                }
                return loading.promise;
            };

            /* Filters out items in original that are also in toFilter. Compares by reference. */
            var filterOut = function (original, toFilter) {
                var filtered = [];
                angular.forEach(original, function (entity) {
                    var match = false;
                    for (var i = 0; i < toFilter.length; i++) {
                        if (toFilter[i][attrs.displayAttr] === entity[attrs.displayAttr]) {
                            match = true;
                            break;
                        }
                    }
                    if (!match) {
                        filtered.push(entity);
                    }
                });
                return filtered;
            };

            scope.refreshAvailable = function () {
                scope.available = filterOut(scope.available, scope.model);
                scope.selected.available = [];
                scope.selected.current = [];
            };

            scope.add = function () {
                scope.model = scope.model.concat(scope.selected.available);
                scope.refreshAvailable();
            };
            scope.remove = function () {
                scope.available = scope.available.concat(scope.selected.current);
                scope.model = filterOut(scope.model, scope.selected.current);
                scope.refreshAvailable();
            };

            $q.all([dataLoading("model"), dataLoading("available")]).then(function (results) {
                scope.refreshAvailable();
            });
        }
    };
}]);
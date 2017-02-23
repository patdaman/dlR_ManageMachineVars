var app = angular.module('app', ['ngTouch', 'ui.grid']);
 
app.controller('MainCtrl', ['$scope', function ($scope) {
 
  $scope.exampleData = [{ "Company": "Samsung", "Model": "Samsung Galaxy Grand 2", "Price": 5000, "Stocks": 4 },  
                                             { "Company": "Samsung", "Model": "Samsung Galaxy S3 Neo", "Price": 9000, "Stocks": 41 },  
                                           { "Company": "Samsung", "Model": "Samsung Galaxy Grand Max", "Price": 11000, "Stocks": 4 },  
                                            { "Company": "MicroMax", "Model": "Micromax Canvas Blaze", "Price": 6300, "Stocks": 0 },  
                                           { "Company": "MicroMax", "Model": "Micromax Canvas Duddle", "Price": 11000, "Stocks": 3 },  
                                             { "Company": "MicroMax", "Model": "Micromax Canvas Duddle- SPL", "Price": 11000, "Stocks": 3 },  
                                             { "Company": "Blackberry", "Model": "Blackberry Z30", "Price": 19000, "Stocks": 10 },  
                                           { "Company": "Blackberry", "Model": "Micromax bold  9780", "Price": 12900, "Stocks": 20 },  
  
                            ];
}]);
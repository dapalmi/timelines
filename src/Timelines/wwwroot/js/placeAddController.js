(function () {

    "use strict";

    angular.module("app-places")
        .controller('placeAddController',
        ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

            $scope.newPlace = {};
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = false;

            $scope.addPlace = function () {
                $scope.isBusy = true;

                $http.post("/api/places", $scope.newPlace)
                .then(function (response) {
                    //Success
                    $scope.newPlace = {};
                    $scope.successMessage = "Place successfully added";
                    $timeout(function () { $scope.successMessage = ""; }, 2000);
                },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to save data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });
            }
                
            }
        ]);

})();
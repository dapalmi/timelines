(function () {

    "use strict";

    angular.module("app-places")
        .controller('placeEditController',
        ['$scope', '$http', '$routeParams', '$timeout', '$location', function ($scope, $http, $routeParams, $timeout, $location) {

            $scope.placeId = $routeParams.placeId;
            $scope.place = {};
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = true;

            $http.get("/api/places/" + $scope.placeId)
                    .then(function (response) {
                        //Success
                        $scope.place = response.data;
                    },
                        function (error) {
                            //Failure
                            $scope.errorMessage = "Failed to load data";
                        })
                    .finally(function () {
                        $scope.isBusy = false;
                    });

            $scope.editPlace = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.put("/api/places/" + $scope.placeId, $scope.place)
                    .then(function (response) {
                        //Success
                        $scope.successMessage = "Place successfully updated";
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

            $scope.deletePlace = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.delete("/api/places/" + $scope.placeId)
                    .then(function (response) {
                        //Success
                        $location.path("/");
                    },
                        function (error) {
                            //Failure
                            $scope.errorMessage = "Failed to delete data";
                        })
                    .finally(function () {
                        $scope.isBusy = false;
                    });
            }

            }
        ]);

})();
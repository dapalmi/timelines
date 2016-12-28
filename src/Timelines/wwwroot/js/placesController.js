(function () {

    "use strict";

    angular.module("app-places")
        .controller('placesController',
        ['$scope', '$http', function ($scope, $http) {

            $scope.places = [];
            $scope.errorMessage = "";
            $scope.isBusy = true;

            $scope.gridOptions = {
                data: 'places',
                columnDefs: [
                    { name: 'name' },
                    { name: 'latitude' },
                    { name: 'longitude' },
                    { field: 'id', displayName: '', enableSorting: false, cellTemplate: '<a ng-href="#/edit/{{COL_FIELD}}" class="btn btn-sm btn-primary">Edit</a>' }
                ]
            };

            $http.get("/api/places")
                .then(function (response) {
                    //Success
                    angular.copy(response.data, $scope.places);
                },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to load data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });
        }
        ]);

})();
(function () {

    "use strict";

    angular.module("app-persons")
        .controller('personsController',
        ['$scope', '$http', function ($scope, $http) {

                $scope.persons = [];
                $scope.errorMessage = "";
                $scope.isBusy = true;

                $scope.gridOptions = {
                    data: 'persons',
                    columnDefs: [
                        { name: 'name' },
                        { name: 'meaning' },
                        { name: 'start' },
                        { name: 'end' },
                        { name: 'unknownStart' },
                        { name: 'unknownEnd' },
                        { name: 'imageUrl' },
                        { field: 'id', displayName: '', enableSorting: false, cellTemplate: '<a ng-href="#/edit/{{COL_FIELD}}" class="btn btn-sm btn-primary">Edit</a>' }
                    ]
                };

                $http.get("/api/persons")
                    .then(function(response) {
                            //Success
                            angular.copy(response.data, $scope.persons);
                        },
                        function(error) {
                            //Failure
                            $scope.errorMessage = "Failed to load data";
                        })
                    .finally(function() {
                        $scope.isBusy = false;
                    });
            }
        ]);

})();
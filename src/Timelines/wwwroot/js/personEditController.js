(function () {

    "use strict";

    angular.module("app-persons")
        .controller('personEditController',
        ['$scope', '$http', '$routeParams', '$timeout', '$location', function ($scope, $http, $routeParams, $timeout, $location) {

            $scope.personId = $routeParams.personId;
            $scope.person = {};
            $scope.events = [];
            $scope.relationships = [];
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = true;

            $scope.eventsGridOptions = {
                data: 'events',
                columnDefs: [
                        { name: 'name' },
                        { name: 'year' },
                        { field: 'id', displayName: '', enableSorting: false, cellTemplate: '<a ng-href="#/eventEdit/{{grid.appScope.personId}}/{{COL_FIELD}}" class="btn btn-sm btn-primary">Edit</a>' }
                ]
            };

            $scope.relationshipsGridOptions = {
                data: 'relationships',
                columnDefs: [
                        { name: 'relatedPerson.name' },
                        { name: 'relationshipType' },
                        { field: 'relatedPerson.id', displayName: '', enableSorting: false, cellTemplate: '<a ng-href="#/relationshipEdit/{{grid.appScope.personId}}/{{COL_FIELD}}" class="btn btn-sm btn-primary">Edit</a>' }
                ]
            };

            $http.get("/api/persons/" + $scope.personId)
                    .then(function (response) {
                        //Success
                        $scope.person = response.data;
                    },
                        function (error) {
                            //Failure
                            $scope.errorMessage = "Failed to load data";
                        })
                    .finally(function () {
                        $scope.isBusy = false;
                    });

            $http.get("/api/persons/" + $scope.personId + "/events")
                    .then(function (response) {
                        //Success
                        angular.copy(response.data, $scope.events);
                    },
                        function (error) {
                            //Failure
                            $scope.errorMessage = "Failed to load data";
                        })
                    .finally(function () {
                        $scope.isBusy = false;
                    });

            $http.get("/api/persons/" + $scope.personId + "/relationships")
                .then(function (response) {
                    //Success
                    angular.copy(response.data, $scope.relationships);
                },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to load data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });

            $scope.editPerson = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.put("/api/persons/" + $scope.personId, $scope.person)
                    .then(function (response) {
                        //Success
                        $scope.successMessage = "Person successfully updated";
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

            $scope.deletePerson = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.delete("/api/persons/" + $scope.personId)
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
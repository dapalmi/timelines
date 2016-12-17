(function () {

    "use strict";

    angular.module("app-persons")
        .controller('relationshipEditController',
        ['$scope', '$http', '$routeParams', '$timeout', '$location', function ($scope, $http, $routeParams, $timeout, $location) {

            $scope.personId = $routeParams.personId;
            $scope.relatedPersonId = $routeParams.relatedPersonId;
            $scope.person = {};
            $scope.relationship = {};
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = true;

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

            $http.get("/api/persons/" + $scope.personId + "/relationships/" + $scope.relatedPersonId)
                .then(function (response) {
                    //Success
                    $scope.relationship = response.data;
                },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to load data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });

            $http.get("/api/relationships/types")
                .then(function (response) {
                    //Success
                    $scope.relationshipTypes = response.data;
                },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to load data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });

            $scope.editRelationship = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.put("/api/persons/" + $scope.personId + "/relationships/" + $scope.relatedPersonId, $scope.relationship)
                    .then(function (response) {
                        //Success
                        $location.path("/edit/" + $scope.personId);
                    },
                        function (error) {
                            //Failure
                            $scope.errorMessage = "Failed to save data";
                        })
                    .finally(function () {
                        $scope.isBusy = false;
                    });
            }

            $scope.deleteRelationship = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.delete("/api/persons/" + $scope.personId + "/relationships/" + $scope.relatedPersonId)
                    .then(function (response) {
                        //Success
                        $location.path("/edit/" + $scope.personId);
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
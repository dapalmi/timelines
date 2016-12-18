(function () {

    "use strict";

    angular.module("app-persons")
        .controller('relationshipAddController',
        ['$scope', '$http', '$timeout', '$routeParams', function ($scope, $http, $timeout, $routeParams) {

            $scope.personId = $routeParams.personId;
            $scope.person = {};
            $scope.newRelationship = {};
            $scope.relationshipPersons = [];
            $scope.relationshipTypes = [];
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = false;

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

            $http.get("/api/persons/" + $scope.personId + "/relationships/persons")
                .then(function (response) {
                    //Success
                    $scope.relationshipPersons = response.data;
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

            $scope.addRelationship = function () {
                $scope.isBusy = true;

                $http.post("/api/persons/" + $scope.personId + "/relationships", $scope.newRelationship)
                .then(function (response) {
                    //Success
                    $scope.newRelationship = {};
                    $scope.successMessage = "Relationship successfully added";
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
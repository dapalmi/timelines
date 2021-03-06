﻿(function () {

    "use strict";

    angular.module("app-persons")
        .controller('personAddController',
        ['$scope', '$http', '$timeout', function ($scope, $http, $timeout) {

            $scope.newPerson = {};
            $scope.genderTypes = [];
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = false;

            $http.get("/api/persons/gendertypes")
                .then(function (response) {
                    //Success
                    $scope.genderTypes = response.data;
                },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to load data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });

            $scope.addPerson = function () {
                $scope.isBusy = true;

                $http.post("/api/persons", $scope.newPerson)
                .then(function (response) {
                    //Success
                    $scope.newPerson = {};
                    $scope.successMessage = "Person successfully added";
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
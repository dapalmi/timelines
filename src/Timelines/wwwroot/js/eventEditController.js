(function () {

    "use strict";

    angular.module("app-persons")
        .controller('eventEditController',
        ['$scope', '$http', '$routeParams', '$timeout', '$location', function ($scope, $http, $routeParams, $timeout, $location) {

            $scope.personId = $routeParams.personId;
            $scope.eventId = $routeParams.eventId;
            $scope.person = {};
            $scope.event = {};
            $scope.places = [];
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = true;

            $scope.convertedMarkdownText = "";
            $scope.convertMarkdown = function () {
                var converter = new showdown.Converter();
                var htmlText = converter.makeHtml($scope.event.text);
                $scope.convertedMarkdownText = htmlText;
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

            $http.get("/api/events/" + $scope.eventId)
                .then(function (response) {
                    //Success
                    $scope.event = response.data;
                    },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to load data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });

            $http.get("/api/places/")
                .then(function (response) {
                    //Success
                    $scope.places = response.data;
                },
                    function (error) {
                        //Failure
                        $scope.errorMessage = "Failed to load data";
                    })
                .finally(function () {
                    $scope.isBusy = false;
                });

            $scope.editEvent = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.put("/api/events/" + $scope.eventId, $scope.event)
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

            $scope.deleteEvent = function () {
                $scope.isBusy = true;
                $scope.errorMessage = "";

                $http.delete("/api/events/" + $scope.eventId)
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
(function () {

    "use strict";

    angular.module("app-persons")
        .controller('eventAddController',
        ['$scope', '$http', '$timeout', '$routeParams', function ($scope, $http, $timeout, $routeParams) {

            $scope.personId = $routeParams.personId;
            $scope.newEvent = {};
            $scope.successMessage = "";
            $scope.errorMessage = "";
            $scope.isBusy = false;

            $scope.convertedMarkdownText = "";
            $scope.convertMarkdown = function () {
                var converter = new showdown.Converter();
                var htmlText = converter.makeHtml($scope.newEvent.text);
                $scope.convertedMarkdownText = htmlText;
            };

            $scope.addEvent = function () {
                $scope.isBusy = true;

                $http.post("/api/persons/" + $scope.personId + "/events", $scope.newEvent)
                .then(function (response) {
                    //Success
                    $scope.newEvent = {};
                    $scope.successMessage = "Event successfully added";
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
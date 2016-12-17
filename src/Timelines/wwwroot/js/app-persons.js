(function () {

    "use strict";

    angular.module("app-persons", ["ngSanitize", "ngRoute", "ui.grid", "simpleControls"]).config(function ($routeProvider) {

        $routeProvider.when("/",
        {
            controller: "personsController",
            templateUrl: "/views/persons/gridView.html"
        });

        $routeProvider.when("/add",
        {
            controller: "personAddController",
            templateUrl: "/views/persons/addView.html"
        });

        $routeProvider.when("/edit/:personId",
        {
            controller: "personEditController",
            templateUrl: "/views/persons/editView.html"
        });

        $routeProvider.when("/eventAdd/:personId",
        {
            controller: "eventAddController",
            templateUrl: "/views/events/addView.html"
        });

        $routeProvider.when("/eventEdit/:personId/:eventId",
        {
            controller: "eventEditController",
            templateUrl: "/views/events/editView.html"
        });

        $routeProvider.when("/relationshipAdd/:personId",
        {
            controller: "relationshipAddController",
            templateUrl: "/views/relationships/addView.html"
        });

        $routeProvider.when("/relationshipEdit/:personId/:relatedPersonId",
         {
             controller: "relationshipEditController",
             templateUrl: "/views/relationships/editView.html"
         });

        $routeProvider.otherwise({ redirectTo: "/" });
    });

})();
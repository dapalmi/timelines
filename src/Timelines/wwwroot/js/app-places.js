(function () {

    "use strict";

    angular.module("app-places", ["ngSanitize", "ngRoute", "ui.grid"]).config(function ($routeProvider) {

        $routeProvider.when("/",
        {
            controller: "placesController",
            templateUrl: "/views/places/gridView.html"
        });

        $routeProvider.when("/add",
        {
            controller: "placeAddController",
            templateUrl: "/views/places/addView.html"
        });

        $routeProvider.when("/edit/:placeId",
        {
            controller: "placeEditController",
            templateUrl: "/views/places/editView.html"
        });

        $routeProvider.otherwise({ redirectTo: "/" });
    });

})();
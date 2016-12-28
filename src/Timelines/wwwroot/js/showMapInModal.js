(function () {
    "use strict";

    angular.module("showMapInModal", [])
        .directive("modal", modal);

    function modal() {
        return {
            restrict: 'C',
            link: function postLink(scope, elem, attrs) {
                elem.on('shown.bs.modal',
                        function() {
                            var id = $(this).attr('id');
                            NgMap.getMap("map-" + id)
                                .then(function(map) {
                                    google.maps.event.trigger(map, 'resize');
                                });
                        });
            }
        };
    }

})();
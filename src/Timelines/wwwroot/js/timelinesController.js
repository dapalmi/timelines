(function () {

    "use strict";

    angular.module("app-timelines")
        .controller('timelinesController',
        ['$scope', '$http', function ($scope, $http) {

            $scope.isBusy = true;

            $scope.timelines = [];

                $scope.config = {
                    "intervalWidth": 360,
                    "yearsPerInterval": 500,
                    "startYear": -4030,
                    "endYear": 2020
                };
                $scope.currentYear = $scope.config.startYear;
                $scope.intervalYears = [1, 5, 10, 50, 100, 500, 1000];
                $scope.years = [];
                for (var i = 0; i < ($scope.config.endYear - $scope.currentYear + 1) ; i++) {
                    var value = $scope.currentYear + i;
                    var name = $scope.currentYear + i;
                    if (value !== 0) {
                        if (value < 0) {
                            name = name + " B.C.E";
                        }
                        else {
                            name = name + " C.E";
                        }
                        var year = {
                            "value": value,
                            "name": name
                        };
                        $scope.years.push(year);
                    }
                };
                $scope.selectedYear = $scope.years[0];

                $scope.sizes = getSizes();
                $scope.pixelsPerYear = $scope.config.intervalWidth / $scope.config.yearsPerInterval;
                $scope.changeConfig = function () {
                    $scope.currentYear = $scope.selectedYear.value;
                    $scope.sizes = getSizes();
                    $scope.pixelsPerYear = $scope.config.intervalWidth / $scope.config.yearsPerInterval;
                }

                $http.get("/api/timelines")
                    .then(function (response) {
                        //Success
                        angular.copy(response.data, $scope.timelines);
                        $scope.changeConfig();
                    },
                        function (error) {
                            //Failure
                            $scope.errorMessage = "Failed to load data";
                        })
                    .finally(function () {
                        $scope.isBusy = false;
                    });


                $scope.parents = [];
                $scope.children = [];
                $scope.spouse = [];
                $scope.siblings = [];
                $scope.setHighlights = function (id) {
                    $scope.clearHighlights();
                    var person = getPersonById(id);
                    $scope.parents = [];
                    $scope.children = [];
                    $scope.spouse = [];
                    $scope.siblings = [];
                    if (person[0].parents && person[0].parents.length) {
                        $scope.parents.push.apply($scope.parents, person[0].parents);
                    }
                    if (person[0].children && person[0].children.length) {
                        $scope.children.push.apply($scope.children, person[0].children);
                    }
                    if (person[0].spouse && person[0].spouse.length) {
                        $scope.spouse.push.apply($scope.spouse, person[0].spouse);
                    }
                    if (person[0].siblings && person[0].siblings.length) {
                        $scope.siblings.push.apply($scope.siblings, person[0].siblings);
                    }
                }
                $scope.clearHighlights = function () {
                    $scope.parents = [];
                    $scope.children = [];
                    $scope.spouse = [];
                }
                $scope.isParent = function (id) {
                    return $scope.parents.indexOf(id) !== -1;
                }
                $scope.isChild = function (id) {
                    return $scope.children.indexOf(id) !== -1;
                }
                $scope.isSpouse = function (id) {
                    return $scope.spouse.indexOf(id) !== -1;
                }
                $scope.isSibling = function (id) {
                    return $scope.siblings.indexOf(id) !== -1;
                }

                $scope.isMale = function(genderType) {
                    return genderType === "Male";
                }
                $scope.isFemale = function (genderType) {
                    return genderType === "Female";
                }
                $scope.isSpirit = function (genderType) {
                    return genderType === "Spirit";
                }

                $scope.fastBackward = function (size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var newYear = $scope.config.startYear;
                    var selectedYear = $scope.years.filter(
                        function (year) { return year.value === newYear }
                    );
                    $scope.selectedYear = selectedYear[0];
                    $scope.changeConfig();
                }
                $scope.backward = function (size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var newYear = $scope.currentYear - ($scope.config.yearsPerInterval * numberOfIntervals);
                    var selectedYear = $scope.years.filter(
                        function (year) { return year.value === newYear }
                    );
                    $scope.selectedYear = selectedYear[0];
                    $scope.changeConfig();
                }
                $scope.forward = function (size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var newYear = $scope.currentYear + ($scope.config.yearsPerInterval * numberOfIntervals);
                    var selectedYear = $scope.years.filter(
                        function (year) { return year.value === newYear }
                    );
                    $scope.selectedYear = selectedYear[0];
                    $scope.changeConfig();
                }
                $scope.fastForward = function (size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var newYear = $scope.config.endYear - ($scope.config.yearsPerInterval * (numberOfIntervals - 1));
                    var selectedYear = $scope.years.filter(
                        function (year) { return year.value === newYear }
                    );
                    $scope.selectedYear = selectedYear[0];
                    $scope.changeConfig();
                }

                $scope.getTimelineWidth = function (size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var width = $scope.config.intervalWidth * numberOfIntervals + 'px';
                    return { "width": width };
                }
                $scope.getTimelineClass = function (size) {
                    return "visible-" + size + "-block";
                }
                $scope.getTimelineTitle = function (timeline) {
                    var title = timeline.name + " [" + timeline.meaning + "] \n";
                    title += $scope.getTimelineRangeString(timeline);
                    return title;
                }
                $scope.getTimelineRangeString = function (timeline) {
                    var rangeString = "";
                    var range = getTimelineRange(timeline);
                    if (range.start < 0) {
                        rangeString += range.start * -1 + " B.C.E";
                    }
                    else if (range.start === 0) {
                        rangeString += "?";
                    }
                    else {
                        rangeString += range.start + " C.E";
                    }
                    rangeString += " - ";
                    if (range.end < 0) {
                        rangeString += range.end * -1 + " B.C.E";
                    }
                    else if (range.end === 0) {
                        rangeString += "?";
                    }
                    else {
                        rangeString += range.end + " C.E";
                    }
                    return rangeString;
                }
                $scope.getEventYearString = function (year) {
                    var yearString = "";
                    if (year === null) {
                        yearString = "?";
                    }
                    else if (year < 0) {
                        yearString += year * -1 + " B.C.E";
                    }
                    else {
                        yearString += year + " C.E";
                    }
                    return yearString;
                }
                $scope.getEventTitle = function (event) {
                    var title = event.name + " \n";
                    title += $scope.getEventYearString(event.year);
                    return title;
                }
                $scope.getIntervalWidth = function () {
                    var width = $scope.config.intervalWidth + 'px';
                    return { "width": width };
                }
                $scope.selectTimeline = function (timeline) {
                    timeline.isSelected = !timeline.isSelected;
                    $scope.setHighlights(timeline.id);
                }

                $scope.getHtmlEventText = function (text) {
                    var converter = new showdown.Converter();
                    var htmlText = converter.makeHtml(text);
                    return htmlText;
                }

                $scope.getImageSrc = function (imageUrl) {
                    if (imageUrl) {
                        return imageUrl;
                    }
                    return "./images/placeholder.png";
                }

                $scope.$watch("sizes", function () {
                    $scope.getCurrentEvents = function (events, size) {
                        var currentEvents = filterCurrentEvents(events, size);
                        return currentEvents;
                    }
                    $scope.getTimelineDetailsVoidWidth = function (timeline, size) {
                        var width = calculateTimelineVoidWidth(timeline, size) - 10 + 'px';
                        return { "width": width };
                    }
                    $scope.getEventVoidWidth = function (event, timeline, size) {
                        var width = calculateEventVoidWidth(event, timeline, size) + 'px';
                        return { "width": width };
                    }
                    $scope.getTimelineVoidWidth = function (timeline, size) {
                        var width = calculateTimelineVoidWidth(timeline, size) + 'px';
                        return { "width": width };
                    }
                    $scope.getCompleteWidth = function (timeline, size) {
                        //var width = calculateCompleteWidth(timeline, size) + 'px';
                        var unknownStartWidth = calculateUnknownStartWidth(timeline, size);
                        var mainWidth = calculateMainWidth(timeline, size);
                        var unknownEndWidth = calculateUnknownEndWidth(timeline, size);
                        var width = unknownStartWidth + mainWidth + unknownEndWidth;
                        return { "width": width };
                    }
                    $scope.getUnknownStartWidth = function (timeline, size) {
                        var width = calculateUnknownStartWidth(timeline, size) + 'px';
                        return { "width": width };
                    }
                    $scope.getMainWidth = function (timeline, size) {
                        var width = calculateMainWidth(timeline, size) + 'px';
                        return { "width": width };
                    }
                    $scope.getUnknownEndWidth = function (timeline, size) {
                        var width = calculateUnknownEndWidth(timeline, size) + 'px';
                        return { "width": width };
                    }
                    $scope.getUnknownStartTimelineName = function (timeline, size) {
                        var unknownStartWidth = calculateUnknownStartWidth(timeline, size);
                        var mainWidth = calculateMainWidth(timeline, size);
                        var unknownEndWidth = calculateUnknownEndWidth(timeline, size);
                        if (mainWidth === 0 && unknownStartWidth > 0 && unknownStartWidth > unknownEndWidth) {
                            return timeline.name;
                        }
                        return "";
                    }
                    $scope.getMainTimelineName = function (timeline, size) {
                        var mainWidth = calculateMainWidth(timeline, size);
                        if (mainWidth > 0) {
                            return timeline.name;
                        }
                        return "";
                    }
                    $scope.getUnknownEndTimelineName = function (timeline, size) {
                        var unknownStartWidth = calculateUnknownStartWidth(timeline, size);
                        var mainWidth = calculateMainWidth(timeline, size);
                        var unknownEndWidth = calculateUnknownEndWidth(timeline, size);
                        if (mainWidth === 0 && unknownEndWidth > 0 && unknownEndWidth > unknownStartWidth) {
                            return timeline.name;
                        }
                        return "";
                    }
                });

                function getSizes() {
                    return [
                            {
                                "name": "xs",
                                "intervals": getIntervals("xs"),
                                "timelines": getTimelines("xs")
                            },
                            {
                                "name": "sm",
                                "intervals": getIntervals("sm"),
                                "timelines": getTimelines("sm")
                            },
                            {
                                "name": "md",
                                "intervals": getIntervals("md"),
                                "timelines": getTimelines("md")
                            },
                            {
                                "name": "lg",
                                "intervals": getIntervals("lg"),
                                "timelines": getTimelines("lg")
                            },
                    ];
                }

                function getNumberOfIntervals(size) {
                    var numberOfIntervals = 3;
                    switch (size) {
                        case "xs":
                            numberOfIntervals = 1;
                            break;
                        case "sm":
                            numberOfIntervals = 2;
                            break;
                        case "md":
                            numberOfIntervals = 2;
                            break;
                        case "lg":
                            numberOfIntervals = 3;
                            break;
                    }
                    return numberOfIntervals;
                }

                function getIntervals(size) {
                    var intervals = [];
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var currentYear = $scope.currentYear;
                    for (var i = 0; i < numberOfIntervals; i++) {
                        if (currentYear < 0) {
                            intervals[i] = (currentYear * -1) + " B.C.E";
                        }
                        else if (currentYear === 0) {
                            intervals[i] = 1 + " C.E";
                        }
                        else {
                            intervals[i] = currentYear + " C.E";
                        }
                        currentYear += $scope.config.yearsPerInterval;
                    }
                    return intervals;
                }

                function getTimelines(size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var intervalStart = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var intervalEnd = intervalStart + (numberOfIntervals * $scope.config.yearsPerInterval);
                    return $scope.timelines.filter(
                        function (data) {
                            var start = data.start;
                            var end = data.end;
                            if (data.unknownStart !== null && data.unknownStart < data.start) {
                                start = data.unknownStart;
                            }
                            if (data.unknownEnd !== null && data.unknownEnd > data.end) {
                                end = data.unknownEnd;
                            }
                            return (start >= intervalStart && start < intervalEnd) || (end > intervalStart && end <= intervalEnd) || (start < intervalStart && end > intervalEnd)
                        }
                    );
                }

                function getTimelineRange(timeline) {
                    var range = { "start": 0, "end": 0 }
                    range.start = timeline.start;
                    range.end = timeline.end;
                    if (timeline.unknownStart !== null && timeline.unknownStart <= timeline.start) {
                        //range.start = timeline.unknownStart;
                        range.start = 0;
                    }
                    if (timeline.unknownEnd !== null && timeline.unknownEnd > timeline.end) {
                        //range.end = timeline.unknownEnd;
                        range.end = 0;
                    }
                    return range;
                }

                function getPersonById(id) {
                    return $scope.timelines.filter(
                        function (data) { return data.id === id }
                    );
                }

                function filterCurrentEvents(events, size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var endYear = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                    return events.filter(
                        function (event) { return (event.year >= startYear && event.year <= endYear) || event.year === null }
                    );
                }

                function calculateEventVoidWidth(event, timeline, size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var width = 0;
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var endYear = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                    var timelineStartYear = getTimelineStartYear(timeline);
                    var start = event.previousEventYear;
                    var end = event.year;
                    if (start === null) {
                        start = timelineStartYear;
                    }
                    if (start < startYear) {
                        start = startYear;
                    }
                    if (end === null) {
                        end = timelineStartYear;
                    }
                    if (end > endYear) {
                        end = endYear;
                    }
                    width = ((end - start) * $scope.pixelsPerYear);

                    //Adjust width so dot (width: 20px) is centered
                    if (event.previousEventYear !== null || end === start) {
                        width -= 10;
                    }
                    if (event.previousEventYear !== null && event.previousEventYear >= startYear) {
                        width -= 10;
                    }
                    if (event.previousEventYear !== null && event.previousEventYear === startYear) {
                        width -= 10;
                    }
                    if (event.year === endYear) {
                        width -= 10;
                    }
                    return width;
                }
                function getTimelineStartYear(timeline) {
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var start = timeline.start;
                    if (timeline.unknownStart !== null && timeline.unknownStart < timeline.start) {
                        start = timeline.unknownStart;
                    }
                    if (start < startYear) {
                        start = startYear;
                    }
                    return start;
                }
                function calculateTimelineVoidWidth(timeline, size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var width = 0;
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var endYear = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                    var start = timeline.start;
                    if (timeline.unknownStart !== null && timeline.unknownStart < timeline.start) {
                        start = timeline.unknownStart;
                    }
                    if (start > endYear) {
                        start = endYear;
                    }
                    if (start >= startYear) {
                        width = (start - startYear) * $scope.pixelsPerYear;
                    }
                    return width;
                }
                function calculateCompleteWidth(timeline, size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var width = 0;
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var endYear = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                    var start = timeline.start;
                    var end = timeline.end;
                    if (timeline.unknownStart !== null && timeline.unknownStart < timeline.start) {
                        start = timeline.unknownStart;
                    }
                    if (start < startYear) {
                        start = startYear;
                    }
                    if (timeline.unknownEnd !== null && timeline.unknownEnd > timeline.end) {
                        end = timeline.unknownEnd;
                    }
                    if (end > endYear) {
                        end = endYear;
                    }
                    if (start >= startYear && start < endYear && start < end) {
                        width = (end - start) * $scope.pixelsPerYear;
                    }
                    return width;
                }
                function calculateUnknownStartWidth(timeline, size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var width = 0;
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var endYear = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                    if (timeline.unknownStart !== null && timeline.unknownStart < timeline.start) {
                        var start = timeline.unknownStart;
                        var end = timeline.start;
                        if (start < startYear) {
                            start = startYear;
                        }
                        if (end > startYear + numberOfIntervals * $scope.config.yearsPerInterval) {
                            end = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                        }
                        if (start >= startYear && start < endYear && start < end) {
                            width = (end - start) * $scope.pixelsPerYear;
                        }
                    }
                    return width;
                }
                function calculateMainWidth(timeline, size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var width = 0;
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var endYear = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                    var start = timeline.start;
                    var end = timeline.end;
                    if (start < startYear) {
                        start = startYear;
                    }
                    if (end > endYear) {
                        end = endYear;
                    }
                    if (start >= startYear && start < endYear && start < end) {
                        width = (end - start) * $scope.pixelsPerYear;
                    }
                    return width;
                }
                function calculateUnknownEndWidth(timeline, size) {
                    var numberOfIntervals = getNumberOfIntervals(size);
                    var width = 0;
                    var startYear = $scope.currentYear - ($scope.config.yearsPerInterval / 2);
                    var endYear = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                    if (timeline.unknownEnd !== null && timeline.unknownEnd > timeline.end) {
                        var start = timeline.end;
                        var end = timeline.unknownEnd;
                        if (start < startYear) {
                            start = startYear;
                        }
                        if (end > startYear + numberOfIntervals * $scope.config.yearsPerInterval) {
                            end = startYear + numberOfIntervals * $scope.config.yearsPerInterval;
                        }
                        if (start >= startYear && start < endYear && start < end) {
                            width = (end - start) * $scope.pixelsPerYear;
                        }
                    }
                    return width;
                }
            }
        ]);

})();
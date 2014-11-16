'use strict';

/* Filters */

angular.module('taskFilters', []).filter('priorityIcon', function () {
    return function (input) {
        if (input == TaskPriority.Low)
        {
            return "glyphicon glyphicon-circle-arrow-down";
        }
        else if (input == TaskPriority.High)
        {
            return "glyphicon glyphicon-circle-arrow-up";
        }

        return "";
    };
});
﻿'use strict';

/* App Module */

var taskApp = angular.module('taskApp', [
  'ngRoute',
  'ui.bootstrap',
  'taskControllers',
  'taskServices',
  'taskFilters'
]);

taskApp.config(['$routeProvider',
    function($routeProvider) {
        $routeProvider.
            when('/list', {
                templateUrl: 'app/partials/list.html',
                controller: 'TaskCtrl'
            }).
            when('/create', {
                templateUrl: 'app/partials/create.html',
                controller: 'TaskCreateCtrl'
            }).
            when('/edit/:taskId', {
                templateUrl: 'app/partials/edit.html',
                controller: 'TaskEditCtrl'
            }).
            otherwise({
                redirectTo: '/list'
            });
    }]);

var TaskPriority = {
    Normal: 0,
    High: 1,
    Low: 2
};
'use strict';

/* Controllers */

var taskControllers = angular.module('taskControllers', []);

taskControllers.controller('TaskCtrl', ['$scope', 'Task',
    function ($scope, Task) {
        $scope.alerts = [];
        $scope.tasks = Task.query();
        
        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

        $scope.completeTask = function (taskId) {
            Task.complete({ taskId: taskId }, function () {
                $scope.alerts.push({ type: 'success', msg: 'Task completed' });
                $scope.tasks = Task.query();
            }, function (error) {
                $scope.alerts.push({ type: 'danger', msg: error.data.message });
            });
        };
    }]);

taskControllers.controller('TaskCreateCtrl', ['$scope', 'Task',
    function ($scope, Task) {
        $scope.alerts = [];
        $scope.task = new Task();

        $scope.openDatePopup = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

        $scope.createTask = function () {
            Task.save($scope.task, function () {
                $scope.alerts.push({ type: 'success', msg: 'Task created' });
                $scope.task = new Task();
            }, function (error) {
                $scope.alerts.push({ type: 'danger', msg: error.data.message });
            });
        };
    }]);

taskControllers.controller('TaskEditCtrl', ['$scope', '$routeParams', 'Task',
    function ($scope, $routeParams, Task) {
        $scope.alerts = [];

        $scope.openDatePopup = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();
            $scope.opened = true;
        };

        $scope.closeAlert = function (index) {
            $scope.alerts.splice(index, 1);
        };

        $scope.getTask = function () {
            $scope.task = Task.get({ id: $routeParams.taskId });
        };

        $scope.editTask = function () {
            Task.update($scope.task, function () {
                $scope.alerts.push({ type: 'success', msg: 'Task updated' });
            }, function (error) {
                $scope.alerts.push({ type: 'danger', msg: error.data.message });
            });
        };

        $scope.getTask();
    }]);
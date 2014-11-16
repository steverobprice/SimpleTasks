'use strict';

/* Services */

var taskServices = angular.module('taskServices', ['ngResource']);

taskServices.factory('Task', ['$resource',
  function($resource){
      return $resource('api/task/:taskId', { taskId: '@taskId' }, {
          update: { method: 'PUT' },
          complete: { method: 'POST', url: 'api/task/:taskId/complete' }
    });
  }]);
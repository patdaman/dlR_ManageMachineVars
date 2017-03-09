'use strict';

var app = angular.module('angularServiceDashboard', ['ng.epoch','n3-pie-chart']);

app.value('backendServerUrl', 'http://localhost:41999/signalr/performance');
/*globals define, Backbone */
define([
    'underscore',
    'backbone'
],
    function (_, Backbone) {
        var eventBus = {};
        _.extend(eventBus, Backbone.Events);
        return eventBus;
    });
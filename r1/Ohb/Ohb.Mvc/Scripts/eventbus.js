/*globals define, Backbone */
define([
    'underscore',
    'backbone'
],
    function (_, Backbone) {
        var eventBus = {};
        _.extend(eventBus, Backbone.Events);
        eventBus.bind("all", function (eventName) {
            console.log(eventName);
        });
        return eventBus;
    });
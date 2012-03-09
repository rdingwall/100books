"use strict";

var Ohb = window;

Ohb.EventBus = (function ($, _, Backbone) {

    var log = $.jog("EventBus"),
        instance = null,
        EventBus = function () {

            if (instance !== null) {
                throw new Error("Cannot instantiate more than one EventBus, use EventBus.getInstance()");
            }

            this.initialize();
        };

    EventBus.prototype = {

        initialize: function () {
            log.info("EventBus ctor");

            _.extend(this, Backbone.Events);
            this.reset();
        },

        reset: function () {
            log.info("eventBus.reset");
            this.unbind();
            this.bind("all", function (eventName) {
                log.info("Received event: " + eventName);
            });
        }
    };

    EventBus.getInstance = function () {
        // summary:
        //      Gets an instance of the singleton. It is better to use
        if (instance === null) {
            instance = new EventBus();
        }
        return instance;
    };

    log.info("returning eventBus...");
    return EventBus.getInstance();

})($, _, Backbone);
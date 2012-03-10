"use strict";

var Ohb = window;

Ohb.eventBus = (function ($, _, Backbone) {

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

    log.info("returning eventBus...");
    return new EventBus();

})($, _, Backbone);
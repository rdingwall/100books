Ohb.eventBus = (function ($, _, Backbone) {
    "use strict";

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
            log.info("Initializing EventBus...");

            _.extend(this, Backbone.Events);
            this.reset();
        },

        reset: function () {
            log.info("EventBus reset");
            this.unbind();
            this.on("all", function (eventName) {
                log.info("Received event: " + eventName);
            });
        }
    };

    return new EventBus();

}($, _, Backbone));
var Ohb = window;

Ohb.Router = (function ($, Backbone, eventBus) {
    "use strict";

    return Backbone.Router.extend({
        routes: {
            "books/:id": "openBook"
        },

        openBook: function (id) {
            eventBus.trigger("book:requested", id);
        }
    });

}($, Backbone, Ohb.eventBus));

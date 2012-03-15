Ohb.Router = (function ($, Backbone, eventBus) {
    "use strict";

    return Backbone.Router.extend({
        routes: {
            "" : "openMyProfile",
            "books/:id": "openBook",
            "books/:id/:slug": "openBook",
            "*path": "openMyProfile" // default catch-all route
        },

        openBook: function (id) {
            eventBus.trigger("book:requested", id);
        },

        openMyProfile: function () {
            eventBus.trigger("myprofile:requested");
        }
    });

}($, Backbone, Ohb.eventBus));

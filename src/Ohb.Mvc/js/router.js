Ohb.router = (function ($, Backbone, eventBus) {
    "use strict";

    var Router = Backbone.Router.extend({
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

    return new Router();

}($, Backbone, Ohb.eventBus));

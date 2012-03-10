var Ohb = window;

Ohb.Router = (function ($, Backbone) {
    "use strict";

    return Backbone.Router.extend({
        routes: {
            "book/:id/:dummy": "openBook"
        }
    });

}($, Backbone));

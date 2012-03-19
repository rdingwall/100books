Ohb.Collections.PreviousReadCollection = (function ($, _, Backbone, PreviousRead) {
    "use strict";

    return Backbone.Collection.extend({
        model: PreviousRead,
        url: "/api/v1/previousreads"
    });

}($, _, Backbone, Ohb.Models.PreviousRead));
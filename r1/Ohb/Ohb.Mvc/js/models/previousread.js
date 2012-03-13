var Ohb = window;

Ohb.PreviousRead = (function (Backbone) {
    "use strict";

    return Backbone.Model.extend({

        urlRoot: "/api/v1/previousreads",

        defaults: {
            googleVolumeId: null,
            title: "",
            authors: "",
            publishedYear: "",
            smallThumbnailUrl: null
        }
    });

}(Backbone));

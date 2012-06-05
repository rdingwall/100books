Ohb.Models.PreviousRead = (function (Backbone, eventBus) {
    "use strict";

    return Backbone.Model.extend({

        urlRoot: "/api/v1/previousreads",

        defaults: {
            googleVolumeId: null,
            title: "",
            authors: "",
            publishedYear: "",
            smallThumbnailUrl: null
        },

        remove: function () {
            eventBus.trigger("previousread:removeRequested", this.id);
        }
    });

}(Backbone, Ohb.eventBus));

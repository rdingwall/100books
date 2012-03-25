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

        initialize: function () {
            eventBus.on("previousread:removed", this.onPreviousReadRemoved, this);
        },

        remove: function () {
            eventBus.trigger("previousread:removeRequested", this.id);
        },

        onPreviousReadRemoved: function (id) {
            if (id !== this.id) {
                return;
            }

            this.destroy();
        },

        sync: function (method, model, options) {
            if (method === "delete") {
                return; // do nothing
            }

            return Backbone.sync(method, model, options);
        }
    });

}(Backbone, Ohb.eventBus));

Ohb.Models.PreviousRead = (function (Backbone, eventBus, urlHelper) {
    "use strict";

    return Backbone.Model.extend({

        urlRoot: "/api/v1/previousreads",

        defaults: {
            googleVolumeId: null,
            title: "",
            authors: "",
            publishedYear: "",
            smallThumbnailUrl: null,
            viewUrl: null
        },

        initialize: function () {
            this.set("viewUrl", urlHelper.bookUrl(this.id, this.get("title"))); // for tests
        },

        parse: function (response) {
            response.viewUrl = urlHelper.bookUrl(response.id, response.title);
            return response;
        },

        remove: function () {
            eventBus.trigger("previousread:removeRequested", this.id);
        }
    });

}(Backbone, Ohb.eventBus, Ohb.urlHelper));

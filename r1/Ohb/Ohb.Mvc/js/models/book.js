var Ohb = window;

Ohb.Book = (function (Backbone, eventBus) {
    "use strict";

    return Backbone.Model.extend({
        id: null,
        hasPreviouslyRead: false,
        googleVolumeId: null,

        urlRoot: "/api/v1/books",

        publisher: "",
        title: "",
        authors: "",
        publishedYear: "",
        description: "",
        pageCount: 0,
        thumbnailUrl: null,
        smallThumbnailUrl: null,

        initialize: function () {
            this.on("change:hasPreviouslyRead", this.notifyStatusChange, this);
        },

        toggleStatus: function () {
            var previousStatus = this.get("hasPreviouslyRead");
            this.set({ hasPreviouslyRead: !previousStatus });
        },

        notifyStatusChange: function () {
            eventBus.trigger("book:statusChanged", this);
        },

        getSearchResultThumbnail: function () {
            return this.get("smallThumbnailUrl")
                || "img/search-result-no-cover.png";
        },

        getBookThumbnail: function () {
            return this.get("thumbnailUrl")
                || "img/book-no-cover.png";
        }
    });

}(Backbone, Ohb.eventBus));

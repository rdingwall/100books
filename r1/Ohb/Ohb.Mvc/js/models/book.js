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

        toggleStatus: function () {
            var previouslyRead = this.get("hasPreviouslyRead");
            this.set({ hasPreviouslyRead: !previouslyRead });

            var event = previouslyRead ?
                    "previousread:removeRequested" :
                    "previousread:addRequested";

            eventBus.trigger(event, this.id);
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

Ohb.Models.Book = (function (Backbone, eventBus, urlHelper) {
    "use strict";

    var Book =  Backbone.Model.extend({
        urlRoot: "/api/v1/books",

        defaults: {
            hasPreviouslyRead: false,
            googleVolumeId: null,
            publisher: "",
            title: "",
            authors: "",
            publishedYear: "",
            description: "",
            pageCount: 0,
            thumbnailUrl: null,
            smallThumbnailUrl: null,
            viewUrl: ""
        },

        parse: function (response) {
            response.viewUrl = urlHelper.bookUrl(response.id, response.title);
            return response;
        },

        initialize: function () {
            this.set("viewUrl", urlHelper.bookUrl(this.id, this.get("title"))); // for tests

            eventBus.on("previousread:added", function (id) {
                if (this.id === id) {
                    this.set("hasPreviouslyRead", true);
                }
            }, this);

            eventBus.on("previousread:removed", function (id) {
                if (this.id === id) {
                    this.set("hasPreviouslyRead", false);
                }
            }, this);
        },

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

    return Book;

}(Backbone, Ohb.eventBus, Ohb.urlHelper));

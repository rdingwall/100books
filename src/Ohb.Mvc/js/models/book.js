Ohb.Models.Book = (function (eventBus, urlHelper, Readable) {
    "use strict";

    var Book =  Readable.extend({
        urlRoot: "/api/v1/books",

        defaults: {
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
            this.initializeReadable();
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

}(Ohb.eventBus, Ohb.urlHelper, Ohb.Readable));

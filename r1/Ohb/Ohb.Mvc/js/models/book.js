"use strict";

var Ohb = window;

Ohb.Book = (function(Backbone) {

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

        getSearchResultThumbnail: function() {
            return this.get("smallThumbnailUrl")
                || "img/search-result-no-cover.png";
        },

        getBookThumbnail: function() {
            return this.get("thumbnailUrl")
                || "img/book-no-cover.png";
        }
    });

})(Backbone);

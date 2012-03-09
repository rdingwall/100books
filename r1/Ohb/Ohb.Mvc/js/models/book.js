/*globals define, Backbone */

define([
    'backbone'
],
    function (Backbone) {
        "use strict";

        var Book = Backbone.Model.extend({
            id: null,
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

            getSearchResultThumbnail: function() {
                return this.get("smallThumbnailUrl")
                    || "img/search-result-no-cover.png";
            },

            getBookThumbnail: function() {
                return this.get("thumbnailUrl")
                    || "img/book-no-cover.png";
            }
        });

        return Book;
    });

/*globals define, Backbone */

define([
    'backbone'
],
    function (Backbone) {
        "use strict";

        var Book = Backbone.Model.extend({
            book: {
                googleVolumeId: null,
                staticInfo: {
                    publisher: "",
                    id: "",
                    title: "",
                    authors: "",
                    publishedYear: "",
                    description: "",
                    pageCount: 0,
                    thumbnailUrl: null,
                    smallThumbnailUrl: null
                },
                id: null
            },
            hasPreviouslyRead: false,

            getSearchResultThumbnail: function() {

            },

            getBookThumbnail: function() {

            }
        });

        return Book;
    });

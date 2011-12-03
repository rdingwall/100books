﻿/*globals define, Backbone */

define([
    'backbone'
],
    function (Backbone) {
        "use strict";

        var SearchResult = Backbone.Model.extend({
            title: null,
            authors: null,
            smallThumbnailUrl: null,
            id: null
        });

        SearchResult.fromGoogle = function (volume) {
            var info = volume.volumeInfo;

            var title = info.title;
            if (info.subtitle) {
                title += ": " + info.subtitle;
            }

            var imageLinks = info.imageLinks,
                smallThumbnailUrl = imageLinks ? imageLinks.smallThumbnail : null,
                authors = info.authors ? info.authors.join(", ") : null;

            return new SearchResult({
                title: title,
                authors: authors,
                smallThumbnailUrl: smallThumbnailUrl,
                id: volume.id
            });
        };

        return SearchResult;
    });
"use strict";

var Ohb = window;

Ohb.SearchResult = (function (Backbone) {

    var searchResult = Backbone.Model.extend({
        defaults: {
            title: null,
            authors: null,
            smallThumbnailUrl: null
        }
    });

    searchResult.fromGoogle = function (volume) {
        var info = volume.volumeInfo;

        var title = info.title;
        if (info.subtitle) {
            title += ": " + info.subtitle;
        }

        var imageLinks = info.imageLinks,
            smallThumbnailUrl = imageLinks ? imageLinks.smallThumbnail : null,
            authors = info.authors ? info.authors.join(", ") : null;

        return new searchResult({
            title: title,
            authors: authors,
            smallThumbnailUrl: smallThumbnailUrl,
            id: volume.id
        });
    };

    return searchResult;

})(Backbone);

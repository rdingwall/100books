var Ohb = window;

Ohb.SearchResult = (function (Backbone) {
    "use strict";

    var SearchResult = Backbone.Model.extend({
        defaults: {
            title: null,
            authors: null,
            smallThumbnailUrl: null
        }
    });

    SearchResult.fromGoogle = function (volume) {
        var info = volume.volumeInfo,
            title = info.title,
            imageLinks = info.imageLinks,
            smallThumbnailUrl = imageLinks ? imageLinks.smallThumbnail : null,
            authors = info.authors ? info.authors.join(", ") : null;

        if (info.subtitle) {
            title += ": " + info.subtitle;
        }

        return new SearchResult({
            title: title,
            authors: authors,
            smallThumbnailUrl: smallThumbnailUrl,
            id: volume.id
        });
    };

    return SearchResult;

}(Backbone));

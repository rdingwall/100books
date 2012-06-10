Ohb.Models.SearchResult = (function (Backbone, eventBus, urlHelper) {
    "use strict";

    var SearchResult = Backbone.Model.extend({
        defaults: {
            title: null,
            authors: null,
            smallThumbnailUrl: null,
            hasRead: false,
            viewUrl: null
        },

        initialize: function () {
            this.set("viewUrl", urlHelper.bookUrl(this.id, this.get("title")));
        }
    });

    var placeholderImage = "img/search-result-no-cover.png",
        getThumbnail = function (imageLinks) {
            if (!imageLinks) {
                return placeholderImage;
            }
            return imageLinks.smallThumbnail || placeholderImage;
        };

    SearchResult.fromGoogle = function (volume) {
        var info = volume.volumeInfo,
            title = info.title,
            authors = info.authors ? info.authors.join(", ") : null;

        if (info.subtitle) {
            title += ": " + info.subtitle;
        }

        return new SearchResult({
            title: title,
            authors: authors,
            smallThumbnailUrl: getThumbnail(info.imageLinks),
            id: volume.id
        });
    };

    return SearchResult;

}(Backbone, Ohb.eventBus, Ohb.urlHelper));

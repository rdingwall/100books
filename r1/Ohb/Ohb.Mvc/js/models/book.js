Ohb.Models.Book = (function (Backbone, _, ReadableMixin) {
    "use strict";

    var properties = {
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
            smallThumbnailUrl: null
        },

        initialize: function () {
            this.bindReadableEvents();
        },

        getSearchResultThumbnail: function () {
            return this.get("smallThumbnailUrl")
                || "img/search-result-no-cover.png";
        },

        getBookThumbnail: function () {
            return this.get("thumbnailUrl")
                || "img/book-no-cover.png";
        }
    };

    _.extend(properties, ReadableMixin);

    return Backbone.Model.extend(properties);

}(Backbone, _, Ohb.Models.ReadableMixin));

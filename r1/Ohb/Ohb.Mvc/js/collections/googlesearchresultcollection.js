Ohb.Collections.GoogleSearchResultCollection = (function ($, _, Backbone, SearchResult) {
    "use strict";

    return Backbone.Collection.extend({
        model: SearchResult,

        url: function () {
            // plus q=search+term
            return "https://www.googleapis.com/books/v1/volumes?projection=lite&callback=?";
        },

        parse: function (response) {
            if (!response.items) {
                return;
            }

            var uniqueItems = _.uniq(response.items, false, function (result) {
                return result.id;
            });

            return _.map(uniqueItems, SearchResult.fromGoogle);
        }
    });

}($, _, Backbone, Ohb.Models.SearchResult));
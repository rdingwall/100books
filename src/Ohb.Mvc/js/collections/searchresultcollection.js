// Accepts an GoogleSearchResultCollection and enriches its SearchResult
// models with a hasRead property from the local API.
Ohb.Collections.SearchResultCollection = (function ($, _, Backbone, SearchResult) {
    "use strict";

    return Backbone.Collection.extend({
        model: SearchResult,

        initialize: function (googleResultsCollection) {
            this.googleResultsCollection = googleResultsCollection;
        },

        url: function () {
            var ids = _.map(this.googleResultsCollection.models, function (result) { return result.id; });
            return "/api/v1/books/" + ids.join() + "/statuses";
        },

        parse: function (response) {
            _.each(response, function (item) {
                var result = this.googleResultsCollection.get(item.googleVolumeId);
                result.set("hasRead", item.hasRead);
            }, this);

            return this.googleResultsCollection.models;
        }
    });

}($, _, Backbone, Ohb.Models.SearchResult));
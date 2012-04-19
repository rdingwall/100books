// Wraps around an existing set of SearchResult models and injects an extra
// property, hasRead, from the local API.
Ohb.Collections.SearchResultStatusCollection = (function ($, _, Backbone, SearchResult) {
    "use strict";

    return Backbone.Collection.extend({
        model: SearchResult,

        initialize: function (models) {
            this.googleResults = models;
        },

        url: function () {
            var ids = _.map(this.googleResults.models, function (result) { return result.id; });
            return "/api/v1/books/" + ids + "/statuses";
        },

        parse: function (response) {
            _.each(response, function (item) {
                var result = this.googleResults.get(item.googleVolumeId);
                result.set("hasRead", item.hasRead);
            }, this);

            return this.googleResults.models;
        }
    });

}($, _, Backbone, Ohb.Models.SearchResult));
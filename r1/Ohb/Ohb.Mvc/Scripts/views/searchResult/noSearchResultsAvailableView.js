/*globals define */
define([
    'backbone',
    'mustache',
    'jquery',
    'models/searchresult',
    'text!/templates/searchresult/noSearchResultsAvailable.html'
], function (Backbone, Mustache, $, SearchResult, noSearchResultsAvailableTemplate) {
    "use strict";

    return Backbone.View.extend({
        model: SearchResult,
        tagName: "div",
        className: "no-search-results-available",

        render: function () {
            $(this.el).empty();
            $(this.el).append(noSearchResultsAvailableTemplate);

            return this;
        }
    });
});

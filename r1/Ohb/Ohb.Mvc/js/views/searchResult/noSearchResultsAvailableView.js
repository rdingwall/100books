"use strict";

var Ohb = window;

var template = '<div class="row searchresult-no-results-available"> \
    <p>No books found, sorry! Try broadening your search.</p> \
</div>';

Ohb.NoSearchResultsAvailableView = (function(
    Backbone, SearchResult, noSearchResultsAvailableTemplate) {

    return Backbone.View.extend({
        model: SearchResult,
        tagName: "div",
        className: "no-search-results-available",


        // need to put this back in.
        //'lib/requires/text!/templates/searchresult/noSearchResultsAvailable.html'

        render: function () {
            $(this.el).empty();
            $(this.el).append(noSearchResultsAvailableTemplate);

            return this;
        }
    });

})(Backbone, Ohb.SearchResult, template);


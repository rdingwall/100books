$(function () {
    "use strict";

    // This is pretty complicated - it does a two-step search, first looking up
    // Google for the results then enriching them from the local API to show
    // whether you have already read them or not.
    Ohb.Commands.SearchCommand = (function (
        $,
        eventBus,
        GoogleSearchResultCollection,
        SearchResultCollection,
        SearchResultCollectionView
    ) {

        var log = $.jog("SearchCommand");

        var error = function () {
            log.severe("Search failed!");
            eventBus.trigger("search:completed");
            eventBus.trigger("search:failed");
        };

        var renderResults = function (collection) {
            new SearchResultCollectionView({
                el: "#search-results",
                collection: collection
            }).render();

            eventBus.trigger("search:completed");
        };

        var enrichSearchResults = function (collection) {
            if (collection.length === 0) {
                // No point checking book statuses if there were no
                // results, just render them as is.
                return renderResults(collection);
            }

            log.info("Got google search results. Looking up local API for corresponding statuses...");

            // Step 2: enrich SearchResult models with local hasRead property.
            new SearchResultCollection(collection).fetch({
                success: renderResults,
                error: error
            });
        };

        var command = function () {};

        command.prototype.execute = function (query) {
            log.info("Searching for " + query + "...");
            eventBus.trigger("search:began", query);

            // Step 1: fetch SearchResult models from Google.
            new GoogleSearchResultCollection().fetch({
                data: { q: query },
                success: enrichSearchResults,
                error: error
            });
        };

        return command;
    }(
        $,
        Ohb.eventBus,
        Ohb.Collections.GoogleSearchResultCollection,
        Ohb.Collections.SearchResultCollection,
        Ohb.Views.SearchResultCollectionView
    ));
});
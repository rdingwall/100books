$(function () {
    "use strict";

    Ohb.Commands.SearchCommand = (function (
        $,
        eventBus,
        SearchResultCollection,
        SearchResultCollectionView,
        SearchResultStatusCollection
    ) {

        var log = $.jog("SearchCommand");

        var error = function () {
            log.severe("Search failed!");
            eventBus.trigger("search:completed");
            eventBus.trigger("search:failed");
        };

        var endSearch = function (collection) {
            new SearchResultCollectionView({
                el: "#search-results",
                collection: collection
            }).render();

            eventBus.trigger("search:completed");
        };

        var lookUpLocalStatus = function (collection) {
            if (collection.length === 0) {
                // No point checking book statuses if there were no
                // results, just render them as is.
                return endSearch(collection);
            }

            log.info("Got google search results. Looking up local API for corresponding statuses...");

            new SearchResultStatusCollection(collection).fetch({
                success: endSearch,
                error: error
            });
        };

        var command = function () {};

        command.prototype.execute = function (query) {
            log.info("Searching for " + query + "...");
            eventBus.trigger("search:began", query);

            new SearchResultCollection().fetch({
                data: { q: query },
                success: lookUpLocalStatus,
                error: error
            });
        };

        return command;
    }(
        $,
        Ohb.eventBus,
        Ohb.Collections.SearchResultCollection,
        Ohb.Views.SearchResultCollectionView,
        Ohb.Collections.SearchResultStatusCollection
    ));
});
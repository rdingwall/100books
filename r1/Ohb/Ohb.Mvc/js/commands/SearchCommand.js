$(function () {
    "use strict";

    Ohb.Commands.SearchCommand = (function (
        $,
        eventBus,
        SearchResultCollection,
        SearchResultCollectionView
    ) {

        var log = $.jog("SearchCommand");

        var command = function () {};

        command.prototype.execute = function (query) {
            log.info("Searching for " + query + "...");
            eventBus.trigger("search:began", query);

            new SearchResultCollection().fetch(
                {
                    data: { q: query },
                    success: function (collection) {
                        new SearchResultCollectionView({
                            el: "#search-results",
                            collection: collection
                        }).render();

                        eventBus.trigger("search:completed");
                    },
                    error: function () {
                        log.severe("Search failed!");
                        eventBus.trigger("search:completed");
                        eventBus.trigger("search:failed");
                    }
                }
            );
        };

        return command;
    }(
        $,
        Ohb.eventBus,
        Ohb.Collections.SearchResultCollection,
        Ohb.Views.SearchResultCollectionView
    ));
});
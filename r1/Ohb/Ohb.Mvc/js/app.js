$(function () {

    "use strict";

    var Ohb = window;

    Ohb.app = (function (
        $,
        _,
        Backbone,
        Router,
        eventBus,
        MenuBarView,
        SearchResultCollectionView,
        SearchResultCollection,
        BookDetailsView
    ) {

        var log = $.jog("App");

        return {

            initialize: function () {
                log.info("initializing router...");
                eventBus.on("search:requested", this.search, this);
                eventBus.on("search:failed", this.onSearchFailed, this);
                eventBus.on("search:resultSelected", this.onSearchResultSelected, this);

                // initialize singleton views
                this.menuBarView = new MenuBarView();
                this.searchResultCollectionView = new SearchResultCollectionView();
                this.bookDetailsView = new BookDetailsView();

                this.router = new Router();
            },

            search: function (query) {
                log.info("Searching for " + query + "...");
                eventBus.trigger("search:began", query);

                new SearchResultCollection().fetch(
                    {
                        data: { q: query },
                        success: function (collection) {
                            eventBus.trigger("search:completed");
                            if (collection.length === 0) {
                                eventBus.trigger("search:returnedNoResults");
                            } else {
                                eventBus.trigger("search:resultsArrived", collection);
                            }
                        },
                        error: function () {
                            log.severe("Search failed!");
                            eventBus.trigger("search:completed");
                            eventBus.trigger("search:failed");
                        }
                    }
                );
            },

            onSearchFailed: function () {
                log.info('showing search failed modal...');
                $("#search-failed-modal").modal({ keyboard: true, show: true });
            },

            onSearchResultSelected: function (searchResult) {
                log.info("navigating to show book " + searchResult.id);

                // this = that. Bit of a hack.
                this.router.navigate("books/" + searchResult.id, { trigger: true });
            }
        };
    }(
        $,
        _,
        Backbone,
        Ohb.Router,
        Ohb.eventBus,
        Ohb.MenuBarView,
        Ohb.SearchResultCollectionView,
        Ohb.SearchResultCollection,
        Ohb.BookDetailsView
    ));
});
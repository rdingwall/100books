$(function() {

    "use strict";

    var Ohb = window;

    Ohb.App = (function (
        $,
        _,
        Backbone,
        Router,
        eventBus,
        MenuBarView,
        SearchResultCollectionView,
        SearchResultCollection
    ) {

        var log = $.jog("App");

        return {

            initialize: function () {
                log.info("initializing router...");
                eventBus.on('searchRequested', this.search, this);
                eventBus.on('searchFailed', this.onSearchFailed, this);
                eventBus.on('searchResultSelected', this.onSearchResultSelected, this);
                this.menuBarView = new MenuBarView();
                this.searchResultCollectionView = new SearchResultCollectionView();
                this.router = new Router();
            },

            search: function (query) {
                log.info("Searching for " + query + "...");
                eventBus.trigger("searchBegan", query);

                new SearchResultCollection().fetch(
                    {
                        data: { q: query },
                        success: function(collection) {
                            eventBus.trigger("searchCompleted");
                            if (collection.length == 0) {
                                eventBus.trigger("searchReturnedNoResults");
                            }
                            else {
                                eventBus.trigger("searchResultsArrived", collection);
                            }
                        },
                        error: function() {
                            log.severe("Search failed!");
                            eventBus.trigger("searchCompleted");
                            eventBus.trigger("searchFailed");
                        }
                    });
            },

            onSearchFailed: function () {
                log.info('showing search failed modal...');
                $("#search-failed-modal").modal({ keyboard: true, show: true });
            },

            onSearchResultSelected: function (searchResult) {
                log.info('navigating to show book ' + searchResult.id);

                // this = that. Bit of a hack.
                this.router.navigate("books/" + searchResult.id);
            }
        };
    })($, _,
        Backbone,
        Ohb.Router,
        Ohb.EventBus,
        Ohb.MenuBarView,
        Ohb.SearchResultCollectionView,
        Ohb.SearchResultCollection);
});
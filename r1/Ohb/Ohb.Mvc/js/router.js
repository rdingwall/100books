var Ohb = window;

Ohb.Router = (function ($, Backbone, SearchResult, SearchResultCollection, eventBus) {
    "use strict";

    var log = $.jog("Router"),
        instance = null,
        AppRouter = Backbone.Router.extend({

            routes: {
                "book/:id/:dummy": "openBook"
            },

            initialize: function () {
                log.info("initializing router...");
                eventBus.bind('searchRequested', this.search);
                eventBus.bind('searchFailed', this.onSearchFailed);
                eventBus.bind('searchResultSelected', this.onSearchResultSelected);
            },

            search: function (q) {
                log.info("Searching for " + q + "...");

                eventBus.trigger("searchBegan", q);

                $.getJSON("https://www.googleapis.com/books/v1/volumes?callback=?",
                    {
                        q: decodeURIComponent(q),
                        projection: "lite"
                    },
                    function (json) {
                        try {
                            var results = new SearchResultCollection();

                            if (!json.items) {
                                eventBus.trigger("searchCompleted");
                                eventBus.trigger("searchReturnedNoResults");
                                return;
                            }

                            $(json.items).each(function () {
                                results.add(SearchResult.fromGoogle($(this)[0]));
                            });

                            eventBus.trigger("searchResultsArrived", results);
                        } catch (e) {
                            log.error("Search error: " + e.message);
                            eventBus.trigger("searchCompleted");
                            eventBus.trigger("searchFailed");
                        }

                        eventBus.trigger("searchCompleted");
                    })
                    .error(function (jqXHR, textStatus, errorThrown) {
                        log.warning("Search error: " + textStatus);
                        eventBus.trigger("searchCompleted");
                        eventBus.trigger("searchFailed");
                    });
            },

            onSearchFailed: function () {
                log.info('showing search failed modal...');
                $("#search-failed-modal").modal({ keyboard: true, show: true });
            },

            onSearchResultSelected: function (searchResult) {
                log.info('navigating to show book ' + searchResult.id);

                // this = that. Bit of a hack.
                AppRouter.getInstance().navigate("books/" + searchResult.id);
            }
        });

    AppRouter.getInstance = function () {
        // singleton
        if (instance === null) {
            instance = new AppRouter();
        }
        return instance;
    };

    return AppRouter.getInstance();

})($, Backbone, Ohb.SearchResult, Ohb.SearchResultCollection, Ohb.EventBus);

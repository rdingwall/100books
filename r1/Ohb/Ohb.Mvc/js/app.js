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
        SearchResultCollectionView
    ) {

        var log = $.jog("App");

        var distinct = function(items, keyCallback) {
            var keys = new Array();
            var results = new Array();

            $.each(items, function(i, item) {
                var key = keyCallback(item);
                if (jQuery.inArray(key, keys) === -1) {
                    results.push(item);
                    keys.push(key);
                }
            });

            return results;
        };

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

                            var uniqueItems = distinct(json.items, function(result) { return result.id; });

                            $(uniqueItems).each(function () {
                                log.info("Adding result " + this.id);
                                results.add(SearchResult.fromGoogle(this));
                            });

                            eventBus.trigger("searchResultsArrived", results);
                        } catch (e) {
                            log.severe("Search error: " + e.message);
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
                this.router.navigate("books/" + searchResult.id);
            }
        };
    })($, _, Backbone, Ohb.Router, Ohb.EventBus, Ohb.MenuBarView, Ohb.SearchResultCollectionView);
});
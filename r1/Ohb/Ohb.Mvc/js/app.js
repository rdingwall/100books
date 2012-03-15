$(function () {

    "use strict";

    Ohb.app = (function (
        $,
        _,
        Backbone,
        Router,
        eventBus,
        MenuBarView,
        SearchResultCollectionView,
        SearchResultCollection,
        BookDetailsView,
        MyProfileView
    ) {

        var log = $.jog("App");

        var bookHashFragment = function (searchResult) {
            var fragment = "books/" + searchResult.id;

            var title = searchResult.get("title");
            if (title) {
                var titleSlug = (searchResult.get("title") || "")
                    .replace(/[^\w\s]|_/g, "")
                    .replace(/\s+/g, "-")
                    .toLowerCase();

                fragment += "/" + titleSlug;
            }

            return fragment;
        };

        return {

            initialize: function () {
                log.info("initializing router...");
                eventBus.on("search:requested", this.search, this);
                eventBus.on("search:failed", this.onSearchFailed, this);
                eventBus.on("search:resultSelected", this.onSearchResultSelected, this);
                eventBus.on("previousread:addRequested", this.onPreviousReadAddRequested, this);
                eventBus.on("previousread:removeRequested", this.onPreviousReadRemoveRequested, this);

                // initialize singleton views
                this.menuBarView = new MenuBarView();
                this.searchResultCollectionView = new SearchResultCollectionView();
                this.bookDetailsView = new BookDetailsView();
                this.profileView = new MyProfileView();

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
                this.router.navigate(bookHashFragment(searchResult), { trigger: true });
            },

            onPreviousReadAddRequested : function (id) {
                $.ajax({
                    url: "/api/v1/previousreads/" + id,
                    type: 'PUT',
                    success: function () {
                        eventBus.trigger("previousread:added", id);
                    }
                });
            },

            onPreviousReadRemoveRequested : function (id) {
                $.ajax({
                    url: "/api/v1/previousreads/" + id,
                    type: 'DELETE',
                    success: function () {
                        eventBus.trigger("previousread:removed", id);
                    }
                });
            }
        };
    }(
        $,
        _,
        Backbone,
        Ohb.Router,
        Ohb.eventBus,
        Ohb.Views.MenuBarView,
        Ohb.Views.SearchResultCollectionView,
        Ohb.Collections.SearchResultCollection,
        Ohb.Views.BookDetailsView,
        Ohb.Views.MyProfileView
    ));
});
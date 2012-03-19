$(function () {

    "use strict";

    Ohb.app = (function (
        $,
        _,
        Backbone,
        Router,
        eventBus,
        SearchResultCollectionView,
        SearchResultCollection,
        BookDetailsView,
        ProfileCardView,
        Profile,
        mainRegion,
        Book,
        CompositeProfileView,
        PreviousReadCollection
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
                log.info("Initializing router...");

                eventBus.on("myprofile:requested", this.onMyProfileRequested, this);
                eventBus.on("book:requested", this.onBookRequested, this);
                eventBus.on("search:requested", this.search, this);
                eventBus.on("search:failed", this.onSearchFailed, this);
                eventBus.on("search:result:selected", this.onSearchResultSelected, this);
                eventBus.on("previousread:addRequested", this.onPreviousReadAddRequested, this);
                eventBus.on("previousread:removeRequested", this.onPreviousReadRemoveRequested, this);

                this.router = new Router();
            },

            onMyProfileRequested: function () {
                log.info("Fetching user from API...");

                var view = new CompositeProfileView();
                mainRegion.show(view);

                var model = new Profile({ id: "me" });

                model.fetch({
                    success: function (model) {
                        view.renderProfileCard(model);
                    },
                    error: function () {
                        mainRegion.showError("Sorry, there was an error retrieving this profile.");
                    }
                });

                var collection = new PreviousReadCollection();
                collection.fetch({
                    success: function (collection) {
                        view.renderPreviousReads(collection);
                    },
                    error: function () {
                        mainRegion.showError("Sorry, there was an error retrieving this profile.");
                    }
                });
            },

            onBookRequested: function (id) {
                log.info("Fetching book from API...");
                var model = new Book({ id: id });
                model.fetch({
                    success: function (model) {
                        mainRegion.show(new BookDetailsView({ model: model }));
                    },
                    error: function () {
                        mainRegion.showError("Sorry, there was an error retrieving this book.");
                    }
                });
            },

            search: function (query) {
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
            },

            onSearchFailed: function () {
                log.info('Showing search failed modal...');
                $("#search-failed-modal").modal({ keyboard: true, show: true });
            },

            onSearchResultSelected: function (searchResult) {
                log.info("Navigating to show book " + searchResult.id);
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
        Ohb.Views.SearchResultCollectionView,
        Ohb.Collections.SearchResultCollection,
        Ohb.Views.BookDetailsView,
        Ohb.Views.ProfileCardView,
        Ohb.Models.Profile,
        Ohb.mainRegion,
        Ohb.Models.Book,
        Ohb.Views.CompositeProfileView,
        Ohb.Collections.PreviousReadCollection
    ));
});
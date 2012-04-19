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
        PreviousReadCollection,
        PreviousRead,
        SearchCommand,
        ViewProfileCommand,
        ViewBookCommand,
        AddPreviousReadCommand,
        RemovePreviousReadCommand
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

                this.router = new Router();

                eventBus.on("myprofile:requested", new ViewProfileCommand().execute, this);
                eventBus.on("book:requested", new ViewBookCommand().execute, this);
                eventBus.on("search:requested", new SearchCommand().execute, this);
                eventBus.on("search:failed", this.onSearchFailed, this);
                eventBus.on("search:result:selected", this.onSearchResultSelected, this);
                eventBus.on("previousread:addRequested", new AddPreviousReadCommand().execute, this);
                eventBus.on("previousread:removeRequested", new RemovePreviousReadCommand().execute, this);
            },

            onSearchFailed: function () {
                log.info('Showing search failed modal...');
                $("#search-failed-modal").modal({ keyboard: true, show: true });
            },

            onSearchResultSelected: function (searchResult) {
                log.info("Navigating to show book " + searchResult.id);
                this.router.navigate(bookHashFragment(searchResult), { trigger: true });
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
        Ohb.Collections.PreviousReadCollection,
        Ohb.Models.PreviousRead,
        Ohb.Commands.SearchCommand,
        Ohb.Commands.ViewProfileCommand,
        Ohb.Commands.ViewBookCommand,
        Ohb.Commands.AddPreviousReadCommand,
        Ohb.Commands.RemovePreviousReadCommand
    ));
});
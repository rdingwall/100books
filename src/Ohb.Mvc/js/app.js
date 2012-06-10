$(function () {

    "use strict";

    Ohb.app = (function (
        $,
        Router,
        eventBus,
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
                log.info("Initializing app...");

                this.router = new Router();

                eventBus.on("myprofile:requested", new ViewProfileCommand().execute);
                eventBus.on("book:requested", new ViewBookCommand().execute);
                eventBus.on("search:requested", new SearchCommand().execute);
                eventBus.on("search:failed", this.onSearchFailed, this);
                eventBus.on("search:result:selected", this.onSearchResultSelected, this);
                eventBus.on("previousread:addRequested", new AddPreviousReadCommand().execute);
                eventBus.on("previousread:removeRequested", new RemovePreviousReadCommand().execute);
            },

            onSearchFailed: function () {
                log.info('Showing search failed modal...');
                $("#search-failed-modal").modal({ keyboard: true, show: true });
            },

            onSearchResultSelected: function (searchResult) {
                log.info("Navigating to show book " + searchResult.id);
                this.router.navigate(searchResult.get("viewUrl"), { trigger: true });
            }
        };
    }(
        $,
        Ohb.Router,
        Ohb.eventBus,
        Ohb.Commands.SearchCommand,
        Ohb.Commands.ViewProfileCommand,
        Ohb.Commands.ViewBookCommand,
        Ohb.Commands.AddPreviousReadCommand,
        Ohb.Commands.RemovePreviousReadCommand
    ));
});
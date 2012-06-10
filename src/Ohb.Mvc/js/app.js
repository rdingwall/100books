$(function () {

    "use strict";

    Ohb.app = (function (
        $,
        eventBus,
        SearchCommand,
        ViewProfileCommand,
        ViewBookCommand,
        AddPreviousReadCommand,
        RemovePreviousReadCommand
    ) {

        var log = $.jog("App");

        return {

            initialize: function () {
                log.info("Initializing app...");

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
            }
        };
    }(
        $,
        Ohb.eventBus,
        Ohb.Commands.SearchCommand,
        Ohb.Commands.ViewProfileCommand,
        Ohb.Commands.ViewBookCommand,
        Ohb.Commands.AddPreviousReadCommand,
        Ohb.Commands.RemovePreviousReadCommand
    ));
});
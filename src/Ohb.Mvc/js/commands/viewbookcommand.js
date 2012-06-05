$(function () {
    "use strict";

    Ohb.Commands.ViewBookCommand = (function (
        $,
        mainRegion,
        Book,
        BookDetailsView
    ) {

        var log = $.jog("ViewBookCommand");

        var command = function () {};

        command.prototype.execute = function (id) {
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
        };

        return command;
    }(
        $,
        Ohb.mainRegion,
        Ohb.Models.Book,
        Ohb.Views.BookDetailsView
    ));
});
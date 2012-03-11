$(function () {

    "use strict";

    var Ohb = window;

    Ohb.BookDetailsView = (function ($, Backbone, _, Mustache,
                           eventBus, Book) {

        var log = $.jog("BookDetailsView");

        return Backbone.View.extend({

            el: "#content-main",

            initialize: function () {
                eventBus.on("book:requested", this.onBookRequested, this);
            },

            onBookRequested: function (id) {
                log.info("Fetching book from API...");
                this.model = new Book({ id: id });
                this.model.fetch({
                    success: _.bind(this.render, this),
                    error: _.bind(this.onFetchError, this)
                });
            },

            render: function () {
                log.info("Successfully fetched book. Rendering.");

                $.get("/templates/book/bookdetails.html", "text",
                    _.bind(function (template) {
                        $(this.el).html(Mustache.to_html(template, this.model.toJSON()));
                        $(this.el).show();
                        eventBus.trigger("book:rendered", this.model);
                    }, this));

                return this;
            },

            onFetchError: function () {
                log.warning("Error loading book");

                $.get("/templates/book/fetcherror.html", "text",
                    _.bind(function (template) {
                        $(this.el).html(template);
                        eventBus.trigger("book:fetchError");
                    }, this));
            }
        });

    }(
        $,
        Backbone,
        _,
        Mustache,
        Ohb.eventBus,
        Ohb.Book
    ));
});
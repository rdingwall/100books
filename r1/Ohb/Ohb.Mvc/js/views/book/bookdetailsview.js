$(function () {

    "use strict";

    var Ohb = window;

    Ohb.BookDetailsView = (function ($, Backbone, _, Mustache,
                           eventBus, Book) {

        var log = $.jog("BookDetailsView");

        return Backbone.View.extend({

            el: "#content-main",

            events: {
                "click .status-toggle-button" : "toggleStatus"
            },

            initialize: function () {
                eventBus.on("book:requested", this.onBookRequested, this);
                this.setModel(this.model);
            },

            setModel: function (model) {
                if (this.model) {
                    // Unbind previous model's events...
                    this.model.off("change:hasPreviouslyRead",
                        this.onModelStatusChanged, this);
                }
                if (model) {
                    this.model = model;
                    this.model.on("change:hasPreviouslyRead",
                        this.onModelStatusChanged, this);
                }
            },

            onBookRequested: function (id) {
                log.info("Fetching book from API...");
                this.setModel(new Book({ id: id }));
                this.model.fetch({
                    success: _.bind(this.render, this),
                    error: _.bind(this.onFetchError, this)
                });
            },

            render: function () {
                log.info("Successfully fetched book. Rendering.");

                $.get("/templates/book/bookdetails.html", "text",
                    _.bind(function (template) {
                        var el = $(Mustache.to_html(template, this.model.toJSON()));
                        this.renderStatusButtonPartial(el);
                        $(this.el).html(el);
                        $(this.el).show();
                        eventBus.trigger("book:rendered", this.model);
                    }, this));

                return this;
            },

            onModelStatusChanged: function () {
                this.renderStatusButtonPartial($(this.el));
            },

            renderStatusButtonPartial: function (el) {

                var newButton;
                if (this.model.get("hasPreviouslyRead")) {
                    newButton = $("<a class='status-toggle-button btn btn-success large'><i class='icon-ok icon-white'></i>You have read this book</a>");

                } else {
                    newButton = $("<a class='status-toggle-button btn large'><i class='icon-remove'></i>You have not read this book</a>");
                }

                $(el).find(".status-toggle-region").html(newButton);
            },

            onFetchError: function () {
                log.warning("Error loading book");

                $.get("/templates/book/fetcherror.html", "text",
                    _.bind(function (template) {
                        $(this.el).html(template);
                        eventBus.trigger("book:fetchError");
                    }, this));
            },

            toggleStatus: function (event) {
                event.preventDefault();
                this.model.toggleStatus();
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
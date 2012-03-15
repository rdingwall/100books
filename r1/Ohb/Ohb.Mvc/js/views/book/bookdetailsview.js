$(function () {

    "use strict";

    Ohb.Views.BookDetailsView = (function ($, Backbone, _, Mustache,
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
                        this.updateToggleButton(el);
                        $(this.el).html(el);
                        $(this.el).show();
                        eventBus.trigger("book:rendered", this.model);
                    }, this));

                return this;
            },

            onModelStatusChanged: function () {
                this.updateToggleButton($(this.el));
            },

            updateToggleButton: function (el) {
                if (this.model.get("hasPreviouslyRead")) {
                    $(el).find("#book-add-previousread-button").hide();
                    $(el).find("#book-remove-previousread-button").css("display", "block");
                } else {
                    $(el).find("#book-remove-previousread-button").hide();
                    $(el).find("#book-add-previousread-button").css("display", "block");
                }
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
        Ohb.Models.Book
    ));
});
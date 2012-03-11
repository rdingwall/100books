$(function () {

    "use strict";

    var Ohb = window;

    var template = '<div class="book-details"> \
        <h3>{{ title }}</h3> \
        <p>{{ authors }}</p> \
        <p>{{ description }}</p>\
    </div>';

    Ohb.BookDetailsView = (function ($, Backbone, _, Mustache,
                           eventBus, Book, bookDetailsTemplate) {

        var log = $.jog("BookDetailsView");

        return Backbone.View.extend({

            el: "#content-main",

            initialize: function () {
                eventBus.on("search:resultSelected", this.onResultSelected, this);
            },

            onResultSelected: function (searchResult) {
                log.info("Fetching book from API...");
                this.model = new Book({ id: searchResult.id });
                this.model.fetch({ success: _.bind(this.render, this) });
            },

            render: function () {
                log.info("Successfully fetched book. Rendering.");
                $(this.el).html(Mustache.to_html(bookDetailsTemplate,
                    this.model.toJSON()));
                $(this.el).show();
                eventBus.trigger("book:rendered", this.model);
                return this;
            }
        });

    }(
        $,
        Backbone,
        _,
        Mustache,
        Ohb.eventBus,
        Ohb.Book,
        template
    ));
});
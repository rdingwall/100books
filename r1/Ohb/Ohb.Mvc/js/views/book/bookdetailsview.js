$(function () {

    "use strict";

    var Ohb = window;

    var template = '<div class="book-details row"> \
        <div class="row span16">\
            <h1>{{ title }}</h1>\
            {{ authors }}\
        </div>\
        <div class="span3"><img src="{{ thumbnailUrl }}" title="{{ title }}" alt="{{ title }}" /> </div>\
        <div class="span7">\
            <p>{{{ description }}}</p>\
        </div>\
        <div class="span2">\
        <input type="button" value="\
        {{#hasPreviouslyRead}}\
        No I haven\'t\
        {{/hasPreviouslyRead}}\
        {{^hasPreviouslyRead}}\
        Yes I have\
        {{/hasPreviouslyRead}}\
        " />\
        </div>\
     </div>';

    var fetchErrorTemplate = '<div class="book-details">\
        <div class="book-details-error">We\'re sorry, there was an error loading this book.</div>\
        </div>';

    Ohb.BookDetailsView = (function ($, Backbone, _, Mustache,
                           eventBus, Book, bookDetailsTemplate,
                           fetchErrorTemplate) {

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
                $(this.el).html(Mustache.to_html(bookDetailsTemplate,
                    this.model.toJSON()));
                $(this.el).show();
                eventBus.trigger("book:rendered", this.model);
                return this;
            },

            onFetchError: function () {
                log.warning("Error loading book");
                $(this.el).html(fetchErrorTemplate);
                eventBus.trigger("book:fetchError");
            }
        });

    }(
        $,
        Backbone,
        _,
        Mustache,
        Ohb.eventBus,
        Ohb.Book,
        template,
        fetchErrorTemplate
    ));
});
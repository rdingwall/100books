$(function () {

    "use strict";

    var template = '<div class="book-details" xmlns="http://www.w3.org/1999/html">\
        <div class="row span12">\
        <h1 class="book-details-title">{{ title }}</h1>\
        {{ authors }}{{#publishedYear}}, {{publishedYear}}{{/publishedYear}}\
        </div>\
            <div class="row">\
                <div class="span2">\
                    <img src="{{ thumbnailUrl }}" title="{{ title }}" alt="{{ title }}" />\
                </div>\
                <div class="span8">\
                    <p>{{{ description }}}</p>\
                </div>\
                <div class="span2">\
                    <a id="book-remove-previousread-button" class="hide status-toggle-button btn btn-success large"><i class="icon-ok icon-white"></i> You have read this book</a>\
                    <a id="book-add-previousread-button" class="hide status-toggle-button btn large">You have not read this book</a>\
                </div>\
            </div>\
        </div>';

    Ohb.Views.BookDetailsView = (function ($, _, Mustache,
                           template, ReadableView) {

        var log = $.jog("BookDetailsView");

        return ReadableView.extend({

            className: "book-details",

            events: {
                "click .status-toggle-button" : "toggleStatus"
            },

            render: function () {
                log.info("Rendering BookDetailsView.");

                var el = $(Mustache.to_html(template, this.model.toJSON()));
                this.updateToggleButton(el);
                this.$el.html(el);

                return this;
            },

            updateToggleButton: function (el) {
                if (this.model.get("hasPreviouslyRead")) {
                    el.find("#book-add-previousread-button").hide();
                    el.find("#book-remove-previousread-button").css("display", "block");
                } else {
                    el.find("#book-remove-previousread-button").hide();
                    el.find("#book-add-previousread-button").css("display", "block");
                }
            }
        });
    }(
        $,
        _,
        Mustache,
        template,
        Ohb.Views.ReadableView
    ));
});
Ohb.Views.SearchResultView = (function (
    Backbone,
    Mustache,
    $,
    eventBus
) {
    "use strict";

    return Backbone.View.extend({
        tagName: "div",
        className: "book-search-result",

        events: {
            "click": "select"
        },

        render: function () {
            $.get("/templates/searchResult/searchResult.html", "text",
                _.bind(function (template) {
                    $(this.el).html(Mustache.to_html(template, this.model.toJSON()));
                    $(this.el).attr("id", "book-search-result-" + this.model.id);
                }, this));

            return this;
        },

        select: function () {
            eventBus.trigger("search:resultSelected", this.model);
        }
    });
}(Backbone, Mustache, $, Ohb.eventBus));

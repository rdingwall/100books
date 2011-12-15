/*globals define */
define([
    'backbone',
    'mustache',
    'jquery',
    'models/searchresult',
    'lib/requires/text!/templates/searchresult/searchresult.html',
    'eventbus'
], function (
    Backbone,
    Mustache,
    $,
    SearchResult,
    searchResultTemplate,
    EventBus
) {
    "use strict";

    return Backbone.View.extend({
        model: SearchResult,
        tagName: "div",
        className: "book-search-result",

        events: {
            "click": "select"
        },

        render: function () {
            $(this.el).empty();
            $(this.el).append(Mustache.to_html(searchResultTemplate, this.model.toJSON()));
            return this;
        },

        select: function () {
            EventBus.trigger('searchResultSelected', this.model);
        }
    });
});

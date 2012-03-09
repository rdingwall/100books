"use strict";

var Ohb = window;

var template = '<div class="row"> \
    <div class="span2"> \
    <img src="{{ smallThumbnailUrl }}" alt="{{ title }}" /> \
    </div> \
<div class="span8"> \
    <h3><a href="/books/{{ id }}" class="searchresult-title">{{ title }}</a></h3> \
    <p class="searchresult-authors">{{ authors }}</p> \
</div> \
</div>';

Ohb.SearchResultView = (function (
    Backbone,
    Mustache,
    $,
    //SearchResult,
    searchResultTemplate,
    EventBus
) {
    return Backbone.View.extend({
        //model: SearchResult,
        tagName: "div",
        className: "book-search-result",

        events: {
            "click": "select"
        },

        //'lib/requires/text!/templates/searchresult/searchresult.html',
        render: function () {
            $(this.el).empty();
            $(this.el).append(Mustache.to_html(searchResultTemplate, this.model.toJSON()));
            return this;
        },

        select: function () {
            EventBus.trigger('searchResultSelected', this.model);
        }
    });

})(Backbone, Mustache, $, /*Ohb.SearchResult,*/ template, Ohb.EventBus);

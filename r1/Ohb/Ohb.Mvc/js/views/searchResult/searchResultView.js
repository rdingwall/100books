"use strict";

var Ohb = window;

var template = '<div class="row"> \
    <div class="span2"> \
    <img src="{{ smallThumbnailUrl }}" alt="{{ title }}" /> \
    </div> \
<div class="span8"> \
    <h3 class="searchresult-title">{{ title }}</h3> \
    <p class="searchresult-authors">{{ authors }}</p> \
</div> \
</div>';

Ohb.SearchResultView = (function (
    Backbone,
    Mustache,
    $,
    searchResultTemplate,
    EventBus
) {
    return Backbone.View.extend({
        tagName: "div",
        className: "book-search-result",

        events: {
            "click": "select"
        },

        //'lib/requires/text!/templates/searchresult/searchresult.html',
        render: function () {
            $(this.el).html(Mustache.to_html(searchResultTemplate, this.model.toJSON()));
            $(this.el).attr("id", "book-search-result-" + this.model.id);
            return this;
        },

        select: function () {
            //alert("you clicked me!");
            EventBus.trigger('searchResultSelected', this.model);
        }
    });

})(Backbone, Mustache, $, template, Ohb.EventBus);

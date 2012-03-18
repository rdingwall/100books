Ohb.Views.SearchResultView = (function (
    Backbone,
    Mustache,
    $,
    eventBus
) {
    "use strict";

    var template = '<div class="row"> \
        <div class="span2"> \
            <img src="{{ smallThumbnailUrl }}" alt="{{ title }}" /> \
            </div> \
        <div class="span8"> \
            <h3 class="searchresult-title">{{ title }}</h3> \
            <p class="searchresult-authors">{{ authors }}</p> \
        </div> \
    </div>';

    return Backbone.View.extend({
        tagName: "div",
        className: "book-search-result",

        events: {
            "click": "select"
        },

        render: function () {
            $(this.el).html(Mustache.to_html(template, this.model.toJSON()));
            $(this.el).attr("id", "book-search-result-" + this.model.id);
            return this;
        },

        select: function () {
            eventBus.trigger("search:resultSelected", this.model);
        }
    });
}(Backbone, Mustache, $, Ohb.eventBus));

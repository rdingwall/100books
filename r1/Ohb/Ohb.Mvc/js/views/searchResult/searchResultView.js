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
        <div class="span7"> \
            <h3 class="searchresult-title">{{ title }}</h3> \
            <p class="searchresult-authors">{{ authors }}</p> \
        </div> \
        <div class="span1">\
            <a id="toggle-previousread-button" class="status-toggle-button btn large"><i class="icon-ok"></i> </a>\
        </div>\
    </div>';

    return Backbone.View.extend({
        className: "book-search-result",

        events: {
            "click .toggle-previousread-button": "toggle",
            "click": "select"
        },

        render: function () {
            this.$el.html(Mustache.to_html(template, this.model.toJSON()));
            this.$el.attr("id", "book-search-result-" + this.model.id);
            return this;
        },

        select: function () {
            this.model.set("selected", true);
        },

        toggle: function () {
            this.model.
        }
    });
}(Backbone, Mustache, $, Ohb.eventBus));

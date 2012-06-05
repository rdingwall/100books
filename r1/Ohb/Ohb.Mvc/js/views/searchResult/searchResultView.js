﻿Ohb.Views.SearchResultView = (function (
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
        <div class="span1 button-container" />\
    </div>';

    var readButtonTemplate = '<a id="toggle-previousread-button" class="status-toggle-button btn large btn-success">\
            <i class="icon-ok icon-white"></i>\
        </a>';

    var unreadButtonTemplate = '<a id="toggle-previousread-button" class="status-toggle-button btn large">\
            <i class="icon-ok"></i>\
        </a>';

    return Backbone.View.extend({
        className: "book-search-result",

        events: {
            "click": "select"
        },

        render: function () {
            var view = this.model.toJSON();
            view.buttonClass = view.hasRead ? "btn-success" : "";
            this.$el.html(Mustache.to_html(template, view));
            this.$el.attr("id", "book-search-result-" + this.model.id);
            return this;
        },

        select: function () {
            this.model.set("selected", true);
        }
    });
}(Backbone, Mustache, $, Ohb.eventBus));

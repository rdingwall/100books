$(function () {
    "use strict";
    Ohb.Views.SearchResultView = (function (
        Mustache,
        ReadableView
    ) {

        var template = '<div class="row"> \
        <div class="span2"> \
            <img src="{{ smallThumbnailUrl }}" alt="{{ title }}" /> \
            </div> \
        <div class="span7"> \
            <h3 class="searchresult-title"><a href="{{ viewUrl }}">{{ title }}</a></h3> \
            <p class="searchresult-authors">{{ authors }}</p> \
        </div> \
        <div class="span1 button-container" />\
    </div>';

        var unreadButtonTemplate = '<a class="status-toggle-button btn large">\
            <i class="icon-plus"></i>\
        </a>';

        var readTemplate = '<i class="icon-ok"></i>';

        return ReadableView.extend({
            className: "book-search-result",

            events: {
                "click a.status-toggle-button": "toggleStatus"
            },

            render: function () {
                this.$el.html(Mustache.to_html(template, this.model.toJSON()));
                this.updateToggleButton();
                return this;
            },

            updateToggleButton: function () {
                if (this.model.get("hasPreviouslyRead")) {
                    this.$el.find(".button-container").html(readTemplate);
                } else {
                    this.$el.find(".button-container").html(unreadButtonTemplate);
                }
            }
        });
    }(
        Mustache,
        Ohb.Views.ReadableView
    ));
});

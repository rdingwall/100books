$(function () {
    "use strict";
    Ohb.Views.SearchResultView = (function (
        Backbone,
        Mustache
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

        return Backbone.View.extend({
            className: "book-search-result",

            events: {
                "click a.status-toggle-button": "toggleStatus"
            },

            initialize: function () {
                this.model.on("change:hasPreviouslyRead",
                    this.onModelStatusChanged, this);
            },

            close: function () {
                this.model.off("change:hasPreviouslyRead",
                    this.onModelStatusChanged, this);
            },

            render: function () {
                this.$el.html(Mustache.to_html(template, this.model.toJSON()));
                this.updateToggleButton();
                return this;
            },

            onModelStatusChanged: function () {
                this.updateToggleButton(this.$el);
            },

            updateToggleButton: function () {
                if (this.model.get("hasPreviouslyRead")) {
                    this.$el.find(".button-container").html(readTemplate);
                } else {
                    this.$el.find(".button-container").html(unreadButtonTemplate);
                }
            },

            toggleStatus: function (event) {
                event.preventDefault();
                this.model.toggleStatus();
            }
        });
    }(Backbone, Mustache));
});

Ohb.Views.NoSearchResultsAvailableView = (function (
    $,
    _,
    Backbone
) {
    "use strict";

    return Backbone.View.extend({
        tagName: "div",
        className: "no-search-results-available",

        render: function () {
            $.get("/templates/searchResult/noSearchResultsAvailable.html", "text",
                _.bind(function (template) {
                    $(this.el).html(template);
                }, this));

            return this;
        }
    });

}($, _, Backbone));


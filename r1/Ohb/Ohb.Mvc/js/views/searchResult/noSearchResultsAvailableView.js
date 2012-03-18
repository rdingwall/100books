Ohb.Views.NoSearchResultsAvailableView = (function (
    $,
    _,
    Backbone
) {
    "use strict";

    var template = '<p class="row">No books found, sorry! Try broadening your search.</p>';

    return Backbone.View.extend({
        className: "searchresult-no-results-available",

        render: function () {
            $(this.el).html(template);
            return this;
        }
    });

}($, _, Backbone));


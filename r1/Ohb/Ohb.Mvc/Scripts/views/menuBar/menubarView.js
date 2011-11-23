define([
    'jquery',
    'backbone',
    'eventbus'
], function ($, Backbone, eventBus) {
    var menuBarView = Backbone.View.extend({
        el: $('#menubar'),

        events: {
            "keyup #menubar-search-input": "searchRequested"
        },

        searchRequested: function (e) {
            if (e.which != 13)
                return;

            var q = $("#menubar-search-input").val();

            if (q.replace(/\s/g, "") == "")
                return;

            eventBus.trigger("searchRequested", q);
        }

    });

    return menuBarView;
});

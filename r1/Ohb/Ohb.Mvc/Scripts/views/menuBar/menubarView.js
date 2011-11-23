define([
    'jquery',
    'backbone',
    'eventBus'
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
            eventBus.trigger("search:requested", q);
        }

    });

    return menuBarView;
});

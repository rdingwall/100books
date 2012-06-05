$(function () {
    "use strict";

    Ohb.menuBarView = (function ($, Backbone, eventBus) {

        var log = $.jog("MenuBarView");

        var MenuBarView = Backbone.View.extend({

            events: {
                "keyup #menubar-search-input": "searchRequested"
            },

            initialize: function () {
                log.info("Initializing MenuBarView...");
                eventBus.on("search:began", this.onSearchBegan);
                eventBus.on("search:completed", this.onSearchCompleted);
            },

            searchRequested: function (e) {
                if (e.which !== 13) {
                    return;
                }

                var q = $("#menubar-search-input").val();

                if (q.replace(/\s/g, "") === "") {
                    return;
                }

                eventBus.trigger("search:requested", q);
            },

            onSearchBegan: function () {
                $("#search-loader-spinner").show();
            },

            onSearchCompleted: function () {
                $("#search-loader-spinner").hide();
            }
        });

        // Instantiated view
        return new MenuBarView({ el: $('#menubar') }).render();

    }($, Backbone, Ohb.eventBus));
});


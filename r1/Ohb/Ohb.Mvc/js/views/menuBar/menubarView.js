﻿$(function () {
    "use strict";

    Ohb.Views.MenuBarView = (function ($, Backbone, eventBus) {

        var log = $.jog("MenuBarView");

        return Backbone.View.extend({
            el: $('#menubar'),

            events: {
                "keyup #menubar-search-input": "searchRequested"
            },

            initialize: function () {
                log.info("initializing menubarview...");
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

    }($, Backbone, Ohb.eventBus));
});


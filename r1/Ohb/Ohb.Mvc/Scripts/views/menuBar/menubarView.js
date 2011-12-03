/*globals define */
define([
    'jquery',
    'backbone',
    'eventbus'
], function ($, Backbone, eventBus) {
    "use strict";

    return Backbone.View.extend({
        el: $('#menubar'),

        events: {
            "keyup #menubar-search-input": "searchRequested"
        },

        initialize: function () {
            console.log("initializing menubarview...");
            eventBus.bind('searchBegan', this.onSearchBegan);
            eventBus.bind('searchCompleted', this.onSearchCompleted);
        },

        searchRequested: function (e) {
            if (e.which !== 13) {
                return;
            }

            var q = $("#menubar-search-input").val();

            if (q.replace(/\s/g, "") === "") {
                return;
            }

            eventBus.trigger("searchRequested", q);
        },

        onSearchBegan: function () {
            $("#search-loader-spinner").show();
        },

        onSearchCompleted: function () {
            $("#search-loader-spinner").hide();
        }
    });
});

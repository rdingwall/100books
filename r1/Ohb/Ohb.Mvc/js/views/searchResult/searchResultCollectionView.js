﻿$(function () {

    "use strict";

    Ohb.Views.SearchResultCollectionView = (function ($, Backbone,
              _, eventBus, SearchResultView, SearchResult,
              NoSearchResultsAvailableView) {

        var log = $.jog("SearchResultCollectionView");

        // todo: generic version http://liquidmedia.ca/blog/2011/02/backbone-js-part-3/
        return Backbone.View.extend({

            el: $('#search-results'),

            initialize: function () {
                this.searchResultViews = [];

                $("html").click("click", $.proxy(this.tryClose, this));

                $(this.el).click(function (e) {
                    e.stopPropagation();
                });

                $("#menubar").click(function (e) {
                    e.stopPropagation();
                });

                eventBus.on("search:resultsArrived", this.onSearchResultsArrived, this);
                eventBus.on("search:returnedNoResults", this.onSearchReturnedNoResults, this);
                eventBus.on("search:resultSelected", this.tryClose, this);
            },

            addResult: function (searchResult) {
                var view = new SearchResultView({
                    model: searchResult
                });

                this.addView(view);
            },

            addView: function (view) {
                this.searchResultViews.push(view);

                if (this._rendered) {
                    $(this.el).append(view.render().el);
                }
            },

            render: function () {
                // We keep track of the rendered state of the view
                this._rendered = true;

                $(this.el).empty();

                var that = this;

                _(this.searchResultViews).each(function (view) {
                    $(that.el).append(view.render().el);
                });

                $(this.el).show();

                return this;
            },

            onSearchResultsArrived: function (results) {
                log.info("showing search results...");
                this.clearResults();

                results.each($.proxy(this.addResult, this));

                this.render();
            },

            onSearchReturnedNoResults: function () {
                log.info("showing 'no results' msg...");
                this.clearResults();
                this.addView(new NoSearchResultsAvailableView());
                this.render();
            },

            clearResults: function () {
                $(this.el).empty();
                this.searchResultViews.length = 0;
            },

            tryClose: function () {

                if (!this._rendered) {
                    return;
                }

                log.info("closing search results...");

                $(this.el).hide();
                this.clearResults();
                this._rendered = false;
            }
        });

    }(
        $,
        Backbone,
        _,
        Ohb.eventBus,
        Ohb.Views.SearchResultView,
        Ohb.Models.SearchResult,
        Ohb.Views.NoSearchResultsAvailableView
    ));
});
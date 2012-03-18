$(function () {

    "use strict";

    Ohb.Views.SearchResultCollectionView = (function ($, Backbone,
              _, eventBus, SearchResultView, SearchResult,
              NoSearchResultsAvailableView, SearchResultCollection) {

        var log = $.jog("SearchResultCollectionView");

        // todo: generic version http://liquidmedia.ca/blog/2011/02/backbone-js-part-3/
        return Backbone.View.extend({

            collection: new SearchResultCollection(),

            initialize: function () {
                this.views = [];

                _.bindAll(this);

                $("html").on("click", this.close);

                $(this.el).click(function (e) {
                    e.stopPropagation();
                });

                $("#menubar").click(function (e) {
                    e.stopPropagation();
                });

                if (this.collection.length === 0) {
                    this.addView(new NoSearchResultsAvailableView());
                } else {
                    this.collection.each(this.addOne);
                }

                eventBus.on("search:began", this.close);
                eventBus.on("search:resultSelected", this.close);
            },

            addOne: function (searchResult) {
                var view = new SearchResultView({
                    model: searchResult
                });

                this.addView(view);
            },

            addView: function (view) {
                this.views.push(view);

                if (this._rendered) {
                    $(this.el).append(view.render().el);
                }
            },

            render: function () {
                $(this.el).empty();

                var that = this;

                _(this.views).each(function (view) {
                    $(that.el).append(view.render().el);
                });

                $(this.el).show();

                return this;
            },

            close: function () {

                log.info("closing search results...");

                $("html").off("click", this.close);
                $(this.el).hide();
                $(this.el).empty();
                this.collection.reset();
                this.views = [];
            }
        });
    }(
        $,
        Backbone,
        _,
        Ohb.eventBus,
        Ohb.Views.SearchResultView,
        Ohb.Models.SearchResult,
        Ohb.Views.NoSearchResultsAvailableView,
        Ohb.Collections.SearchResultCollection
    ));
});
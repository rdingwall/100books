define([
  'backbone',
  'jquery',
  'underscore',
  'views/searchresult/searchresultview',
  'collections/searchresultcollection',
    'eventbus'
], function (Backbone, $, _, SearchResultView, SearchResultCollection, eventBus) {
    // todo: generic version http://liquidmedia.ca/blog/2011/02/backbone-js-part-3/
    var SearchResultCollectionView = Backbone.View.extend({

        searchResultViews: [],

        initialize: function () {
            var that = this;

            eventBus.bind('searchResultsArrived', this.onSearchResultsArrived, this);
        },

        add: function(searchResult) {
            var view = new SearchResultView({
                model: searchResult,
                tagName: 'div'
            });

            this.searchResultViews.push(view);

            if (this._rendered) {
                $(this.el).append(view.render().el);
            }
        },

        remove: function(searchResult) {
            var viewToRemove = _(this.searchResultViews).select(function(cv) {
                return cv.model === model;
            })[0];

            this.searchResultViews = _(this.searchResultViews).without(viewToRemove);

            if (this._rendered) $(viewToRemove.el).remove();
        },

        render : function() {
            // We keep track of the rendered state of the view
            this._rendered = true;

            $(this.el).empty();

            var that = this;

            // Render each Donut View and append them.
            _(this.searchResultViews).each(function(view) {
                $(that.el).append(view.render().el);
            });

            $(this.el).show();

            return this;
        },

        onSearchResultsArrived: function(results) {
            console.log('showing search results...');

            results.each($.proxy(this.add, this));

            this.render();
        }
    });

    return SearchResultCollectionView;
});

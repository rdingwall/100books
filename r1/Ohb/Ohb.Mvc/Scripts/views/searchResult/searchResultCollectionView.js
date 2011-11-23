define([
  'backbone',
  'collections/searchResults'
], function (Backbone, searchResultCollection) {
    var searchResultCollectionView = Backbone.View.extend({
        initialize: function () {
            var that = this;
            this.collection = searchResultCollection;
            this.searchResultViews = [];



            this.collection.each(function (searchResult) {
                that.searchResultViews.push(new SearchResultView({
                    model: searchResult,
                    tagName: 'div'
                }));
            });
        },

        render: function () {
            var that = this;
            // Clear out this element.
            $(this.el).empty();

            // Render each sub-view and append it to the parent view's element.
            _(this.searchResultViews).each(function (srv) {
                $(that.el).append(srv.render().el);
            });
        },

        addSearchResult: function (model) {
            $(this.el).append(model.render().el);
        }
    });

    return searchResultCollectionView;
});

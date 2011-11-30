define([
  'backbone',
  'mustache',
  'jquery',
  'models/searchresult',
  'text!/templates/searchresult/searchresult.html'
], function (Backbone, Mustache, $, SearchResult, searchResultTemplate) {
    var SearchResultView = Backbone.View.extend({
        model: SearchResult,
        tagName: "div",
        className: "book-search-result",

        render: function () {
            $(this.el).empty();
            $(this.el).append(Mustache.to_html(searchResultTemplate, this.model.toJSON()));
            return this;
        }
    });

    return SearchResultView;
});

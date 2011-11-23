define([
  'backbone',
  'Mustache',
  'models/searchResult',
  'text!templates/searchResult/searchResult.html'
], function (Backbone, Mustache, searchResult, searchResultTemplate) {
    var searchResultView = Backbone.View.extend({
        model: searchResult,
        tagName: "div",
        className: "book-search-result",

        render: function () {
            $(this.el).append(Mustache.to_html(searchResultTemplate, this.model.toJSON()));
            return this;
        }
    });

    return searchResultView;
});

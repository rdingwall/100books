define([
  'backbone',
   'models/searchResult'
], function (Backbone, SearchResult) {

    SearchResultCollection = Backbone.Collection.extend({
        model: SearchResult
    });
    
    return SearchResultCollection;

});

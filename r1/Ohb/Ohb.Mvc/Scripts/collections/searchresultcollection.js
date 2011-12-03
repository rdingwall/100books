define([
  'backbone',
   'models/searchresult'
], function (Backbone, SearchResult) {

    SearchResultCollection = Backbone.Collection.extend({
        model: SearchResult
    });
    
    return SearchResultCollection;

});

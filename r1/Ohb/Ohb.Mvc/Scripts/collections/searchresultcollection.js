define([
  'backbone',
   'models/searchResult'
], function (Backbone, SearchResult) {

    var SearchResultCollection = Backbone.Collection.extend({
        model: SearchResult
    });



    return new SearchResultCollection();

});

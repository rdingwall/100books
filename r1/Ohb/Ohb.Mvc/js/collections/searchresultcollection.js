"use strict";

var Ohb = window;

Ohb.SearchResultCollection = (function(Backbone, SearchResult) {

    return Backbone.Collection.extend({
        model: SearchResult
    });

})(Backbone, Ohb.SearchResult);
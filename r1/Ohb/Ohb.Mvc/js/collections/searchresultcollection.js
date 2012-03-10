"use strict";

var Ohb = window;

Ohb.SearchResultCollection = (function($, _, Backbone, SearchResult) {
    var distinct = function(items, keyCallback) {
        var keys = new Array();
        var results = new Array();

        $.each(items, function(i, item) {
            var key = keyCallback(item);
            if ($.inArray(key, keys) === -1) {
                results.push(item);
                keys.push(key);
            }
        });

        return results;
    };

    return Backbone.Collection.extend({
        model: SearchResult,

        url: function() {
            return "https://www.googleapis.com/books/v1/volumes?projection=lite&callback=?";
        },

        parse: function(response) {
            if (!response.items) {
                return;
            }

            var uniqueItems = distinct(response.items, function(result) { return result.id; });

            return _.map(uniqueItems, SearchResult.fromGoogle);
        }
    });

})($, _, Backbone, Ohb.SearchResult);
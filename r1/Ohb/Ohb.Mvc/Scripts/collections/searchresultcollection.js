/*globals define */
define([
    'backbone',
    'models/searchresult'
], function (Backbone, SearchResult) {
    "use strict";
    return Backbone.Collection.extend({
        model: SearchResult
    });
});

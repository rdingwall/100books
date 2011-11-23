/*globals $, BackBone, searchResult, searchResultCollection,
searchResultsListView, eventBus, define */
define([
    'jquery',
    'backbone',
    'models/searchresult',
    'collections/searchresultcollection',
    'eventBus'
], function (
    $,
    Backbone,
    SearchResult,
    SearchResultCollection,
    eventBus
) {
    var AppRouter = Backbone.Router.extend({

        routes: {
            "search/:q": "search"
        },

        initialize: function () {
            eventBus.bind('search:requested', 'search');
        },

        search: function (q) {
            $.getJSON("https://www.googleapis.com/books/v1/volumes?callback=?",
                {
                    q: decodeURIComponent(q),
                    projection: "lite"
                },
                function (data) {

                    var results = new SearchResultCollection();

                    $(data.items).each(function () {
                        results.add(SearchResult.fromGoogle($(this)[0]));
                    });

                    eventBus.trigger("search:resultsArrived", results);
                });
        }
    });

    var initialize = function () {
        var router = new AppRouter();
        Backbone.history.start();
        return router;
    };
    return {
        initialize: initialize
    };
});
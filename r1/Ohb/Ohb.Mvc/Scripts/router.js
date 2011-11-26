/*globals $, BackBone, searchResult, searchResultCollection,
searchResultsListView, eventBus, define */
define([
    'jquery',
    'backbone',
    'models/searchresult',
    'collections/searchresultcollection',
    'eventbus',
    'lib/bootstrap/bootstrap-modal.js'
], function (
    $,
    Backbone,
    SearchResult,
    SearchResultCollection,
    eventBus
) {
    var instance = null;

    AppRouter = Backbone.Router.extend({

        routes: {
            "search/:q": "search"
        },

        initialize: function () {
            console.log("initializing router...");
            eventBus.bind('searchRequested', this.search);
            eventBus.bind('searchFailed', this.onSearchFailed);
        },

        search: function (q) {
            console.log("Searching for " + q + "...");

            eventBus.trigger("searchBegan", q);

            $.getJSON("https://www.googleapis.com/books/v1/volumes?callback=?",
                  {
                      q: decodeURIComponent(q),
                      projection: "lite"
                  },
                  function (json) {
                      try
                      {
                          var results = new SearchResultCollection();

                          if (!json.items)
                          {
                              eventBus.trigger("searchReturnedNoResults");
                              return;
                          }

                          $(json.items).each(function () {
                              results.add(SearchResult.fromGoogle($(this)[0]));
                          });

                          eventBus.trigger("searchResultsArrived", results);
                      }
                      catch (e)
                      {
                          eventBus.trigger("searchFailed");
                      }
                      eventBus.trigger("searchCompleted");
                  }).error(function(jqXHR, textStatus, errorThrown) {
                      console.log("Search error: " + textStatus);
                      eventBus.trigger("searchFailed");
                  });
        },

        onSearchFailed: function() {
            console.log('showing search failed modal...');
            $("#search-failed-modal").modal({ keyboard: true });
        }
    });

    AppRouter.getInstance = function(){
        // summary:
        //      Gets an instance of the singleton. It is better to use
        if(instance === null){
            instance = new AppRouter();
        }
        return instance;
    }

    return AppRouter.getInstance();
});
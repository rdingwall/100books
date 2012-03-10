$(function() {

    "use strict";

    var Ohb = window;

    Ohb.App = (function (
        $,
        _,
        Backbone,
        router,
        eventBus,
        MenuBarView,
        SearchResultCollectionView
    ) {

        var menuBarView = new MenuBarView(),
            searchResultCollectionView = new SearchResultCollectionView();

        return {
            initialize: function () {

            }
        };
    })($, _, Backbone, Ohb.Router, Ohb.EventBus, Ohb.MenuBarView, Ohb.SearchResultCollectionView);

});
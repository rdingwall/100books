﻿"use strict";

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
            // Pass in our Router module and call it's initialize function
            router.initialize();
        }
    };
})($, _, Backbone, Ohb.Router, Ohb.EventBus, Ohb.MenuBarView, Ohb.SearchResultCollectionView);

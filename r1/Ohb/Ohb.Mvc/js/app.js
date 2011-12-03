﻿/*globals require, define, Router */
define([
    'jquery',
    'underscore',
    'backbone',
    'router',
    'eventbus',
    'views/menubar/menubarview',
    'views/searchresult/searchresultcollectionview'
], function (
    $,
    _,
    Backbone,
    AppRouter,
    eventBus,
    MenuBarView,
    SearchResultCollectionView
) {
    "use strict";

    var menuBarView = new MenuBarView(),
        searchResultCollectionView = new SearchResultCollectionView();

    return {
        initialize: function () {
            // Pass in our Router module and call it's initialize function
            AppRouter.initialize();
        }
    };
});

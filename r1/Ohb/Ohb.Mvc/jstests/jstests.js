/*global window: false, document: false, $: false, log: false, bleep: false,
 QUnit: false,
 test: false,
 asyncTest: false,
 expect: false,
 module: false,
 ok: false,
 equal: false,
 notEqual: false,
 deepEqual: false,
 notDeepEqual: false,
 strictEqual: false,
 notStrictEqual: false,
 raises: false,
 start: false,
 stop: false
 */

$(function () {
    "use strict";

    (function (
        app,
        router,
        eventBus,
        $,
        _,
        Backbone,
        SearchResultView,
        SearchResult,
        Book,
        SearchResultCollection,
        SearchResultCollectionView,
        BookDetailsView,
        mainRegion,
        PreviousRead,
        PreviousReadView,
        PreviousReadCollection,
        PreviousReadCollectionView,
        Profile,
        ProfileCardView,
        CompositeProfileView
    ) {

        QUnit.config.testTimeout = 2000;

        var log = $.jog("Tests");

        log.info("document loaded, running tests");




















    }(
        Ohb.app,
        Ohb.Router,
        Ohb.eventBus,
        $,
        _,
        Backbone,
        Ohb.Views.SearchResultView,
        Ohb.Models.SearchResult,
        Ohb.Models.Book,
        Ohb.Collections.SearchResultCollection,
        Ohb.Views.SearchResultCollectionView,
        Ohb.Views.BookDetailsView,
        Ohb.mainRegion,
        Ohb.Models.PreviousRead,
        Ohb.Views.PreviousReadView,
        Ohb.Collections.PreviousReadCollection,
        Ohb.Views.PreviousReadCollectionView,
        Ohb.Models.Profile,
        Ohb.Views.ProfileCardView,
        Ohb.Views.CompositeProfileView
    ));
});
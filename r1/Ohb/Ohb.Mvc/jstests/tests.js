﻿/*globals require, define, _ */
/*jslint white: false, onevar: true, undef: true, nomen: false, eqeqeq: true, plusplus: true, bitwise: true, regexp: true, newcap: true, immed: true */
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

require.config({
    baseUrl: "../js",
    paths: {
        underscore: 'lib/underscore/underscore',
        backbone: 'lib/backbone/backbone',
        qunit: 'lib/qunit/qunit',
        jsmockito: 'lib/jsmockito/jsmockito',
        mustache: 'lib/mustache/mustache',
        bootstrapModal: 'lib/bootstrap/bootstrap-modal'
    }
});

require([
    'lib/requires/order!main',
    'router',
    'eventbus',
    'jquery',
    'underscore',
    'backbone',
    'views/menubar/menubarview',
    'views/searchresult/searchresultview',
    'models/searchresult',
    'collections/searchresultcollection',
    'views/searchresult/searchresultcollectionview',
    'qunit',
    'jsmockito'
], function (
    main,
    router,
    eventBus,
    $,
    _,
    Backbone,
    MenuBarView,
    SearchResultView,
    SearchResult,
    SearchResultCollection,
    SearchResultCollectionView
) {
    "use strict";
    console.log("hiya");

    $(function () {

        module("When registering modules");

        test("It should inject the event bus", function () {
            ok(eventBus);
        });

        test("The event bus should be extended with Backbone events", function () {
            ok(eventBus.bind);
        });

        test("It should inject jQuery", function () {
            ok($);
        });

        test("It should inject underscore", function () {
            ok(_);
        });

        test("It should inject backbone", function () {
            ok(Backbone);
        });

        module("When mapping google book search results");

        var assertBookMapping = function (googleBookId, title, authors, smallThumbnailUrl) {
            $.getJSON('https://www.googleapis.com/books/v1/volumes/' + googleBookId + '?callback=?',
                function (data) {
                    var searchResult = SearchResult.fromGoogle(data);

                    equal(searchResult.get('title'), title);
                    equal(searchResult.get('authors'), authors);
                    equal(searchResult.get('smallThumbnailUrl'), smallThumbnailUrl);
                })
                .error(function () {
                    throw "failed";
                });
        };

        asyncTest("It should include the book's subtitle", 3, function () {
            assertBookMapping('abYKXvCwEToC', "Harry Potter: the story of a global business phenomenon", "Susan Gunelius", "http://bks6.books.google.co.uk/books?id=abYKXvCwEToC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");

            setTimeout(start, 1000);
        });

        asyncTest("It should list multiple authors", 3, function () {
            assertBookMapping('GGpXN9SMELMC', "Head First design patterns", "Eric Freeman, Elisabeth Freeman, Kathy Sierra, Bert Bates", "http://bks9.books.google.co.uk/books?id=GGpXN9SMELMC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");

            setTimeout(start, 1000);
        });

        asyncTest("It should handle books without thumbnails", 3, function () {
            assertBookMapping('_XDFAAAACAAJ', "Design Patterns: Elements of Reusable Object-Oriented Software with Applying Uml and Patterns:An Introduction to Object-Oriented Analysis and Design and the Unified Process", "Gamma, Larman", null);

            setTimeout(start, 1000);
        });

        asyncTest("It should handle books with no authors", 3, function () {
            assertBookMapping('3zgFSQAACAAJ', "The Girl With the Dragon Tattoo", null, "http://bks1.books.google.co.uk/books?id=3zgFSQAACAAJ&printsec=frontcover&img=1&zoom=5&source=gbs_api");

            setTimeout(start, 1000);
        });

        module('when pressing enter in the search box');

        test('It should raise the searchRequested event', function () {
            eventBus.reset();

            var view = new MenuBarView({ el: $("#qunit-fixture") }), expected = 'test search';

            eventBus.bind('searchRequested', function (q) {
                equal(q, expected);
            });

            $("#menubar-search-input").val(expected);
            var e = $.Event('keyup');
            e.which = 13;
            $("#menubar-search-input").trigger(e);
        });

        test('It shouldn\'t raise any event if the search box is empty', function () {
            eventBus.reset();
            var view = new MenuBarView({ el: $("#qunit-fixture") });

            $("#menubar-search-input").val('');
            var e = $.Event('keyup');
            e.which = 13;

            eventBus.bind('searchRequested', function (q) {
                ok(false, "should not have been raised!");
            });

            $("#menubar-search-input").trigger(e);

        });

        module("When a searchRequested event is raised");

        asyncTest('It should perform a search and raise a resultsArrived event with the results', 2, function () {
            eventBus.reset();
            router.initialize();

            var wasRaised = false;
            eventBus.bind('searchResultsArrived', function (results) {
                wasRaised = true;
                ok(results);
                equal(results.length, 10);
            });

            eventBus.bind('searchFailed', function (results) {
                ok(false, "search failed!");
            });

            eventBus.trigger("searchRequested", "harry potter");

            setTimeout(start, 2000);
        });

        asyncTest('It should raise a searchBegan event', 2, function () {
            eventBus.reset();
            router.initialize();

            eventBus.bind('searchBegan', function (q) {
                ok(true);
                equal(q, "harry potter");
            });

            eventBus.trigger("searchRequested", "harry potter");

            setTimeout(start, 1000);
        });

        asyncTest('It should raise a searchCompleted event', 1, function () {
            eventBus.reset();
            router.initialize();

            eventBus.bind('searchCompleted', function (q) {
                ok(true);
            });

            eventBus.trigger("searchRequested", "harry potter");

            setTimeout(start, 2000);
        });

        asyncTest('When the test fails, it should raise a searchFailed event', 1, function () {
            eventBus.reset();
            router.initialize();

            eventBus.bind('searchFailed', function (results) {
                ok(true);
            });

            eventBus.bind('searchResultsArrived', function (results) {
                ok(false, "should not have been raised!");
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");

            setTimeout(start, 2000);
        });

        asyncTest('When there are no results, it should raise a no results event', 1, function () {
            eventBus.reset();
            router.initialize();

            eventBus.bind('searchFailed', function (results) {
                ok(false, "searchFailed was raised");
            });

            eventBus.bind('searchReturnedNoResults', function (results) {
                ok(true);
            });

            eventBus.bind('searchResultsArrived', function (results) {
                equal(results.length, 0);
                ok(false, "searchResultsArrived was raised");
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");

            setTimeout(start, 2000);
        });

        module("when a search fails");

        test('It should render the error modal', 2, function () {
            eventBus.reset();
            router.initialize();

            ok(!($("#search-failed-modal").is(":visible")), "should be hidden to start with");

            eventBus.trigger("searchFailed");

            ok($("#search-failed-modal").is(":visible"));

            $("#search-failed-modal").hide();
        });

        module("when a search begins");

        test("It should show the ajax loader gif", 2, function () {
            eventBus.reset();
            var view = new MenuBarView({ el: $("#qunit-fixture") });
            view.initialize();

            ok(!($("#search-loader-spinner").is(":visible")), "should be hidden to start with");

            eventBus.trigger("searchBegan");

            ok($("#search-loader-spinner").is(":visible"));

            $("#search-loader-spinner").hide();
        });

        module("when a search completes");

        test("The ajax loader gif should dissappear", 2, function () {
            eventBus.reset();
            var view = new MenuBarView({ el: $("#qunit-fixture") });
            view.initialize();

            $("#search-loader-spinner").show();

            ok(($("#search-loader-spinner").is(":visible")), "should be visible to start with");

            eventBus.trigger("searchCompleted");

            ok(!($("#search-loader-spinner").is(":visible")));
        });

        module("when search results become available");

        test("they should be rendered", 3, function () {
            eventBus.reset();
            var view = new SearchResultCollectionView({ el: $("#test-search-results") });

            ok(!($("#test-search-results").is(":visible")), "should be hidden to start with");

            var results = new SearchResultCollection();
            results.add(new SearchResult({ title: "test book" }));
            results.add(new SearchResult({ title: "test book 2" }));

            eventBus.trigger("searchResultsArrived", results);

            ok($("#test-search-results").is(":visible"), "should become visible");
            equal($("#test-search-results").children().length, 2);
        });

        module("when rendering a single search result");

        test("it should be rendered", 2, function () {

            var el = $("#test-search-result"), view = new SearchResultView({
                el: el[0],
                model: new SearchResult({
                    title: "Harry Potter",
                    authors: "JK Rowling",
                    smallThumbnailUrl: "http://2.gravatar.com/avatar/87acbe2fc2f40edf8fa5a816515bff9f",
                    id: "42"
                })
            });
            view.render();

            equal(el.find(".searchresult-title").text(), "Harry Potter");
            equal(el.find("p.searchresult-authors").text(), "JK Rowling");
        });

        module("when the search result box is open");

        test("clicking anywhere outside should hide it", 4, function () {
            var el = $("#test-search-results");
            ok(!(el.is(":visible")), "should be hidden to start with");

            var view = new SearchResultCollectionView({ el: el[0] });
            view.render();

            ok(el.is(":visible"), "should become visible");

            $("body").trigger("click");

            ok(!(el.is(":visible")), "should be hidden again");

            equal(view.searchResultViews.length, 0, "should clear the items");
        });

        module("when there are no search results available");

        test("It should display a no search results message", 3, function () {
            eventBus.reset();
            var view = new SearchResultCollectionView({ el: $("#test-search-results") });

            ok(!($("#test-search-results").is(":visible")), "should be hidden to start with");

            eventBus.trigger("searchReturnedNoResults");

            ok($("#test-search-results").is(":visible"), "should become visible");
            ok($(".searchresult-no-results-available").is(":visible"));
        });
    });
});
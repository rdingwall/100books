/*globals require, define, _ */
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
    'jquery',
    'models/searchresult',
    'eventbus',
    'router',
    'qunit',
    'lib/jog'
], function (
    main,
    $,
    SearchResult,
    eventBus,
    router
) {
    "use strict";

    var log = $.jog("GoogleTests");

    log.info("hiya");

    $(function () {

        module("When mapping google book search results");

        var assertBookMapping = function (googleBookId, title, authors, smallThumbnailUrl) {
            $.getJSON('https://www.googleapis.com/books/v1/volumes/' + googleBookId + '?&key=AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs&callback=?',
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

        asyncTest('When the test fails, it should raise a searchCompleted event', 1, function () {
            eventBus.reset();
            router.initialize();

            eventBus.bind('searchCompleted', function (results) {
                ok(true);
            });

            eventBus.bind('searchResultsArrived', function (results) {
                ok(false, "should not have been raised!");
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");

            setTimeout(start, 2000);
        });

        module("When requesting a search and there was no results");

        asyncTest('It should raise a no results event', 1, function () {
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

        asyncTest('It should raise a searchCompleted event', 1, function () {
            eventBus.reset();
            router.initialize();

            eventBus.bind('searchFailed', function (results) {
                ok(false, "searchFailed was raised");
            });

            eventBus.bind('searchCompleted', function (results) {
                ok(true);
            });

            eventBus.bind('searchResultsArrived', function (results) {
                equal(results.length, 0);
                ok(false, "searchResultsArrived was raised");
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");

            setTimeout(start, 2000);
        });
    });
});
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
        $,
        app,
        SearchResult,
        eventBus,
        router,
        SearchResultCollection
    ) {

        var log = $.jog("GoogleTests");

        log.info("hiya");

        QUnit.config.testTimeout = 2000;

        module("When fetching a search result collection");

        asyncTest("It should perform a search and raise a search:resultsArrived event with the results", 4, function () {

            var results = new SearchResultCollection();

            results.fetch({ data: { q: "harry potter" }, success: function (collection) {
                equal(collection.length, 10);
                var result = collection.get("0W0DRgAACAAJ");
                equal(result.get("authors"), "J. K. Rowling");
                equal(result.get("title"), "Harry Potter and the Deathly Hallows");
                equal(result.get("smallThumbnailUrl"), "http://bks2.books.google.co.uk/books?id=0W0DRgAACAAJ&printsec=frontcover&img=1&zoom=5&source=gbs_api");
                start();
            }});
        });

        module("When mapping google book search results");

        var assertBookMapping = function (googleBookId, title, authors, smallThumbnailUrl) {
            $.getJSON("https://www.googleapis.com/books/v1/volumes/" + googleBookId + "?&key=AIzaSyAwesvnG7yP5wCqiNv21l8g7mo-ehkcVJs&callback=?",
                function (data) {
                    var searchResult = SearchResult.fromGoogle(data);

                    equal(searchResult.get("title"), title);
                    equal(searchResult.get("authors"), authors);
                    equal(searchResult.get("smallThumbnailUrl"), smallThumbnailUrl);
                    start();
                })
                .error(function () {
                    ok(false, "failed");
                    start();
                });
        };

        asyncTest("It should include the book's subtitle", 3, function () {
            assertBookMapping("abYKXvCwEToC", "Harry Potter: the story of a global business phenomenon", "Susan Gunelius", "http://bks6.books.google.co.uk/books?id=abYKXvCwEToC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");
        });

        asyncTest("It should list multiple authors", 3, function () {
            assertBookMapping("GGpXN9SMELMC", "Head First design patterns", "Eric Freeman, Elisabeth Freeman, Kathy Sierra, Bert Bates", "http://bks9.books.google.co.uk/books?id=GGpXN9SMELMC&printsec=frontcover&img=1&zoom=5&edge=curl&source=gbs_api");
        });

        asyncTest("It should handle books without thumbnails", 3, function () {
            assertBookMapping("DAAAAAAACAAJ", "Harry Potter: 5 Years of Magic, Adventure, and Mystery at Hogwarts", "J. K. Rowling", "img/search-result-no-cover.png");
        });

        asyncTest("It should handle books with no authors", 3, function () {
            assertBookMapping("3zgFSQAACAAJ", "The Girl With the Dragon Tattoo", null, "http://bks1.books.google.co.uk/books?id=3zgFSQAACAAJ&printsec=frontcover&img=1&zoom=5&source=gbs_api");
        });


        module("When a searchRequested event is raised");

        asyncTest("It should perform a search and raise a search:resultsArrived event with the results", 2, function () {
            eventBus.reset();
            app.initialize();

            var wasRaised = false;
            eventBus.on("search:resultsArrived", function (results) {
                wasRaised = true;
                ok(results);
                equal(results.length, 10);
                start();
            });

            eventBus.on("search:failed", function (results) {
                ok(false, "search failed!");
                start();
            });

            eventBus.trigger("search:requested", "harry potter");
        });

        asyncTest("It should raise a search:began event", 2, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:began", function (q) {
                ok(true);
                equal(q, "harry potter");
                start();
            });

            eventBus.trigger("search:requested", "harry potter");
        });

        asyncTest("It should raise a search:completed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:completed", function (q) {
                ok(true);
                start();
            });

            eventBus.trigger("search:requested", "harry potter");
        });

        module("When the book search fails");

        asyncTest("It should raise a search:failed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:failed", function (results) {
                ok(true);
                start();
            });

            eventBus.on("search:resultsArrived", function (results) {
                ok(false, "should not have been raised (but it's ok, not sure how to fake a test right now)");
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

        asyncTest("It should raise a search:completed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:completed", function (results) {
                ok(true);
                start();
            });

            eventBus.on("search:resultsArrived", function (results) {
                ok(false, "should not have been raised!");
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

        module("When requesting a search and there was no results");

        asyncTest("It should raise a no results event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:failed", function (results) {
                ok(false, "searchFailed was raised");
                start();
            });

            eventBus.on("search:returnedNoResults", function (results) {
                ok(true);
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

        asyncTest("It should raise a search:completed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:failed", function (results) {
                ok(false, "searchFailed was raised");
                start();
            });

            eventBus.on("search:completed", function (results) {
                ok(true);
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

    }(
        $,
        Ohb.app,
        Ohb.Models.SearchResult,
        Ohb.eventBus,
        Ohb.Router,
        Ohb.Collections.SearchResultCollection
    ));
});
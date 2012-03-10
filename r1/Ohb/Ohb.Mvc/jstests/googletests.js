$(function() {
    "use strict";

    var Ohb = window;

    (function (
        $,
        app,
        SearchResult,
        eventBus,
        router,
        SearchResultCollection
    ){

        var log = $.jog("GoogleTests");

        log.info("hiya");

        QUnit.config.testTimeout = 2000;

        module("When fetching a search result collection");

        asyncTest("It should perform a search and raise a resultsArrived event with the results", 4, function () {

            var results = new SearchResultCollection();

            results.fetch({ data: { q: "harry potter" }, success: function(collection) {
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
                    throw "failed";
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
            assertBookMapping("DAAAAAAACAAJ", "Harry Potter: 5 Years of Magic, Adventure, and Mystery at Hogwarts", "J. K. Rowling", null);
        });

        asyncTest("It should handle books with no authors", 3, function () {
            assertBookMapping("3zgFSQAACAAJ", "The Girl With the Dragon Tattoo", null, "http://bks1.books.google.co.uk/books?id=3zgFSQAACAAJ&printsec=frontcover&img=1&zoom=5&source=gbs_api");
        });


        module("When a searchRequested event is raised");

        asyncTest("It should perform a search and raise a resultsArrived event with the results", 2, function () {
            eventBus.reset();
            app.initialize();

            var wasRaised = false;
            eventBus.on("searchResultsArrived", function (results) {
                wasRaised = true;
                ok(results);
                equal(results.length, 10);
                start();
            });

            eventBus.on("searchFailed", function (results) {
                ok(false, "search failed!");
                start();
            });

            eventBus.trigger("searchRequested", "harry potter");
        });

        asyncTest("It should raise a searchBegan event", 2, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("searchBegan", function (q) {
                ok(true);
                equal(q, "harry potter");
                start();
            });

            eventBus.trigger("searchRequested", "harry potter");
        });

        asyncTest("It should raise a searchCompleted event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("searchCompleted", function (q) {
                ok(true);
                start();
            });

            eventBus.trigger("searchRequested", "harry potter");
        });

        asyncTest("When the test fails, it should raise a searchFailed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("searchFailed", function (results) {
                ok(true);
                start();
            });

            eventBus.on("searchResultsArrived", function (results) {
                ok(false, "should not have been raised!");
                start();
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");
        });

        asyncTest("When the test fails, it should raise a searchCompleted event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("searchCompleted", function (results) {
                ok(true);
                start();
            });

            eventBus.on("searchResultsArrived", function (results) {
                ok(false, "should not have been raised!");
                start();
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");
        });

        module("When requesting a search and there was no results");

        asyncTest("It should raise a no results event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("searchFailed", function (results) {
                ok(false, "searchFailed was raised");
                start();
            });

            eventBus.on("searchReturnedNoResults", function (results) {
                ok(true);
                start();
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");
        });

        asyncTest("It should raise a searchCompleted event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("searchFailed", function (results) {
                ok(false, "searchFailed was raised");
                start();
            });

            eventBus.on("searchCompleted", function (results) {
                ok(true);
                start();
            });

            eventBus.trigger("searchRequested", "3894h9f893jhf934jf92ht8");
        });




    })($, Ohb.app, Ohb.SearchResult, Ohb.eventBus, Ohb.Router, Ohb.SearchResultCollection);
});
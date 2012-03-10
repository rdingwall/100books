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
            assertBookMapping('DAAAAAAACAAJ', "Harry Potter: 5 Years of Magic, Adventure, and Mystery at Hogwarts", "J. K. Rowling", null);

            setTimeout(start, 1000);
        });

        asyncTest("It should handle books with no authors", 3, function () {
            assertBookMapping('3zgFSQAACAAJ', "The Girl With the Dragon Tattoo", null, "http://bks1.books.google.co.uk/books?id=3zgFSQAACAAJ&printsec=frontcover&img=1&zoom=5&source=gbs_api");

            setTimeout(start, 1000);
        });


        module("When a searchRequested event is raised");

        asyncTest('It should perform a search and raise a resultsArrived event with the results', 2, function () {
            eventBus.reset();
            app.initialize();

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
            app.initialize();

            eventBus.bind('searchBegan', function (q) {
                ok(true);
                equal(q, "harry potter");
            });

            eventBus.trigger("searchRequested", "harry potter");

            setTimeout(start, 1000);
        });

        asyncTest('It should raise a searchCompleted event', 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.bind('searchCompleted', function (q) {
                ok(true);
            });

            eventBus.trigger("searchRequested", "harry potter");

            setTimeout(start, 2000);
        });

        asyncTest('When the test fails, it should raise a searchFailed event', 1, function () {
            eventBus.reset();
            app.initialize();

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
            app.initialize();

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
            app.initialize();

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
            app.initialize();

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




    })($, Ohb.App, Ohb.SearchResult, Ohb.EventBus, Ohb.Router, Ohb.SearchResultCollection);
});
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

    var Ohb = window;

    (function (
        app,
        router,
        eventBus,
        $,
        _,
        Backbone,
        MenuBarView,
        SearchResultView,
        SearchResult,
        Book,
        SearchResultCollection,
        SearchResultCollectionView,
        BookDetailsView
    ) {

        QUnit.config.testTimeout = 2000;

        var log = $.jog("Tests");

        log.info("document loaded, running tests");

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

        module("When pressing enter in the search box");

        test("It should raise the search:requested event", function () {
            eventBus.reset();

            var view = new MenuBarView({ el: $("#qunit-fixture") }), expected = "test search";

            eventBus.on("search:requested", function (q) {
                equal(q, expected);
            });

            $("#menubar-search-input").val(expected);
            var e = $.Event("keyup");
            e.which = 13;
            $("#menubar-search-input").trigger(e);
        });

        test("It should not raise search:requested if the search box is empty", function () {
            eventBus.reset();
            var view = new MenuBarView({ el: $("#qunit-fixture") });

            $("#menubar-search-input").val("");
            var e = $.Event("keyup");
            e.which = 13;

            eventBus.on("search:requested", function (q) {
                ok(false, "should not have been raised!");
            });

            $("#menubar-search-input").trigger(e);
        });

        module("When a search fails");

        test("It should render the error modal", 2, function () {
            eventBus.reset();
            app.initialize();

            ok(!($("#search-failed-modal").is(":visible")), "should be hidden to start with");

            eventBus.trigger("search:failed");

            ok($("#search-failed-modal").is(":visible"));

            $("#search-failed-modal").hide();
        });

        module("When a search begins");

        test("It should show the ajax loader gif", 2, function () {
            eventBus.reset();
            var view = new MenuBarView({ el: $("#qunit-fixture") });
            view.initialize();

            ok(!($("#search-loader-spinner").is(":visible")), "should be hidden to start with");

            eventBus.trigger("search:began");

            ok($("#search-loader-spinner").is(":visible"));

            $("#search-loader-spinner").hide();
        });

        module("When a search completes");

        test("The ajax loader gif should dissappear", 2, function () {
            eventBus.reset();
            var view = new MenuBarView({ el: $("#qunit-fixture") });
            view.initialize();

            $("#search-loader-spinner").show();

            ok(($("#search-loader-spinner").is(":visible")), "should be visible to start with");

            eventBus.trigger("search:completed");

            ok(!($("#search-loader-spinner").is(":visible")));
        });

        module("When search results become available");

        test("They should be rendered", 3, function () {
            eventBus.reset();
            var view = new SearchResultCollectionView({ el: $("#test-search-results") });

            ok(!($("#test-search-results").is(":visible")), "should be hidden to start with");

            var results = new SearchResultCollection();
            results.add(new SearchResult({ title: "test book" }));
            results.add(new SearchResult({ title: "test book 2" }));

            eventBus.trigger("search:resultsArrived", results);

            ok($("#test-search-results").is(":visible"), "should become visible");
            equal($("#test-search-results").children().length, 2);
        });

        module("When rendering a single search result");

        asyncTest("It should be rendered", 2, function () {

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

            setTimeout(function () {
                equal(el.find(".searchresult-title").text(), "Harry Potter");
                equal(el.find("p.searchresult-authors").text(), "JK Rowling");
                start();
            }, 500);
        });

        module("When the search result box is open");

        test("Clicking anywhere outside should hide it", 4, function () {
            var el = $("#test-search-results");
            ok(!(el.is(":visible")), "should be hidden to start with");

            var view = new SearchResultCollectionView({ el: el[0] });
            view.render();

            ok(el.is(":visible"), "should become visible");

            $("body").trigger("click");

            ok(!(el.is(":visible")), "should be hidden again");

            equal(view.searchResultViews.length, 0, "should clear the items");
        });

        test("Clicking inside the search results should not hide the results", 3, function () {
            var el = $("#test-search-results");
            ok(!(el.is(":visible")), "should be hidden to start with");

            var view = new SearchResultCollectionView({ el: el[0] });
            view.render();

            ok(el.is(":visible"), "should become visible");

            $("#test-search-results").trigger("click");

            ok(el.is(":visible"), "should stay visible");
        });

        test("Clicking in the menu bar should not hide the results", 3, function () {
            var el = $("#test-search-results");
            ok(!(el.is(":visible")), "should be hidden to start with");

            var view = new SearchResultCollectionView({ el: el[0] });
            view.render();

            ok(el.is(":visible"), "should become visible");

            $("#menubar").trigger("click");

            ok(el.is(":visible"), "should stay visible");
        });

        test("New results should replace old ones", 4, function () {
            eventBus.reset();
            var el = $("#test-search-results");
            el.empty();
            ok(!(el.is(":visible")), "should be hidden to start with");

            var view = new SearchResultCollectionView({ el: el[0] });
            view.addResult(new SearchResult({ title: "test book" }));
            view.addResult(new SearchResult({ title: "test book 2" }));
            view.render();

            equal($("#test-search-results").children().length, 2, "started with 2 results");

            var results = new SearchResultCollection();
            results.add(new SearchResult({ title: "test book" }));
            results.add(new SearchResult({ title: "test book 2" }));
            results.add(new SearchResult({ title: "test book 3" }));

            eventBus.trigger("search:resultsArrived", results);

            ok($("#test-search-results").is(":visible"), "should become visible");
            equal($("#test-search-results").children().length, 3, "should replace existing results");
        });

        module("When there are no search results available");

        asyncTest("It should display a no search results message", 3, function () {
            eventBus.reset();
            var view = new SearchResultCollectionView({ el: $("#test-search-results") });

            ok(!($("#test-search-results").is(":visible")), "should be hidden to start with");

            eventBus.trigger("search:returnedNoResults");

            setTimeout(function () {
                ok($("#test-search-results").is(":visible"), "should become visible");
                ok($(".searchresult-no-results-available").is(":visible"));
                start();
            }, 500);
        });

        asyncTest("It should replace any previous results", 6, function () {
            log.info("starting the test");
            eventBus.reset();
            var el = $("#test-search-results");
            el.empty();
            equal(el.children().length, 0, "should be empty to start with");
            ok(!(el.is(":visible")), "should be hidden to start with");

            var view = new SearchResultCollectionView({ el: el[0] });
            view.addResult(new SearchResult({ title: "test book" }));
            view.addResult(new SearchResult({ title: "test book 2" }));
            view.render();

            equal(el.children().length, 2, "started with 2 results");

            eventBus.trigger("search:returnedNoResults");

            setTimeout(function () {
                ok(el.is(":visible"), "should become visible");
                ok($(".searchresult-no-results-available").is(":visible"));
                equal(el.children().length, 1, "should replace existing results");
                start();
            }, 500);
        });

        module("When clicking on a search result");

        test("It should raise a search:resultSelected event", 1, function () {
            eventBus.reset();

            var el = $("#test-search-result");
            var model = new SearchResult({ title: "foo" });
            var view = new SearchResultView({ el: el, model: model });

            eventBus.on("search:resultSelected", function (sr) {
                equal(sr, model);
            });

            el.trigger("click");
        });

        module("When a search:resultSelected event is raised");

        test("It should navigate to the new route", 1, function () {
            eventBus.reset();
            app.initialize();

            var model = new SearchResult({
                id: "foo",
                title: "Harry Potter's amazing #(*@(#)(# adventure$ 2008"
            });

            eventBus.trigger("search:resultSelected", model);

            equal(window.location.hash, "#books/foo/harry-potters-amazing-adventure-2008");
        });

        test("It should close the search results", 4, function () {
            eventBus.reset();

            var el = $("#test-search-results");
            ok(!(el.is(":visible")), "should be hidden to start with");

            var collectionView = new SearchResultCollectionView({ el: el[0] });
            collectionView.render();

            ok(el.is(":visible"), "should become visible");

            eventBus.trigger("search:resultSelected", new SearchResult({}));

            ok(!(el.is(":visible")), "should be hidden");

            equal(collectionView.searchResultViews.length, 0, "should clear the items");
        });

        // This one fails when run with the other tests for some reason
        asyncTest("It should raise a book:requested event (RUN SEPARATELY)", 1, function () {
            eventBus.reset();
            app.initialize();
            var model = new SearchResult({ id: "foo" });

            eventBus.on("book:requested", function (id) {
                equal(id, model.id);
            });

            eventBus.trigger("search:resultSelected", model);

            setTimeout(start, 1000);
        });

        module("When a Search result has no thumbnail image");

        test("It should use the default placeholder thumbnail image", 1, function () {
            var model = { volumeInfo: { } };
            var searchResult = SearchResult.fromGoogle(model);
            equal(searchResult.get("smallThumbnailUrl"), "img/search-result-no-cover.png");
        });

        module("When a Book model has thumbnails");

        test("It should use the real thumbnail image in search results", 1, function () {
            var model = new Book({ smallThumbnailUrl: "test" });
            equal(model.getSearchResultThumbnail(), "test");
        });

        test("It should use the real thumbnail image on the book page", 1, function () {
            var model = new Book({ thumbnailUrl: "test" });
            equal(model.getBookThumbnail(), "test");
        });

        module("When clicking the button to toggle a book's status");

        asyncTest("It should change the book's hasPreviouslyRead attr to true (RUN SEPARATELY)", 1, function () {
            var model  = new Book({ thumbnailUrl: "/img/book-no-cover.png" });
            app.bookDetailsView.model = model;
            app.bookDetailsView.render();

            setTimeout(function () {
                $(".status-toggle-button").trigger("click");
                ok(model.get("hasPreviouslyRead"));
                start();
            }, 1000);
        });

        module("When toggling an unread book's status");

        test("It should change the book's hasPreviouslyRead", 2, function () {
            eventBus.reset();
            var model = new Book({ thumbnailUrl: "test" });
            model.toggleStatus();
            ok(model.get("hasPreviouslyRead"), "set to true");
            model.toggleStatus();
            ok(!model.get("hasPreviouslyRead"), "set to false");
        });

        asyncTest("It should raise a previousread:addRequested event", 1, function () {
            eventBus.reset();
            var model = new Book({ id: "test" });

            eventBus.on("previousread:addRequested", function (id) {
                equal(id, model.id);
                start();
            });

            model.toggleStatus();
        });

        module("When toggling a previously-read book's status");

        asyncTest("It should raise a previousread:removeRequested event", 1, function () {
            eventBus.reset();
            var model = new Book({ id: "test", hasPreviouslyRead: true });

            eventBus.on("previousread:removeRequested", function (id) {
                equal(id, model.id);
                start();
            });

            model.toggleStatus();
        });

        module("When a previousread:added event is raised");

        test("It should set the matching book's hasPreviouslyRead to true", 2, function () {
            eventBus.reset();
            var aaa = new Book({ thumbnailUrl: "test", id : "aaa" });
            var bbb = new Book({ thumbnailUrl: "test", id : "bbb" });

            eventBus.trigger("previousread:added", "bbb");

            ok(!aaa.get("hasPreviouslyRead"));
            ok(bbb.get("hasPreviouslyRead"));
        });

        module("When a previousread:removed event is raised");

        test("It should set the matching book's hasPreviouslyRead to false", 2, function () {
            eventBus.reset();
            var aaa = new Book({ thumbnailUrl: "test", id : "aaa" });
            var bbb = new Book({
                thumbnailUrl: "test",
                id : "bbb",
                hasPreviouslyRead: true
            });

            eventBus.trigger("previousread:removed", "bbb");

            ok(!aaa.get("hasPreviouslyRead"));
            ok(!bbb.get("hasPreviouslyRead"));
        });

    }(
        Ohb.app,
        Ohb.Router,
        Ohb.eventBus,
        $,
        _,
        Backbone,
        Ohb.MenuBarView,
        Ohb.SearchResultView,
        Ohb.SearchResult,
        Ohb.Book,
        Ohb.SearchResultCollection,
        Ohb.SearchResultCollectionView,
        Ohb.BookDetailsView
    ));
});
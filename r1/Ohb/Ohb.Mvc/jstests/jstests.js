$(function() {
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
        SearchResultCollectionView
    ) {

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

        module("when pressing enter in the search box");

        test("It should raise the searchRequested event", function () {
            eventBus.reset();

            var view = new MenuBarView({ el: $("#qunit-fixture") }), expected = "test search";

            eventBus.bind("searchRequested", function (q) {
                equal(q, expected);
            });

            $("#menubar-search-input").val(expected);
            var e = $.Event("keyup");
            e.which = 13;
            $("#menubar-search-input").trigger(e);
        });

        test("It shouldn't raise any event if the search box is empty", function () {
            eventBus.reset();
            var view = new MenuBarView({ el: $("#qunit-fixture") });

            $("#menubar-search-input").val("");
            var e = $.Event("keyup");
            e.which = 13;

            eventBus.bind("searchRequested", function (q) {
                ok(false, "should not have been raised!");
            });

            $("#menubar-search-input").trigger(e);
        });

        module("when a search fails");

        test("It should render the error modal", 2, function () {
            eventBus.reset();
            app.initialize();

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

        test("clicking inside the search results should not hide the results", 3, function () {
            var el = $("#test-search-results");
            ok(!(el.is(":visible")), "should be hidden to start with");

            var view = new SearchResultCollectionView({ el: el[0] });
            view.render();

            ok(el.is(":visible"), "should become visible");

            $("#test-search-results").trigger("click");

            ok(el.is(":visible"), "should stay visible");
        });

        test("clicking in the menu bar should not hide the results", 3, function () {
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

            eventBus.trigger("searchResultsArrived", results);

            ok($("#test-search-results").is(":visible"), "should become visible");
            equal($("#test-search-results").children().length, 3, "should replace existing results");
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

        test("It should replace any previous results", 6, function () {
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

            eventBus.trigger("searchReturnedNoResults");

            ok(el.is(":visible"), "should become visible");
            ok($(".searchresult-no-results-available").is(":visible"));
            equal(el.children().length, 1, "should replace existing results");
        });

        module("When clicking on a search result");

        test("It should raise a search result selected event", 1, function () {
            eventBus.reset();

            var el = $("#test-search-result");
            var model = new SearchResult({ title: "foo" });
            var view = new SearchResultView({ el: el, model: model });



            eventBus.bind("searchResultSelected", function (sr) {
                equal(sr, model);
            });

            el.trigger("click");
        });

        module("when a searchResultSelected event is raised");

        test("It should raise a search result selected event", 1, function () {
            eventBus.reset();
            app.initialize();
            var model = new SearchResult({ id: "foo" });
            eventBus.trigger("searchResultSelected", model);

            equal(window.location.hash, "#books/foo");
        });

        module("When a Book model has no thumbnail image");

        test("It should use the default placeholder thumbnail image in search results", 1, function() {
            var model = new Book({});
            equal(model.getSearchResultThumbnail(), "img/search-result-no-cover.png");
        });

        test("It should use the default placeholder thumbnail image on the book page", 1, function() {
            var model = new Book({});
            equal(model.getBookThumbnail(), "img/book-no-cover.png");
        });

        module("When a Book model has thumbnails");

        test("It should use the real thumbnail image in search results", 1, function() {
            var model = new Book({ smallThumbnailUrl: "test" });
            equal(model.getSearchResultThumbnail(), "test");
        });

        test("It should use the real thumbnail image on the book page", 1, function() {
            var model = new Book({ thumbnailUrl: "test" });
            equal(model.getBookThumbnail(), "test");
        });
    })(
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
        Ohb.SearchResultCollectionView);
});
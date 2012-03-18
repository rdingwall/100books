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
        mainRegion
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

            var expected = "test search";

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
            Ohb.menuBarView.initialize();

            ok(!($("#search-loader-spinner").is(":visible")), "should be hidden to start with");

            eventBus.trigger("search:began");

            ok($("#search-loader-spinner").is(":visible"));

            $("#search-loader-spinner").hide();
        });

        module("When a search completes");

        test("The ajax loader gif should dissappear", 2, function () {
            eventBus.reset();
            Ohb.menuBarView.initialize();

            $("#search-loader-spinner").show();

            ok(($("#search-loader-spinner").is(":visible")), "should be visible to start with");

            eventBus.trigger("search:completed");

            ok(!($("#search-loader-spinner").is(":visible")));
        });

        module("When search results become available");

        test("They should be rendered", 3, function () {
            eventBus.reset();

            ok(!($("#test-search-results").is(":visible")), "should be hidden to start with");

            var collection = new SearchResultCollection();
            collection.add(new SearchResult({ title: "test book" }));
            collection.add(new SearchResult({ title: "test book 2" }));
            var view = new SearchResultCollectionView({
                el: $("#test-search-results"),
                collection: collection
            }).render();

            ok(view.$el.is(":visible"), "should become visible");
            equal(view.$el.children().length, 2);
        });

        module("When rendering a single search result");

        asyncTest("It should be rendered", 2, function () {

            var view = new SearchResultView({
                el: "#test-search-result",
                model: new SearchResult({
                    title: "Harry Potter",
                    authors: "JK Rowling",
                    smallThumbnailUrl: "http://2.gravatar.com/avatar/87acbe2fc2f40edf8fa5a816515bff9f",
                    id: "42"
                })
            });
            view.render();

            setTimeout(function () {
                equal(view.$el.find(".searchresult-title").text(), "Harry Potter");
                equal(view.$el.find("p.searchresult-authors").text(), "JK Rowling");
                start();
            }, 500);
        });

        module("When the search result box is open");

        test("Clicking anywhere outside should hide it", 4, function () {

            var view = new SearchResultCollectionView({
                el: "#test-search-results"
            });

            ok(!(view.$el.is(":visible")), "should be hidden to start with");

            view.render();

            ok(view.$el.is(":visible"), "should become visible");

            $("body").trigger("click");

            ok(!(view.$el.is(":visible")), "should be hidden again");

            equal(view.views.length, 0, "should clear the items");
        });

        test("Clicking inside the search results should not hide the results", 3, function () {
            var view = new SearchResultCollectionView({
                el: "#test-search-results"
            });

            ok(!(view.$el.is(":visible")), "should be hidden to start with");

            view.render();

            ok(view.$el.is(":visible"), "should become visible");

            view.$el.trigger("click");

            ok(view.$el.is(":visible"), "should stay visible");
        });

        test("Clicking in the menu bar should not hide the results", 3, function () {
            var view = new SearchResultCollectionView({
                el: "#test-search-results"
            });

            ok(!view.$el.is(":visible"), "should be hidden to start with");
            view.render();

            ok(view.$el.is(":visible"), "should become visible");

            $("#menubar").trigger("click");

            ok(view.$el.is(":visible"), "should stay visible");
        });

        module("When starting a new search");

        test("The previous search results should be closed", 1, function () {
            var view = new SearchResultCollectionView({
                el: "#test-search-results"
            });

            view.render();

            eventBus.trigger("search:began");

            ok(!view.$el.is(":visible"), "should hide");
        });

        module("When there are no search results available");

        test("It should display a no search results message", 3, function () {
            var view = new SearchResultCollectionView({
                el: "#test-search-results"
            });

            ok(!view.$el.is(":visible"), "should be hidden to start with");

            view.render();

            ok(view.$el.is(":visible"), "should become visible");
            ok($(".searchresult-no-results-available").is(":visible"));
        });

        module("When clicking on a search result");

        test("It should mark the result as selected", 1, function () {

            var model = new SearchResult({ title: "foo" });
            var view = new SearchResultView({ el: "#test-search-result", model: model });

            view.$el.trigger("click");

            ok(model.get("selected"));
        });

        module("When a search result is marked as selected");

        test("It should raise a search:result:selected event", 1, function () {
            eventBus.reset();

            var model = new SearchResult({ title: "foo" });

            eventBus.on("search:result:selected", function (sr) {
                equal(sr, model);
            });

            model.set("selected", true);
        });

        module("When a search:result:selected event is raised");

        test("It should navigate to the new route", 1, function () {
            eventBus.reset();
            app.initialize();

            var model = new SearchResult({
                id: "foo",
                title: "Harry Potter's amazing #(*@(#)(# adventure$ 2008"
            });

            eventBus.trigger("search:result:selected", model);

            equal(window.location.hash, "#books/foo/harry-potters-amazing-adventure-2008");
        });

        // This one fails when run with the other tests for some reason
        asyncTest("It should raise a book:requested event (RUN SEPARATELY)", 1, function () {
            eventBus.reset();
            app.initialize();
            var model = new SearchResult({ id: "foo" });

            eventBus.on("book:requested", function (id) {
                equal(id, model.id);
            });

            eventBus.trigger("search:result:selected", model);

            setTimeout(start, 1000);
        });

        module("When one of the search result models is marked as selected");

        test("It should close the search results", 4, function () {
            eventBus.reset();
            var model = new SearchResult({});
            var collection = new SearchResultCollection();
            collection.add(model);

            var view = new SearchResultCollectionView({
                el: "#test-search-results",
                collection: collection
            });
            ok(!view.$el.is(":visible"), "should be hidden to start with");
            view.render();

            ok(view.$el.is(":visible"), "should become visible");

            model.set("selected", true);

            ok(!view.$el.is(":visible"), "should be hidden");

            equal(view.views.length, 0, "should clear the items");
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

        test("It should change the book's hasPreviouslyRead attr to true", 1,
            function () {
                var model  = new Book({ thumbnailUrl: "/img/book-no-cover.png" });

                var view = new BookDetailsView({
                    model: model
                });

                view.render();

                view.$el.find(".status-toggle-button.btn-success").trigger("click");
                ok(model.get("hasPreviouslyRead"), "The book should now be previously read");

                view.close();
                view.$el.remove();
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

        module("When changing the content of the main region");

        test("It should call close() on the previous view", 1, function () {

            Ohb.mainRegion.show(new (Backbone.View.extend({
                close: function () { ok(true); }
            }))());

            Ohb.mainRegion.close();
        });

        test("It should clear the previous content", 1, function () {
            Ohb.mainRegion.show(new (Backbone.View.extend({
                render: function () {
                    this.$el.html("<div id='mainregion-clear-test'>hello</div>");
                    return this;
                }
            }))());

            Ohb.mainRegion.close();

            equal($("#mainregion-clear-test").length, 0);
        });

        test("It should render the new content", 1, function () {
            Ohb.mainRegion.show(new (Backbone.View.extend({
                render: function () {
                    this.$el.html("<div id='mainregion-render-test'>hello</div>");
                    return this;
                }
            }))());

            equal($("#mainregion-render-test").text(), "hello");

            Ohb.mainRegion.close();
        });

        module("When showing an error");

        test("It should render the error", 1, function () {
            var msg = "test error message";
            Ohb.mainRegion.showError(msg);

            equal($(".error-message p").text(), msg);

            Ohb.mainRegion.close();
        });

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
        Ohb.mainRegion
    ));
});
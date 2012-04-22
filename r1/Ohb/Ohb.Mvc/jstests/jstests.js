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





        module("When rendering a previous read");

        test("It should render the details", 3, function () {
            var model = new PreviousRead({
                title: "test title",
                authors: "test author",
                smallThumbnailUrl: "test url"
            });

            var view = new PreviousReadView({
                model: model,
                el: "#test-previous-read"
            });

            view.render();

            equal(view.$el.find(".previous-read-title").text(), model.get("title"));
            equal(view.$el.find(".previous-read-authors").text(), model.get("authors"));
            equal(view.$el.find(".previous-read-thumbnail").attr("src"), model.get("smallThumbnailUrl"));
        });

        module("When rendering a previous read collection");

        test("It should render the details of the items", 2, function () {
            var model1 = new PreviousRead({
                title: "title 1",
                id: "1"
            });
            var model2 = new PreviousRead({
                title: "title 2",
                id: "2"
            });

            var collection = new PreviousReadCollection();
            collection.reset([ model1, model2 ]);

            var view = new PreviousReadCollectionView({
                el: "#test-previous-reads",
                collection: collection
            });

            view.render();

            equal(view.$el.find("#previous-read-1").find(".previous-read-title").text(),
                model1.get("title"));
            equal(view.$el.find("#previous-read-2").find(".previous-read-title").text(),
                model2.get("title"));
        });


        module("When rendering a profile card view");

        test("It should render the details", 1, function () {
            var model = new Profile({
                displayName: "test user"
            });

            var view = new ProfileCardView({
                model: model,
                el: "#test-profile-card"
            });

            view.render();

            equal(view.$el.find(".profile-card-display-name").text(), model.get("displayName"));
        });

        module("When rendering a composite profile view");

        test("It should render both the profile card and the previous reads", 2, function () {
            var model = new Profile({
                displayName: "test user"
            });

            var collection = new PreviousReadCollection();
            collection.add(new PreviousRead({
                title: "title 1"
            }));

            var view = new CompositeProfileView({
                el: "#test-composite-profile",
                previousReadCollection: collection,
                profileModel: model
            });

            view.render();

            equal(view.$el.find(".profile-card-display-name").text(), model.get("displayName"));
            equal(view.$el.find(".previous-read-title").text(), collection.at(0).get("title"));
        });

        module("When requesting PreviousRead be removed");

        test("It should raise a previousread:removeRequested event", 1, function () {
            eventBus.reset();
            var model = new PreviousRead({ id: "aaa" });

            eventBus.on("previousread:removeRequested", function (id) {
                equal(id, model.id);
            });

            model.remove();
        });

        module("When a previousread:removed event is raised");

        test("The matching previousread should remove itself from it's parent collection", 1, function () {
            eventBus.reset();

            var collection = new PreviousReadCollection();
            collection.add(new PreviousRead({ id: "aaa" }));

            eventBus.trigger("previousread:removed", "aaa");

            equal(collection.length, 0);
        });

        asyncTest("It should remove the view", 2, function () {
            eventBus.reset();

            var model1 = new PreviousRead({
                title: "title 1",
                id: "1"
            });
            var model2 = new PreviousRead({
                title: "title 2",
                id: "2"
            });

            var collection = new PreviousReadCollection();
            collection.reset([ model1, model2 ]);

            var view = new PreviousReadCollectionView({
                el: "#test-previous-reads",
                collection: collection
            });

            view.render();

            equal(view.$el.find("#previous-read-1").find(".previous-read-title").text(),
                model1.get("title"));

            eventBus.trigger("previousread:removed", model1.id);

            setTimeout(function () {
                equal(view.$el.find("#previous-read-1").length, 0);
                start();
            }, 1000);
        });

        module("When clicking the 'remove' button in the previous reads list");

        test("It should raise a previousreads:removeRequested event", 1, function () {
            eventBus.reset();

            var model1 = new PreviousRead({
                title: "title 1",
                id: "1"
            });
            var model2 = new PreviousRead({
                title: "title 2",
                id: "2"
            });

            var collection = new PreviousReadCollection();
            collection.reset([ model1, model2 ]);

            var view = new PreviousReadCollectionView({
                el: "#test-previous-reads",
                collection: collection
            });

            view.render();

            eventBus.on("previousread:removeRequested", function (id) {
                equal(id, model1.id);
            });

            $($(".btn-remove-previousread")[0]).trigger("click");
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
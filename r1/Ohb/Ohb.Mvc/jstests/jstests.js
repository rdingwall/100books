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
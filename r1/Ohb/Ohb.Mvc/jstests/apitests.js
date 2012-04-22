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
        Backbone,
        Book,
        SearchResult,
        eventBus,
        app,
        Profile,
        PreviousReadCollection,
        mainRegion
    ) {

        QUnit.config.testTimeout = 2000;

        var log = $.jog("ApiTests");

        log.info("hiya");

        module("(Setup) Requesting auth cookie...");

        asyncTest("It should return a successful HTTP status code", function () {

            $.ajax({
                type: "POST",
                url: "http://localhost/api/backdoor/createuser",
                data: { setAuthCookie: true },
                error: function () {
                    ok(false, "POST failed");
                    start();
                },
                success: start
            });
        });

        module("When a book:requested event is raised");

        asyncTest("It should fetch the book details and render them", 1, function () {

            eventBus.reset();
            app.initialize();
            mainRegion.off();

            mainRegion.on("view:changed", function (view) {
                if ($(view.el).hasClass("book-details")) {
                    equal($("h1.book-details-title").text(), "The Google story");
                    start();
                }
            });

            eventBus.trigger("book:requested", "4YydO00I9JYC");
        });

        module("When a book:requested event is raised and the fetch fails");

        asyncTest("It should render an error message", 1, function () {

            eventBus.reset();
            app.initialize();
            mainRegion.off();

            mainRegion.on("view:changed", function (view) {
                if ($(view.el).hasClass("error-message")) {
                    equal($(".error-message").length, 1);
                    start();
                }
            });

            eventBus.trigger("book:requested", "xxxx-fake-id");
        });

        module("When navigating to the book hash");

        asyncTest("It should fetch the book details and render them", 1, function () {
            eventBus.reset();
            app.initialize();
            mainRegion.off();

            mainRegion.on("view:changed", function (view) {
                if ($(view.el).hasClass("book-details")) {
                    equal($("h1.book-details-title").text(), "The Google story");
                    start();
                }
            });

            window.location.hash = "books/4YydO00I9JYC/test-slug";
        });

        module("When a previousread:addRequested event is raised");

        asyncTest("It should raise a previousread:added event", 1, function () {
            eventBus.reset();
            app.initialize();
            mainRegion.off();

            eventBus.on("previousread:added", function (id) {
                equal(id, "4YydO00I9JYC");
                start();
            });

            eventBus.trigger("previousread:addRequested", "4YydO00I9JYC");
        });

        asyncTest("It should mark the book as read via the API", 1, function () {
            eventBus.reset();
            app.initialize();
            mainRegion.off();

            eventBus.on("previousread:added", function () {
                $.getJSON("/api/v1/books/4YydO00I9JYC", function (data) {
                    ok(data.hasPreviouslyRead);
                    start();
                });
            });

            eventBus.trigger("previousread:addRequested", "4YydO00I9JYC");
        });

        module("When a previousread:removeRequested event is raised");

        asyncTest("It should raise a previousread:removed event", 1, function () {
            eventBus.reset();
            app.initialize();
            mainRegion.off();

            eventBus.on("previousread:removed", function (id) {
                equal(id, "4YydO00I9JYC");
                start();
            });

            eventBus.trigger("previousread:removeRequested", "4YydO00I9JYC");
        });

        asyncTest("It should mark the book as unread via the API", 1, function () {
            eventBus.reset();
            app.initialize();
            mainRegion.off();

            eventBus.on("previousread:removed", function () {
                $.getJSON("/api/v1/books/4YydO00I9JYC", function (data) {
                    ok(!data.hasPreviouslyRead);
                    start();
                });
            });

            eventBus.trigger("previousread:removeRequested", "4YydO00I9JYC");
        });

        var withNewUser = function (callback, userDisplayName) {
            $.ajax({
                type: "POST",
                url: "http://localhost/api/backdoor/createuser",
                data: {
                    displayName: userDisplayName || "Test user (JS API tests)",
                    profileImageUrl: "test url"
                },
                error: function () {
                    ok(false, "Setup: create user via backdoor failed!");
                    start();
                },
                success: function (data) {
                    callback(data.userId);
                }
            });
        };

        module("When getting a profile by ID");

        asyncTest("It should retrieve and populate the profile from the server", function () {
            var userDisplayName = "Test user for profile JS model fetch";
            withNewUser(function (userId) {
                var model = new Profile({ id: userId});

                model.fetch({ success: function (model) {
                    equal(model.id, userId);
                    equal(model.get("displayName"), userDisplayName);
                    equal(model.get("profileImageUrl"), "test url");
                    start();
                }});
            }, userDisplayName);
        });

        module("When a myprofile:requested event is raised");

        asyncTest("It should fetch the current user's profile details and render them", 1,
            function () {

                eventBus.reset();
                app.initialize();
                mainRegion.close();
                mainRegion.off();

                mainRegion.on("view:changed", function (view) {
                    if ($(view.el).hasClass("profile")) {
                        equal($("h1.profile-card-display-name").length, 1);
                        start();
                    }
                });

                eventBus.trigger("myprofile:requested");
            });

        module("When fetching a previous read collection");

        asyncTest("It should fetch the previous reads", 5, function () {

            eventBus.reset();
            app.initialize();
            mainRegion.off();

            eventBus.on("previousread:added", function () {
                var results = new PreviousReadCollection();

                results.fetch({
                    success: function (collection) {
                        ok(collection.length > 0);
                        var model = collection.get("4YydO00I9JYC");

                        equal(model.get("title"), "The Google story");
                        equal(model.get("authors"), "David A. Vise, Mark Malseed");
                        equal(model.get("publishedYear"), "2005");
                        equal(model.get("smallThumbnailUrl"), "http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=5&source=gbs_api");

                        start();
                    },
                    error: function () {
                        ok(false);
                        start();
                    }
                });
            });

            eventBus.trigger("previousread:addRequested", "4YydO00I9JYC");
        });


        module("When a searchRequested event is raised");

        asyncTest("It should perform a search and render the results", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:completed", function () {
                equal($("#search-results").children().length, 10);
                start();
            });

            eventBus.on("search:failed", function () {
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

            eventBus.on("search:completed", function () {
                ok(true);
                start();
            });

            eventBus.trigger("search:requested", "harry potter");
        });

        module("When the book search fails");

        asyncTest("It should raise a search:failed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:failed", function () {
                ok(true);
                start();
            });

            eventBus.on("search:resultsArrived", function () {
                ok(false, "should not have been raised (but it's ok, not sure how to fake a test right now)");
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

        asyncTest("It should raise a search:completed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:completed", function () {
                ok(true);
                start();
            });

            eventBus.on("search:resultsArrived", function () {
                ok(false, "should not have been raised!");
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

        module("When requesting a search and there was no results");

        asyncTest("It should render the no results available view", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:failed", function () {
                ok(false, "searchFailed was raised");
                start();
            });

            eventBus.on("search:completed", function () {
                ok($(".searchresult-no-results-available").is(":visible"));
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

        asyncTest("It should raise a search:completed event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("search:failed", function () {
                ok(false, "searchFailed was raised");
                start();
            });

            eventBus.on("search:completed", function () {
                ok(true);
                start();
            });

            eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
        });

    }(
        $,
        Backbone,
        Ohb.Models.Book,
        Ohb.Models.SearchResult,
        Ohb.eventBus,
        Ohb.app,
        Ohb.Models.Profile,
        Ohb.Collections.PreviousReadCollection,
        Ohb.mainRegion
    ));
});
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
        $,
        Backbone,
        Book,
        SearchResult,
        eventBus,
        app,
        Profile
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

        module("When getting a book by ID");

        asyncTest("It should retrieve and populate the book from the server", function () {

            var model = new Book({ id: "4YydO00I9JYC"});

            model.fetch({ success: function (model) {
                equal(model.id, "4YydO00I9JYC");
                equal(model.get("publisher"), "Delacorte Press");
                equal(model.get("title"), "The Google story");
                equal(model.get("authors"), "David A. Vise, Mark Malseed");
                equal(model.get("publishedYear"), "2005");
                equal(model.get("description"), "\"Here is the story behind one of the most remarkable Internet successes of our time. Based on scrupulous research and extraordinary access to Google, the book takes you inside the creation and growth of a company whose name is a favorite brand and a standard verb recognized around the world. Its stock is worth more than General Motors’ and Ford’s combined, its staff eats for free in a dining room that used to be<b> </b>run<b> </b>by the Grateful Dead’s former chef, and its employees traverse the firm’s colorful Silicon Valley campus on scooters and inline skates.<br><br><b>THE GOOGLE STORY </b>is the definitive account of the populist media company powered by the world’s most advanced technology that in a few short years has revolutionized access to information about everything for everybody everywhere. <br>In 1998, Moscow-born Sergey Brin and Midwest-born Larry Page dropped out of graduate school at Stanford University to, in their own words, “change the world” through a search engine that would organize every bit of information on the Web for free.<br><br>While the company has done exactly that in more than one hundred languages, Google’s quest continues as it seeks to add millions of library books, television broadcasts, and more to its searchable database. <br>Readers will learn about the amazing business acumen and computer wizardry that started the company on its astonishing course; the secret network of computers delivering lightning-fast search results; the unorthodox approach that has enabled it to challenge Microsoft’s dominance and shake up Wall Street. Even as it rides high, Google wrestles with difficult choices that will enable it to continue expanding while sustaining the guiding vision of its founders’ mantra: DO NO EVIL.\"");
                equal(model.get("pageCount"), "333");
                equal(model.get("thumbnailUrl"), "http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=1&source=gbs_api");
                equal(model.get("smallThumbnailUrl"), "http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=5&source=gbs_api");
                start();
            }});
        });

        module("When a book:requested event is raised");

        asyncTest("It should fetch the book details and render them", 1, function () {

            eventBus.reset();
            app.initialize();

            eventBus.on("book:rendered", function () {
                equal($("div.book-details h1").text(), "The Google story");
                start();
            });

            eventBus.trigger("book:requested", "4YydO00I9JYC");
        });

        module("When a book:requested event is raised and the fetch fails");

        asyncTest("It should fetch the book details and render them", 1, function () {

            eventBus.reset();
            app.initialize();

            eventBus.on("book:fetchError", function () {
                ok($("div.book-details-error").is(":visible"));
                start();
            });

            eventBus.trigger("book:requested", "xxxx-fake-id");
        });

        module("When navigating to the book hash");

        asyncTest("It should fetch the book details and render them", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("book:rendered", function () {
                equal($("div.book-details h1").text(), "The Google story");
                start();
            });

            window.location.hash = "books/4YydO00I9JYC/test-slug";
        });

        module("When a previousread:addRequested event is raised");

        asyncTest("It should raise a previousread:added event", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("previousread:added", function (id) {
                equal(id, "4YydO00I9JYC");
                start();
            });

            eventBus.trigger("previousread:addRequested", "4YydO00I9JYC");
        });

        asyncTest("It should mark the book as read via the API", 1, function () {
            eventBus.reset();
            app.initialize();

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

            eventBus.on("previousread:removed", function (id) {
                equal(id, "4YydO00I9JYC");
                start();
            });

            eventBus.trigger("previousread:removeRequested", "4YydO00I9JYC");
        });

        asyncTest("It should mark the book as unread via the API", 1, function () {
            eventBus.reset();
            app.initialize();

            eventBus.on("previousread:removed", function () {
                $.getJSON("/api/v1/books/4YydO00I9JYC", function (data) {
                    ok(!data.hasPreviouslyRead);
                    start();
                });
            });

            eventBus.trigger("previousread:removeRequested", "4YydO00I9JYC");
        });

        module("When getting a profile by ID");

        asyncTest("It should retrieve and populate the profile from the server", function () {
            $.ajax({
                type: "POST",
                url: "http://localhost/api/backdoor/createuser",
                data: { displayName: "Test user for profile JS model fetch" },
                error: function () {
                    ok(false, "POST failed");
                    start();
                },
                success: function (data) {
                    var userId = data.userId;

                    var model = new Profile({ id: userId});

                    model.fetch({ success: function (model) {
                        equal(model.id, userId);
                        equal(model.get("displayName"), "Test user for profile JS model fetch");
                        start();
                    }});
                }
            });
        });

    }($, Backbone, Ohb.Book, Ohb.SearchResult, Ohb.eventBus, Ohb.app, Ohb.Profile));
});
/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false,
 runs: false,
 runsAsync: false,
 should: false,
 chai: false
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

        var should = chai.should();
        var expect = chai.expect;

        var withNewUser = function (callback, done, userDisplayName) {
            $.ajax({
                type: "POST",
                url: "http://localhost/api/backdoor/createuser",
                data: {
                    displayName: userDisplayName || "Test user (JS API tests)",
                    profileImageUrl: "test url"
                },
                error: function (err) {
                    done(err);
                },
                success: function (data) {
                    callback(data.userId);
                }
            });
        };

        describe("Logged in", function () {

            describe("(Setup) Requesting auth cookie", function () {
                it("should return a successful HTTP status code", function (done) {
                    $.ajax({
                        type: "POST",
                        url: "/api/backdoor/createuser",
                        data: { setAuthCookie: true },
                        error: function (err) {
                            should.fail(err);
                            done();
                        },
                        success: function () {
                            done();
                        }
                    });
                });
            });

            describe("Firing a book:requested event", function () {
                it("should fetch the book details and render them", function (done) {
                    app.initialize();
                    eventBus.off();

                    eventBus.on("mainRegion:view:changed", function (view) {
                        if ($(view.el).hasClass("book-details")) {
                            $("h1.book-details-title").should.have.text("The Google Story");
                            done();
                        }
                    });

                    eventBus.trigger("book:requested", "4YydO00I9JYC");
                });
            });

            describe("Firing a book:requested event (that fails)", function (done) {
                it("should render an error message", function (done) {
                    app.initialize();
                    eventBus.off();

                    eventBus.on("mainRegion:view:changed", function (view) {
                        if ($(view.el).hasClass("error-message")) {
                            $(".error-message").should.have.length(1);
                            done();
                        }
                    });

                    eventBus.trigger("book:requested", "xxxx-fake-id");
                });
            });


            describe("Navigating to the book hash", function () {
                it("should fetch the book details and render them", function (done) {
                    app.initialize();
                    eventBus.off();

                    eventBus.on("mainRegion:view:changed", function (view) {
                        if ($(view.el).hasClass("book-details")) {
                            $("h1.book-details-title").should.have.text("The Google story");
                            done();
                        }
                    });

                    window.location.hash = "books/4YydO00I9JYC/test-slug";
                });
            });

            describe("Firing a previousread:addRequested event", function () {
                it("should raise a previousread:added event", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("previousread:added", function (id) {
                        id.should.equal("4YydO00I9JYC");
                        done();
                    });

                    eventBus.trigger("previousread:addRequested", "4YydO00I9JYC");
                });

                it("should mark the book as read via the API", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("previousread:added", function () {
                        $.getJSON("/api/v1/books/4YydO00I9JYC", function (data) {
                            data.hasPreviouslyRead.should.be.true();
                            done();
                        });
                    });

                    eventBus.trigger("previousread:addRequested", "4YydO00I9JYC");
                });
            });

        });


        describe("Firing a previousread:removeRequested event", function () {

            it("should raise a previousread:removed event", function (done) {
                eventBus.reset();
                app.initialize();

                eventBus.on("previousread:removed", function (id) {
                    id.should.equal("4YydO00I9JYC");
                    done();
                });

                eventBus.trigger("previousread:removeRequested", "4YydO00I9JYC");
            });

            it("should mark the book as unread via the API", function (done) {
                eventBus.reset();
                app.initialize();

                eventBus.on("previousread:removed", function () {
                    $.getJSON("/api/v1/books/4YydO00I9JYC", function (data) {
                        data.hasPreviouslyRead.should.be.false();
                        done();
                    });
                });

                eventBus.trigger("previousread:removeRequested", "4YydO00I9JYC");
            });

            describe("Getting a profile by ID", function () {

                it("should retrieve and populate the profile from the server", function (done) {
                    var userDisplayName = "Test user for profile JS model fetch";
                    withNewUser(function (userId) {
                        var model = new Profile({ id: userId});

                        model.fetch({ success: function (model) {
                            model.id.should.equal(userId);
                            model.get("displayName").should.equal(userDisplayName);
                            model.get("profileImageUrl").should.equal("test url");
                            done();
                        }});
                    }, done, userDisplayName);
                });

            });

            describe("Firing a myprofile:requested event", function () {

                it("should fetch the current user's profile details and render them",
                    function (done) {

                        eventBus.reset();
                        app.initialize();
                        mainRegion.close();

                        eventBus.on("mainRegion:view:changed", function (view) {
                            if ($(view.el).hasClass("profile")) {
                                $("h1.profile-card-display-name").should.have.length(1);
                                done();
                            }
                        });

                        eventBus.trigger("myprofile:requested");
                    });

            });

            describe("Fetching a previous read collection", function () {

                it("should fetch the previous reads", function (done) {

                    eventBus.reset();
                    app.initialize();

                    eventBus.on("previousread:added", function () {
                        var results = new PreviousReadCollection();

                        results.fetch({
                            success: function (collection) {
                                collection.length.should.be.above(0);
                                var model = collection.get("4YydO00I9JYC");

                                model.get("title").should.equal("The Google Story");
                                model.get("authors").should.equal("David A. Vise, Mark Malseed");
                                model.get("publishedYear").should.equal("2005");
                                model.get("smallThumbnailUrl").should.contain("http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=5&source=gbs_api");

                                done();
                            },
                            error: function (err) {
                                done(err);
                            }
                        });
                    });

                    eventBus.trigger("previousread:addRequested", "4YydO00I9JYC");
                });

            });


            describe("Firing a search:requested event", function () {

                it("should perform a search and render the results", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("search:completed", function () {
                        $("#search-results").children().should.have.length(10);
                        done();
                    });

                    eventBus.on("search:failed", function () {
                        should.fail();
                        done();
                    });

                    eventBus.trigger("search:requested", "harry potter");
                });

                it("should raise a search:began event", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("search:began", function (q) {
                        q.should.equal("harry potter");
                        done();
                    });

                    eventBus.trigger("search:requested", "harry potter");
                });

                it("should raise a search:completed event", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("search:completed", function () {
                        done();
                    });

                    eventBus.trigger("search:requested", "harry potter");
                });

            });

            describe("The book search failing", function () {
                it("should raise a search:failed event", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("search:failed", function () {
                        done();
                    });

                    eventBus.on("search:resultsArrived", function () {
                        should.fail("should not have been raised (but it's ok, not sure how to fake a test right now)");
                        done();
                    });

                    eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
                });

                it("should raise a search:completed event", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("search:completed", function () {
                        done();
                    });

                    eventBus.on("search:resultsArrived", function () {
                        should.fail("should not have been raised!");
                        done();
                    });

                    eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
                });

            });

            describe("Requesting a search with no results", function () {

                it("should render the no results available view", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("search:failed", function () {
                        should.fail("searchFailed was raised");
                        done();
                    });

                    eventBus.on("search:completed", function () {
                        $(".searchresult-no-results-available").should.be.visible();
                        done();
                    });

                    eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
                });

                it("should raise a search:completed event", function (done) {
                    eventBus.reset();
                    app.initialize();

                    eventBus.on("search:failed", function () {
                        should.fail("searchFailed was raised");
                        done();
                    });

                    eventBus.on("search:completed", function () {
                        done();
                    });

                    eventBus.trigger("search:requested", "3894h9f893jhf934jf92ht8");
                });
            });


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
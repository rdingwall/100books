/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false,
 runs: false,
 runsAsync: false
 */

$(function () {
    "use strict";

    (function (
        Book,
        eventBus
    ) {
        describe("Book", function () {

            describe("Books with thumbnails", function () {
                it("should use the real thumbnail image in search results", function () {
                    var model = new Book({ smallThumbnailUrl: "test" });
                    expect(model.getSearchResultThumbnail()).toEqual("test");
                });
            });

            describe("Toggling an unread book's status", function () {

                it("should change the book's hasPreviouslyRead", function () {
                    var model = new Book({ thumbnailUrl: "test" });
                    model.toggleStatus();
                    expect(model.get("hasPreviouslyRead")).toBeTruthy();
                    model.toggleStatus();
                    expect(model.get("hasPreviouslyRead")).toBeFalsy();
                });


                it("should raise a previousread:addRequested event", function () {
                    runsAsync(function (callback) {
                        var model = new Book({ id: "test" });

                        eventBus.on("previousread:addRequested", function (id) {
                            expect(id).toEqual(model.id);
                            callback();
                        });

                        model.toggleStatus();
                    });
                });
            });

            describe("Toggling a previously-read book's status", function () {
                it("should raise a previousread:removeRequested event", function () {
                    runsAsync(function (callback) {
                        var model = new Book({ id: "test", hasPreviouslyRead: true });

                        eventBus.on("previousread:removeRequested", function (id) {
                            expect(id).toEqual(model.id);
                            callback();
                        });

                        model.toggleStatus();
                    }, 2000);
                });
            });

            describe("Firing a previousread:added event", function () {
                it("should set the matching book's hasPreviouslyRead to true", function () {
                    var aaa = new Book({ thumbnailUrl: "test", id : "aaa" });
                    var bbb = new Book({ thumbnailUrl: "test", id : "bbb" });

                    eventBus.trigger("previousread:added", "bbb");

                    expect(aaa.get("hasPreviouslyRead")).toBeFalsy();
                    expect(bbb.get("hasPreviouslyRead")).toBeTruthy();
                });
            });

            describe("Firing a previousread:removed event is raised", function () {
                it("should set the matching book's hasPreviouslyRead to false", function () {
                    var aaa = new Book({ thumbnailUrl: "test", id : "aaa" });
                    var bbb = new Book({
                        thumbnailUrl: "test",
                        id : "bbb",
                        hasPreviouslyRead: true
                    });

                    eventBus.trigger("previousread:removed", "bbb");

                    expect(aaa.get("hasPreviouslyRead")).toBeFalsy();
                    expect(bbb.get("hasPreviouslyRead")).toBeFalsy();
                });
            });

            describe("Fetching a book by ID from the API", function () {

                it("should retrieve and populate the book from the server", function () {
                    runsAsync(function (callback) {
                        var model = new Book({ id: "4YydO00I9JYC"});

                        model.fetch({ success: function (model) {
                            expect(model.id).toEqual("4YydO00I9JYC");
                            expect(model.get("publisher")).toEqual("Delacorte Press");
                            expect(model.get("title")).toEqual("The Google Story");
                            expect(model.get("authors")).toEqual("David A. Vise, Mark Malseed");
                            expect(model.get("publishedYear")).toEqual("2005");
                            expect(model.get("description")).toEqual("\"Here is the story behind one of the most remarkable Internet successes of our time. Based on scrupulous research and extraordinary access to Google, the book takes you inside the creation and growth of a company whose name is a favorite brand and a standard verb recognized around the world. Its stock is worth more than General Motors’ and Ford’s combined, its staff eats for free in a dining room that used to be<b> </b>run<b> </b>by the Grateful Dead’s former chef, and its employees traverse the firm’s colorful Silicon Valley campus on scooters and inline skates.<br><br><b>THE GOOGLE STORY </b>is the definitive account of the populist media company powered by the world’s most advanced technology that in a few short years has revolutionized access to information about everything for everybody everywhere. <br>In 1998, Moscow-born Sergey Brin and Midwest-born Larry Page dropped out of graduate school at Stanford University to, in their own words, “change the world” through a search engine that would organize every bit of information on the Web for free.<br><br>While the company has done exactly that in more than one hundred languages, Google’s quest continues as it seeks to add millions of library books, television broadcasts, and more to its searchable database. <br>Readers will learn about the amazing business acumen and computer wizardry that started the company on its astonishing course; the secret network of computers delivering lightning-fast search results; the unorthodox approach that has enabled it to challenge Microsoft’s dominance and shake up Wall Street. Even as it rides high, Google wrestles with difficult choices that will enable it to continue expanding while sustaining the guiding vision of its founders’ mantra: DO NO EVIL.\"");
                            expect(model.get("pageCount")).toEqual(333);
                            expect(model.get("thumbnailUrl")).toStartWith("http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=1");
                            expect(model.get("smallThumbnailUrl")).toStartWith("http://bks2.books.google.co.uk/books?id=4YydO00I9JYC&printsec=frontcover&img=1&zoom=5");
                            expect(model.get("viewUrl")).toEqual("#books/4YydO00I9JYC/the-google-story");
                            callback();
                        }});
                    });
                });

            });
        });
    }(
        Ohb.Models.Book,
        Ohb.eventBus
    ));
});
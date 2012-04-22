/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false,
 runs: false
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

        });
    }(
        Ohb.Models.Book,
        Ohb.eventBus
    ));
});
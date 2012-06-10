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
        Readable,
        eventBus
    ) {
        describe("Readable", function () {

            describe("Toggling an unread Readable's status", function () {

                it("should change the Readable's hasPreviouslyRead", function () {
                    var model = new Readable({});
                    model.toggleStatus();
                    expect(model.get("hasPreviouslyRead")).toBeTruthy();
                    model.toggleStatus();
                    expect(model.get("hasPreviouslyRead")).toBeFalsy();
                });


                it("should raise a previousread:addRequested event", function () {
                    runsAsync(function (callback) {
                        var model = new Readable({ id: "test" });

                        eventBus.on("previousread:addRequested", function (id) {
                            expect(id).toEqual(model.id);
                            callback();
                        });

                        model.toggleStatus();
                    });
                });
            });

            describe("Toggling a previously-read Readable's status", function () {
                it("should raise a previousread:removeRequested event", function () {
                    runsAsync(function (callback) {
                        var model = new Readable({ id: "test", hasPreviouslyRead: true });

                        eventBus.on("previousread:removeRequested", function (id) {
                            expect(id).toEqual(model.id);
                            callback();
                        });

                        model.toggleStatus();
                    }, 2000);
                });
            });

            describe("Firing a previousread:added event", function () {
                it("should set the matching Readable's hasPreviouslyRead to true", function () {
                    var aaa = new Readable({ id : "aaa" });
                    var bbb = new Readable({ id : "bbb" });

                    eventBus.trigger("previousread:added", "bbb");

                    expect(aaa.get("hasPreviouslyRead")).toBeFalsy();
                    expect(bbb.get("hasPreviouslyRead")).toBeTruthy();
                });
            });

            describe("Firing a previousread:removed event is raised", function () {
                it("should set the matching Readable's hasPreviouslyRead to false", function () {
                    var aaa = new Readable({ id : "aaa" });
                    var bbb = new Readable({ id : "bbb", hasPreviouslyRead: true });

                    eventBus.trigger("previousread:removed", "bbb");

                    expect(aaa.get("hasPreviouslyRead")).toBeFalsy();
                    expect(bbb.get("hasPreviouslyRead")).toBeFalsy();
                });
            });
        });
    }(
        Ohb.Models.Readable,
        Ohb.eventBus
    ));
});
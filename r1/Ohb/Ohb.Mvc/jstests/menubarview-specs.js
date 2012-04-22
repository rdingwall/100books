/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false
 */

$(function () {
    "use strict";

    (function (
        $,
        eventBus
    ) {
        describe("MenuBarView", function () {

            describe("Pressing enter in the search box", function () {
                it("should raise the search:requested event", function () {
                    var expected = "test search";

                    eventBus.on("search:requested", function (q) {
                        expect(q).toEqual(expected);
                    });

                    $("#menubar-search-input").val(expected);
                    var e = $.Event("keyup");
                    e.which = 13;
                    $("#menubar-search-input").trigger(e);
                });
            });

            describe("Pressing enter in an empty search box", function () {
                it("should not raise search:requested if the search box is empty", function () {
                    $("#menubar-search-input").val("");
                    var e = $.Event("keyup");
                    e.which = 13;

                    eventBus.on("search:requested", function (q) {
                        expect(false).toBeTruthy(); // should not have been raised
                    });

                    $("#menubar-search-input").trigger(e);
                });
            });

        });
    }(
        $,
        Ohb.eventBus
    ));
});
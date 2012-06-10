/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false,
 runsAsync: false
 */

$(function () {
    "use strict";

    (function (
        $,
        PreviousRead,
        eventBus
    ) {

        describe("PreviousRead", function () {

            describe("Requesting PreviousRead be removed", function () {
                it("should raise a previousread:removeRequested event", function () {
                    runsAsync(function (callback) {
                        var model = new PreviousRead({ id: "aaa" });

                        eventBus.on("previousread:removeRequested", function (id) {
                            expect(id).toEqual(model.id);
                            callback();
                        });

                        model.remove();
                    });
                });
            });
        });

    }(
        $,
        Ohb.Models.PreviousRead,
        Ohb.eventBus
    ));
});
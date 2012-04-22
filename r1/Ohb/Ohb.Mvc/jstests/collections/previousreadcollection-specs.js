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
        eventBus,
        PreviousReadCollection
    ) {

        describe("PreviousReadCollection", function () {

            describe("Firing a previousread:removed event", function () {
                it("should remove the matching previousread the collection", function () {
                    var collection = new PreviousReadCollection();
                    collection.add(new PreviousRead({ id: "aaa" }));

                    eventBus.trigger("previousread:removed", "aaa");

                    expect(collection).toBeEmpty();
                });
            });

        });

    }(
        $,
        Ohb.Models.PreviousRead,
        Ohb.eventBus,
        Ohb.Collections.PreviousReadCollection
    ));
});
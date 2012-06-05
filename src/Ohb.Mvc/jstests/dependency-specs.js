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
        _,
        Backbone,
        eventBus
    ) {
        describe("Dependencies", function () {

            describe("Each javascript file", function () {

                it("should have the OHB event bus available", function () {
                    expect(eventBus).toBeTruthy();
                });

                it("should have extended the OHB event bus with Backbone.js events", function () {
                    expect(eventBus.bind).toBeTruthy();
                });

                it("should have jQuery available", function () {
                    expect($).toBeTruthy();
                });

                it("should have underscore.js available", function () {
                    expect(_).toBeTruthy();
                });

                it("should have backbone.js available", function () {
                    expect(Backbone).toBeTruthy();
                });

            });
        });
    }(
        $,
        _,
        Backbone,
        Ohb.eventBus
    ));
});
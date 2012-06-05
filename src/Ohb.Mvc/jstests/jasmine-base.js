/*global window: false, document: false, $: false, log: false, bleep: false,
 it: false,
 beforeEach: false,
 afterEach: false,
 describe: false,
 expect: false,
 jasmine: false
 */

$(function () {
    "use strict";

    var jasmineEnv = jasmine.getEnv();
    jasmineEnv.updateInterval = 1000;

    var htmlReporter = new jasmine.HtmlReporter();

    jasmineEnv.addReporter(htmlReporter);
    jasmineEnv.addReporter(new jasmine.TeamcityReporter());

    jasmineEnv.specFilter = function (spec) {
        return htmlReporter.specFilter(spec);
    };

    jasmineEnv.beforeEach(function () {
        this.addMatchers({
            toBeVisible: function () {
                return $(this.actual).is(":visible");
            },
            toBeHidden: function () {
                return !$(this.actual).is(":visible");
            },
            toBeEmpty: function () {
                return this.actual.length === 0;
            },
            toStartWith: function (expected) {
                return this.actual.indexOf(expected) === 0;
            }
        });

        Ohb.eventBus.reset();
    });

    jasmineEnv.afterEach(function () {
        $("#fixture").empty();
    });

    jasmineEnv.execute();
});
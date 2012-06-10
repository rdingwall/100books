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
        eventBus,
        app,
        SearchResult
    ) {
        describe("App", function () {

            beforeEach(function () {
                Ohb.menuBarView.initialize();
                $("#fixture").html($("#app-tests").text());
            });

            describe("Firing a search:failed event", function () {

                it("should render the 'search failed' error modal", function () {
                    app.initialize();

                    expect("#search-failed-modal").toBeHidden();

                    eventBus.trigger("search:failed");

                    expect("#search-failed-modal").toBeVisible();

                    $("#search-failed-modal").hide();
                });
            });
        });
    }(
        $,
        Ohb.eventBus,
        Ohb.app,
        Ohb.Models.SearchResult
    ));
});